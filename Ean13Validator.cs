// ***********************************************************************
// Assembly         : FCS.Lib.Utility
// Filename         : Ean13Validator.cs
// Author           : Frede Hundewadt
// Created          : 2024 03 29 12:36
// 
// Last Modified By : root
// Last Modified On : 2024 04 11 13:03
// ***********************************************************************
// <copyright company="FCS">
//     Copyright (C) 2024-2024 FCS Frede's Computer Service.
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

using System;


namespace FCS.Lib.Utility;

public class Ean13Validator
{
    private const int Size = 12;


    public static bool Validate(string number)
    {
        if (number.Length != Size + 1)
            return false;
        return number == ParsedNumber(number.Substring(0, 12));
    }


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