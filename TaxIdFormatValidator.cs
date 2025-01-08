// ***********************************************************************
//  Solution         : Inno.Api.v2
//  Assembly         : FCS.Lib.Utility
//  Filename         : TaxIdFormatValidator.cs
//  Created          : 2025-01-03 14:01
//  Last Modified By : dev
//  Last Modified On : 2025-01-08 13:01
//  ***********************************************************************
//  <copyright company="Frede Hundewadt">
//      Copyright (C) 2010-2025 Frede Hundewadt
//      This program is free software: you can redistribute it and/or modify
//      it under the terms of the GNU Affero General Public License as
//      published by the Free Software Foundation, either version 3 of the
//      License, or (at your option) any later version.
// 
//      This program is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Affero General Public License for more details.
// 
//      You should have received a copy of the GNU Affero General Public License
//      along with this program.  If not, see [https://www.gnu.org/licenses]
//  </copyright>
//  <summary></summary>
//  ***********************************************************************

using System.Linq;
using System.Text.RegularExpressions;

namespace FCS.Lib.Utility;

/// <summary>
///     Provides utility methods for validating and sanitizing tax identification numbers (VAT numbers)
///     according to country-specific formats and rules.
/// </summary>
public static class TaxIdFormatValidator
{
    // https://ec.europa.eu/taxation_customs/vies/faqvies.do#item_11
    // https://ec.europa.eu/taxation_customs/vies/
    //https://www.bolagsverket.se/apierochoppnadata.2531.html

    private static readonly Regex DigitsOnly = new(@"[^\d]");


    /// <summary>
    ///     Validates a tax identification number (VAT number) for a specified country.
    /// </summary>
    /// <param name="countryCode">
    ///     The two-letter ISO 3166-1 alpha-2 country code representing the country for which the tax ID is being validated.
    /// </param>
    /// <param name="taxId">
    ///     The tax identification number (VAT number) to validate. This value may be sanitized internally before validation.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the tax ID is valid according to the rules of the specified country; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     This method supports validation for Denmark (DK), Norway (NO), and Sweden (SE). For unsupported countries, the
    ///     method returns <c>false</c>.
    /// </remarks>
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
    ///     Removes all non-numeric characters from the provided tax ID string.
    /// </summary>
    /// <param name="taxId">The tax ID string to sanitize.</param>
    /// <returns>
    ///     A sanitized tax ID string containing only numeric characters, or an empty string if the input is null or
    ///     whitespace.
    /// </returns>
    public static string SanitizeTaxId(string taxId)
    {
        return string.IsNullOrWhiteSpace(taxId) ? "" : DigitsOnly.Replace(taxId, "");
    }

    /// <summary>
    ///     Validates a Danish tax identification number (VAT number) based on the country's specific rules.
    /// </summary>
    /// <param name="taxId">
    ///     The tax identification number (VAT number) to validate. It must consist of exactly 8 numeric digits.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the tax ID is valid according to Denmark's rules, including a valid Modulus 11 check; otherwise,
    ///     <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     The validation ensures that the tax ID is 8 digits long, starts with a non-zero digit, and passes the Modulus 11
    ///     checksum.
    /// </remarks>
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

    /// <summary>
    ///     Validates a Norwegian tax identification number (VAT number) based on the country's specific rules.
    /// </summary>
    /// <param name="taxId">
    ///     The tax identification number (VAT number) to validate. It must consist of exactly 9 numeric digits.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the tax ID is valid according to Norway's rules, including a valid Modulus 11 check; otherwise,
    ///     <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     The validation ensures that the tax ID is 9 digits long and passes the Modulus 11 checksum.
    ///     Additionally, the tax ID may include the "MVA" suffix, which is ignored during validation.
    /// </remarks>
    private static bool ValidateTaxIdNorway(string taxId)
    {
        // https://wiki.scn.sap.com/wiki/display/CRM/Norway
        // 12 digits
        // C1-C8 random 0 to 9
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

    /// <summary>
    ///     Validates a Swedish tax identification number (VAT number) according to the specific rules and format.
    /// </summary>
    /// <param name="taxId">
    ///     The tax identification number (VAT number) to validate. This value must consist of digits only and may include an
    ///     EU extension.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the tax ID is valid according to Swedish tax ID rules; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     A valid Swedish tax ID must adhere to the following rules:
    ///     - It must consist of 12 digits (0-9), where the last two digits (C11 and C12) are "01" for EU VAT numbers.
    ///     - For non-EU usage, only the first 10 digits are considered.
    ///     - The first digit (C1) represents the type of organization, and digits C2 to C9 represent the organization number.
    ///     - The 10th digit (C10) is a check digit calculated using a specific algorithm.
    ///     - For personally held companies, a relaxed validation is applied, requiring only the first 6 digits in the format
    ///     YYMMDD.
    /// </remarks>
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
        // C10 is verification digit

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

    /// <summary>
    ///     Calculates the check digit (C10) for a given Swedish tax identification number (VAT number).
    /// </summary>
    /// <param name="taxIdToCheck">
    ///     The tax identification number (VAT number) for which the check digit is to be calculated.
    /// </param>
    /// <returns>
    ///     The calculated check digit (C10) as an integer.
    /// </returns>
    /// <remarks>
    ///     The check digit is calculated based on a specific algorithm that involves summing weighted values of certain digits
    ///     in the tax ID. This method is used internally for validating Swedish tax identification numbers.
    /// </remarks>
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

    /// <summary>
    ///     Performs a relaxed validation of a Swedish tax identification number (SSN format)
    ///     used by personally held companies.
    /// </summary>
    /// <param name="ssn">
    ///     The SSN to validate, provided in a 6-digit (YYMMDD) or 10-digit format.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the SSN is valid based on the date format and logical rules; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     This method validates the SSN by checking the format and ensuring the date components
    ///     (year, month, and day) are logically correct. It assumes the SSN belongs to the 21st century.
    /// </remarks>
    private static bool ValidateTaxIdSwedenExt(string ssn)
    {
        // Swedish personally held companies uses SSN number
        // a relaxed validation is required as only first 6 digits is supplied
        // birthday format e.g. 991231

        if (ssn.Length != 6 || ssn.Length != 10 || int.Parse(ssn) == 0)
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

    /// <summary>
    ///     Validates a numeric string using the Modulus 11 algorithm.
    /// </summary>
    /// <param name="number">
    ///     The numeric string to validate. Each character in the string must be a digit.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the numeric string is valid according to the Modulus 11 algorithm; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     The Modulus 11 algorithm calculates a checksum by multiplying each digit by a weight that cycles from 2 to 7.
    ///     The sum of these products is then divided by 11, and the remainder determines validity.
    /// </remarks>
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
}