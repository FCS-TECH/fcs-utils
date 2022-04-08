using System;
using System.Linq;

namespace FCS.Lib.Utility
{
    public static class VatFormatValidator
    {
        // https://ec.europa.eu/taxation_customs/vies/faqvies.do#item_11
        // https://ec.europa.eu/taxation_customs/vies/

        public static bool CheckVat(string countryCode, string vatNumber)
        {
            return countryCode.ToUpperInvariant() switch
            {
                "DK" => ValidateFormatDk(vatNumber),
                "NO" => ValidateFormatNo(vatNumber),
                "SE" => ValidateFormatSe(vatNumber),
                _ => false
            };
        }

        private static bool ValidateFormatDk(string vatNumber)
        {
            // https://wiki.scn.sap.com/wiki/display/CRM/Denmark
            // 8 digits 0 to 9
            // C1..C7
            // C8 check-digit MOD11
            // C1 > 0
            // R = (2*C1 + 7*C2 + 6*C3 + 5*C4 + 4*C5 + 3*C6 + 2*C7 + C8)
            if(vatNumber.Length == 8 && long.Parse(vatNumber) != 0)
                return  ValidateMod11(vatNumber);
            return false;
        }

        private static bool ValidateFormatNo(string vatNumber)
        {
            // https://wiki.scn.sap.com/wiki/display/CRM/Norway
            // 12 digits
            // C1..C8 random 0 to 9
            // C9 check-digit MOD11
            // C10 C11 C12 chars == MVA
            if (vatNumber.Length == 9 && long.Parse(vatNumber) != 0)
                return  ValidateMod11(vatNumber);
            return false;
        }

        private static bool ValidateFormatSe(string vatNumber)
        {
            // https://wiki.scn.sap.com/wiki/display/CRM/Sweden
            // 12 digits 0 to 9
            // C10 = (10 – (18 + 5 + 1 + 8 + 4)MOD10 10) MOD10
            // R = S1 + S3 + S5 + S7 + S9
            // Si = int(Ci/5) + (Ci*2)MOD10)
            // https://www.skatteverket.se/skatter/mervardesskattmoms/momsregistreringsnummer.4.18e1b10334ebe8bc80002649.html
            // C11 C12 == 01 (De två sista siffrorna är alltid 01)
            
            if (vatNumber.Length != 12 || vatNumber.Substring(10) != "01" || long.Parse(vatNumber) == 0)
                return false;

            var r = new[] { 0, 2, 4, 6, 8 }
                .Sum(m => (int)char.GetNumericValue(vatNumber[m]) / 5 +
                          (int)char.GetNumericValue(vatNumber[m]) * 2 % 10);
            var c1 = new[] { 1, 3, 5, 7 }.Sum(m => (int)char.GetNumericValue(vatNumber[m]));
            var c10 = (10 - (r + c1) % 10) % 10;
            return $"{vatNumber.Substring(0, 9)}{c10}01"  == vatNumber;
        }

        private static bool ValidateMod11(string number)
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

        private static string SanitizeVatNumber(string vatNumber)
        {
            vatNumber = vatNumber.ToUpperInvariant();
            return vatNumber
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("DK", "")
                .Replace("NO", "")
                .Replace("SE","")
                .Replace("MVA", "");
        }

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
}