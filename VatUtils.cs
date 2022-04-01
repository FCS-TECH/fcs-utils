using System.Linq;

namespace FCS.Lib.Utility
{
    public static class VatUtils
    {
        public static bool CheckVat(string countryCode, string vatNumber)
        {
            return countryCode.ToUpperInvariant() switch
            {
                "DK" => CheckVatNumberDenmark(vatNumber),
                "NO" => CheckVatNumberNorway(vatNumber),
                "SE" => CheckVatNumberSweden(vatNumber),
                _ => false
            };
        }

        public static bool CheckVatNumberDenmark(string vatNumber)
        {
            // https://wiki.scn.sap.com/wiki/display/CRM/Denmark
            // 8 digits 0 to 9
            // C1..C7
            // C8 Modulo 11 check digit
            // C1 > 0
            // R = (2*C1 + 7*C2 + 6*C3 + 5*C4 + 4*C5 + 3*C6 + 2*C7 + C8)
            return (int)char.GetNumericValue(vatNumber[0]) == 1 && vatNumber.Length == 8 && ValidateMod11(vatNumber);
        }

        public static bool CheckVatNumberNorway(string vatNumber)
        {
            // https://wiki.scn.sap.com/wiki/display/CRM/Norway
            // 12 digits
            // C1..C8 random 0 to 9
            // C9 Check MOD11
            // C10 C11 C12 chars == MVA
            return vatNumber.Length >= 9 && ValidateMod11(vatNumber.Substring(0, 8));
        }

        public static bool CheckVatNumberSweden(string vatNumber)
        {
            // https://wiki.scn.sap.com/wiki/display/CRM/Sweden
            // 12 digits 0 to 9
            // C1 = (10 - (R + C2 + C4 + C6 + C8) modulo 10) modulo 10
            // R = S1 + S3 + S5 + S7 + S9
            // Si = int(Ci/5) + (Ci*2) modulo 10)
            // C11 C12 >= 01 <= 94
            //
            // Validate full string using Luhn algoritm
            // 12 chars
            if (vatNumber.Length != 12 || vatNumber.Substring(10) != "01" || long.Parse(vatNumber) == 0)
                return false;

            var r = new[] { 0, 2, 4, 5, 8 }
                .Sum(m => (int)char.GetNumericValue(vatNumber[m]) / 5 +
                          (int)char.GetNumericValue(vatNumber[m]) * 2 % 10);
            var c1 = new[] { 1, 3, 5, 7 }.Sum(m => (int)char.GetNumericValue(vatNumber[m]));
            var c10 = (10 - (r + c1) % 10) % 10;
            return $"{vatNumber.Substring(0,9)}{c10}{vatNumber.Substring(9,1)}" == vatNumber;
        }

        public static bool ValidateMod11(string number)
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

        public static bool ValidateMod10(string number)
        {
            if (long.Parse(number) == 0)
                return false;

            var nDigits = number.Length;
            var nSum = 0;
            var isSecond = false;
            for (var i = nDigits - 1; i >= 0; i--)
            {
                var d = (int)char.GetNumericValue(number[i]) - '0';
                if (isSecond)
                    d *= 2;
                nSum += d / 10;
                nSum += d % 10;
                isSecond = !isSecond;
            }
            return nSum % 10 == 0;
        }

        public static string GetMod10CheckDigit(string number)
        {
            var sum = 0;
            var alt = true;
            var digits = number.ToCharArray();
            for (var i = digits.Length - 1; i >= 0; i--)
            {
                var curDigit = digits[i] - 48;
                if (alt)
                {
                    curDigit *= 2;
                    if (curDigit > 9)
                        curDigit -= 9;
                }

                sum += curDigit;
                alt = !alt;
            }
            return sum % 10 == 0 ? "0" : (10 - sum % 10).ToString();
        }

        public static string AddMod11CheckDigit(string number)
        {
            return number + GetMod11CheckDigit(number);
        }

        public static string GetMod11CheckDigit(string number)
        {
            var sum = 0;
            for (int i = number.Length - 1, multiplier = 2; i >= 0; i--)
            {
                sum += (int)char.GetNumericValue(number[i]) * multiplier;
                if (++multiplier > 7) multiplier = 2;
            }

            var modulo = sum % 11;
            return modulo is 0 or 1 ? "0" : (11 - modulo).ToString();
        }

        private static bool CheckLuhn(string vatNumber)
        {
            // https://www.geeksforgeeks.org/luhn-algorithm/

            var nDigits = vatNumber.Length;
            var nSum = 0;
            var isSecond = false;
            for (var i = nDigits - 1; i >= 0; i--)
            {
                var d = (int)char.GetNumericValue(vatNumber[i]) - '0';
                if (isSecond)
                    d *= 2;
                // We add two digits to handle
                // cases that make two digits
                // after doubling
                nSum += d / 10;
                nSum += d % 10;

                isSecond = !isSecond;
            }

            return nSum % 10 == 0;
        }

    }
}