// // ***********************************************************************
// // Solution         : Inno.Api.v2
// // Assembly         : FCS.Lib.Utility
// // Filename         : Ean13Validator.cs
// // Created          : 2025-01-03 14:01
// // Last Modified By : dev
// // Last Modified On : 2025-01-04 11:01
// // ***********************************************************************
// // <copyright company="Frede Hundewadt">
// //     Copyright (C) 2010-2025 Frede Hundewadt
// //     This program is free software: you can redistribute it and/or modify
// //     it under the terms of the GNU Affero General Public License as
// //     published by the Free Software Foundation, either version 3 of the
// //     License, or (at your option) any later version.
// //
// //     This program is distributed in the hope that it will be useful,
// //     but WITHOUT ANY WARRANTY; without even the implied warranty of
// //     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //     GNU Affero General Public License for more details.
// //
// //     You should have received a copy of the GNU Affero General Public License
// //     along with this program.  If not, see [https://www.gnu.org/licenses]
// // </copyright>
// // <summary></summary>
// // ***********************************************************************

using System;

namespace FCS.Lib.Utility;

/// <summary>
/// </summary>
public class Ean13Validator
{
    private const int Size = 12;

    /// <summary>
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static bool Validate(string number)
    {
        if (number.Length != Size + 1)
            return false;
        return number == ParsedNumber(number.Substring(0, 12));
    }

    /// <summary>
    ///     Generates a complete EAN-13 barcode string, including the check digit,
    ///     based on the provided array of the first 12 digits. If the input array is null,
    ///     it generates a random EAN-13 barcode.
    /// </summary>
    /// <param name="firstDigits">
    ///     An array of integers representing the first 12 digits of the EAN-13 barcode.
    ///     If null, a random barcode will be generated.
    /// </param>
    /// <returns>
    ///     A string representing the complete EAN-13 barcode, including the check digit.
    /// </returns>
    private static string Ean13(int[] firstDigits)
    {
        var summedProduct = 0;
        var randomDigits = new Random();
        bool isNull;
        if (firstDigits == null)
        {
            firstDigits = new int[Size];
            isNull = true;
        }
        else
        {
            isNull = false;
        }

        for (var idx = 0; idx < Size; idx++)
        {
            var alt = idx % 2 == 0 ? 1 : 3;
            int digit;
            if (isNull)
            {
                digit = randomDigits.Next(10);
                firstDigits[idx] = digit;
            }
            else
            {
                digit = firstDigits[idx];
            }

            summedProduct += digit * alt;
        }

        var checkDigit = 10 - summedProduct % 10;
        if (checkDigit == 10)
            checkDigit = 0;
        return string.Join("", firstDigits) + checkDigit;
    }

    /// <summary>
    ///     Parses the provided 12-digit number string and generates a complete EAN-13 barcode string,
    ///     including the check digit.
    /// </summary>
    /// <param name="number">
    ///     A string representing the first 12 digits of the EAN-13 barcode.
    ///     The string must be exactly 12 digits long and contain only numeric characters.
    /// </param>
    /// <returns>
    ///     A string representing the complete EAN-13 barcode, including the check digit,
    ///     or <c>null</c> if the input string is invalid.
    /// </returns>
    private static string ParsedNumber(string number)
    {
        var firstDigits = new int[Size];
        if ((number.Length != Size) | !long.TryParse(number, out _))
            return null;
        for (var idx = 0; idx < Size; idx++)
        {
            var digit = int.Parse(number[idx].ToString());
            firstDigits[idx] = digit;
        }

        return Ean13(firstDigits);
    }
}