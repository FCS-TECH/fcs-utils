// ***********************************************************************
// Assembly         : FCS.Lib.Utility
// Author           : 
// Created          : 2023-10-02
// 
// Last Modified By : root
// Last Modified On : 2023-10-13 07:33
// ***********************************************************************
// <copyright file="TaxIdFormatValidator.cs" company="FCS">
//     Copyright (C) 2015 - 2023 FCS Fredes Computer Service.
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Affero General Public License as
//     published by the Free Software Foundation, either version 3 of the
//     License, or (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Affero General Public License for more details.
// 
//     You should have received a copy of the GNU Affero General Public License
//     along with this program.  If not, see [https://www.gnu.org/licenses]
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Linq;
using System.Text.RegularExpressions;

namespace FCS.Lib.Utility;

/// <summary>
///     Vat format validator
/// </summary>
public static class TaxIdFormatValidator
{
    // https://ec.europa.eu/taxation_customs/vies/faqvies.do#item_11
    // https://ec.europa.eu/taxation_customs/vies/
    //https://www.bolagsverket.se/apierochoppnadata.2531.html

    private static readonly Regex DigitsOnly = new(@"[^\d]");


    /// <summary>
    ///     Check vat number format
    /// </summary>
    /// <param name="countryCode"></param>
    /// <param name="taxId"></param>
    /// <returns>bool indicating if the vat number conform to country specification</returns>
    public static bool CheckTaxId(string countryCode, string taxId)
    {
        if (string.IsNullOrWhiteSpace(taxId))
            return false;

        var sanitizeTaxId = SanitizeTaxId(taxId);

        return countryCode.ToUpperInvariant() switch
        {
            "DK" => ValidateTaxIdDenmark(sanitizeTaxId),
            "NO" => ValidateTaxIdNorway(sanitizeTaxId),
            "SE" => ValidateTaxIdSweden(sanitizeTaxId),
            _ => false
        };
    }


    /// <summary>
    ///     sanitize vat number
    /// </summary>
    /// <param name="taxId"></param>
    /// <returns>sanitized string</returns>
    public static string SanitizeTaxId(string taxId)
    {
        return string.IsNullOrWhiteSpace(taxId) ? "" : DigitsOnly.Replace(taxId, "");
    }


    private static bool ValidateTaxIdDenmark(string taxId)
    {
        // https://wiki.scn.sap.com/wiki/display/CRM/Denmark
        // 8 digits 0 to 9
        // C1..C7
        // C8 check-digit MOD11
        // C1 > 0
        // R = (2*C1 + 7*C2 + 6*C3 + 5*C4 + 4*C5 + 3*C6 + 2*C7 + C8)
        if (taxId.Length == 8 && long.TryParse(taxId, out _))
            return ValidateModulus11(taxId);
        return false;
    }


    private static bool ValidateTaxIdNorway(string taxId)
    {
        // https://wiki.scn.sap.com/wiki/display/CRM/Norway
        // 12 digits
        // C1..C8 random 0 to 9
        // C9 check-digit MOD11
        // C10 C11 C12 chars == MVA
        try
        {
            if (taxId.Length == 9 && long.TryParse(taxId, out _))
                return ValidateModulus11(taxId);
            return false;
        }
        catch
        {
            return false;
        }
    }


    private static bool ValidateTaxIdSweden(string taxId)
    {
        // https://wiki.scn.sap.com/wiki/display/CRM/Sweden
        // 12 digits 0 to 9
        // C10 = (10 – (18 + 5 + 1 + 8 + 4)MOD10 10) MOD10
        // R = S1 + S3 + S5 + S7 + S9
        // Si = int(Ci/5) + (Ci*2)MOD10)
        // https://www.skatteverket.se/skatter/mervardesskattmoms/momsregistreringsnummer.4.18e1b10334ebe8bc80002649.html
        // EU MOMS => C11 C12 == 01 (De två sista siffrorna är alltid 01)
        // C11 C12 is not used inside Sweden
        // C1 is type of org and C2 to C9 is org number
        // C10 is check digit

        var toCheck = taxId;
        if (!long.TryParse(toCheck, out _))
            return false;

        switch (toCheck.Length)
        {
            // personal vat se
            case 6:
                return ValidateTaxIdSwedenExt(toCheck);

            case < 10:
                return false;

            // strip EU extension `01`
            case 12:
                taxId = taxId.Substring(0, 10);
                break;
        }

        var c10 = GetDigit10(toCheck);

        // compare calculated org number with incoming org number
        return $"{toCheck.Substring(0, 9)}{c10}" == taxId;
    }


    private static int GetDigit10(string taxIdToCheck)
    {
        // check digit calculation
        var r = new[] { 0, 2, 4, 6, 8 }
            .Sum(m => (int)char.GetNumericValue(taxIdToCheck[m]) / 5 +
                      (int)char.GetNumericValue(taxIdToCheck[m]) * 2 % 10);
        var c1 = new[] { 1, 3, 5, 7 }.Sum(m => (int)char.GetNumericValue(taxIdToCheck[m]));
        var c10 = (10 - (r + c1) % 10) % 10;
        return c10;
    }


    private static bool ValidateTaxIdSwedenExt(string ssn)
    {
        // Swedish personally held companies uses SSN number
        // a relaxed validation is required as only first 6 digits is supplied
        // birthday format e.g. 991231

        if (ssn.Length is not 6 or 10 || int.Parse(ssn) == 0)
            return false;

        var y = int.Parse(ssn.Substring(0, 2));
        var m = int.Parse(ssn.Substring(2, 2));
        var d = int.Parse(ssn.Substring(4, 2));
        // this calculation is only valid within 21st century
        var leap = y % 4 == 0; // 2000 was a leap year;    
        // day
        if (d is < 1 or > 31)
            return false;
        // month
        switch (m)
        {
            // feb
            case 2:
            {
                if (leap)
                    return d <= 29;
                return d <= 28;
            }
            // apr, jun, sep, nov
            case 4 or 6 or 9 or 11:
                return d <= 30;
            // jan, mar, may, july, aug, oct, dec
            case 1 or 3 or 5 or 7 or 8 or 10 or 12:
                return true;
            // does not exist
            default:
                return false;
        }
    }


    private static bool ValidateModulus11(string number)
    {
        try
        {
            if (long.Parse(number) == 0)
                return false;
            var sum = 0;
            for (int i = number.Length - 1, multiplier = 1; i >= 0; i--)
            {
                // Console.WriteLine($"char: {number[i]} multiplier: {multiplier}");
                sum += (int)char.GetNumericValue(number[i]) * multiplier;
                if (++multiplier > 7) multiplier = 2;
            }

            return sum % 11 == 0;
        }
        catch
        {
            return false;
        }
    }


    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="ssn"></param>
    ///// <returns></returns>
    //public static string FakeVatSsnSe(string ssn)
    //{
    //    var fake = ssn.PadRight(9, '8');
    //    var c10 = SeGenerateCheckDigit(fake);
    //    return $"{fake}{c10}";
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="org"></param>
    ///// <returns></returns>
    //public static bool OrgIsPrivate(string org)
    //{
    //    var orgType = new List<string>() { };

    //    return ValidateFormatSeExt(org.Substring(0, 5));
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="vatNumber"></param>
    ///// <returns></returns>
    //public static bool CheckLuhn(string vatNumber)
    //{
    //    // https://www.geeksforgeeks.org/luhn-algorithm/

    //    var nDigits = vatNumber.Length;
    //    var nSum = 0;
    //    var isSecond = false;
    //    for (var i = nDigits - 1; i >= 0; i--)
    //    {
    //        var d = (int)char.GetNumericValue(vatNumber[i]) - '0';
    //        if (isSecond)
    //            d *= 2;
    //        // We add two digits to handle
    //        // cases that make two digits
    //        // after doubling
    //        nSum += d / 10;
    //        nSum += d % 10;

    //        isSecond = !isSecond;
    //    }

    //    return nSum % 10 == 0;
    //}


    //private static bool ValidateMod10(string number)
    //{
    //    if (long.Parse(number) == 0)
    //        return false;

    //    var nDigits = number.Length;
    //    var nSum = 0;
    //    var isSecond = false;
    //    for (var i = nDigits - 1; i >= 0; i--)
    //    {
    //        var d = (int)char.GetNumericValue(number[i]) - '0';
    //        if (isSecond)
    //            d *= 2;
    //        nSum += d / 10;
    //        nSum += d % 10;
    //        isSecond = !isSecond;
    //    }
    //    return nSum % 10 == 0;
    //}

    //private static string GetMod10CheckDigit(string number)
    //{
    //    var sum = 0;
    //    var alt = true;
    //    var digits = number.ToCharArray();
    //    for (var i = digits.Length - 1; i >= 0; i--)
    //    {
    //        var curDigit = digits[i] - 48;
    //        if (alt)
    //        {
    //            curDigit *= 2;
    //            if (curDigit > 9)
    //                curDigit -= 9;
    //        }

    //        sum += curDigit;
    //        alt = !alt;
    //    }
    //    return sum % 10 == 0 ? "0" : (10 - sum % 10).ToString();
    //}

    //private string AddMod11CheckDigit(string number)
    //{
    //    return number + GetMod11CheckDigit(number);
    //}

    //private static string GetMod11CheckDigit(string number)
    //{
    //    var sum = 0;
    //    for (int i = number.Length - 1, multiplier = 2; i >= 0; i--)
    //    {
    //        sum += (int)char.GetNumericValue(number[i]) * multiplier;
    //        if (++multiplier > 7) multiplier = 2;
    //    }

    //    var modulo = sum % 11;
    //    return modulo is 0 or 1 ? "0" : (11 - modulo).ToString();
    //}

    //private static bool CheckLuhn(string vatNumber)
    //{
    //    // https://www.geeksforgeeks.org/luhn-algorithm/

    //    var nDigits = vatNumber.Length;
    //    var nSum = 0;
    //    var isSecond = false;
    //    for (var i = nDigits - 1; i >= 0; i--)
    //    {
    //        var d = (int)char.GetNumericValue(vatNumber[i]) - '0';
    //        if (isSecond)
    //            d *= 2;
    //        // We add two digits to handle
    //        // cases that make two digits
    //        // after doubling
    //        nSum += d / 10;
    //        nSum += d % 10;

    //        isSecond = !isSecond;
    //    }

    //    return nSum % 10 == 0;
    //}
}