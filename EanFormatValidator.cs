// ***********************************************************************
// Assembly         : FCS.Lib.Utility
// Author           : inno
// Created          : 2023 04 25 09:01
// 
// Last Modified By : inno
// Last Modified On : 2023 04 30 10:25
// ***********************************************************************
// <copyright file="EanFormatValidator.cs" company="FCS">
//     Copyright (C) 2023-2023 FCS Frede's Computer Services.
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

using System.Collections.Generic;

namespace FCS.Lib.Utility;

public class EanFormatValidator
{
    private const int Ean13Size = 12;

    public static bool ValidateIan13(string number)
    {
        if (number.Length != Ean13Size + 1) return false;

        return number == ParsedNumber(number.Substring(0, Ean13Size));
    }

    private static string CalculateIan13(IReadOnlyList<int> firstDigits)
    {
        var subResult = 0;

        for (var idx = 0; idx < Ean13Size; idx++)
        {
            var alt = idx % 2 == 0 ? 1 : 3;

            var digit = firstDigits[idx];

            subResult += digit * alt;
        }

        var checkDigit = 10 - subResult % 10 == 10 ? 0 : 10 - subResult % 10;

        return string.Join("", firstDigits) + checkDigit;
    }

    private static string ParsedNumber(string number)
    {
        var firstDigits = new int[Ean13Size];
        if ((number.Length != Ean13Size) | !long.TryParse(number, out _))
            return null;
        for (var idx = 0; idx < Ean13Size; idx++)
        {
            var digit = int.Parse(number[idx].ToString());
            firstDigits[idx] = digit;
        }

        return CalculateIan13(firstDigits);
    }
}