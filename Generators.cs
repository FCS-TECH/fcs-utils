// ***********************************************************************
// Assembly         : FCS.Lib.Utility
// Author          : fhdk
// Created          : 2023 02 02 06:59
// 
// Last Modified By: fhdk
// Last Modified On : 2023 03 14 09:16
// ***********************************************************************
// <copyright file="Generators.cs" company="FCS">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace FCS.Lib.Utility;

/// <summary>
/// Generators
/// </summary>
public static class Generators
{
    /// <summary>
    /// Generate 6 character shortUrl
    /// </summary>
    /// <returns><see cref="string"/> of 6 characters</returns>
    public static string ShortUrlGenerator()
    {
        return ShortUrlGenerator(6);
    }

    /// <summary>
    /// Generate shortUrl with length
    /// </summary>
    /// <param name="length">The length.</param>
    /// <returns><see cref="string"/></returns>
    /// <remarks>derived from https://sourceforge.net/projects/shorturl-dotnet/</remarks>
    public static string ShortUrlGenerator(int length)
    {
        const string charsLower = "abcdefghijkmnopqrstuvwxyz";
        const string charsUpper = "ABCDFEGHJKLMNPQRSTUVWXYZ-";
        const string charsNumeric = "1234567890";

        // Create a local array containing supported short-url characters
        // grouped by types.
        var charGroups = new[]
        {
            charsLower.ToCharArray(),
            charsUpper.ToCharArray(),
            charsNumeric.ToCharArray()
        };

        // Use this array to track the number of unused characters in each
        // character group.
        var charsLeftInGroup = new int[charGroups.Length];

        // Initially, all characters in each group are not used.
        for (var i = 0; i < charsLeftInGroup.Length; i++)
            charsLeftInGroup[i] = charGroups[i].Length;

        // Use this array to track (iterate through) unused character groups.
        var leftGroupsOrder = new int[charGroups.Length];

        // Initially, all character groups are not used.
        for (var i = 0; i < leftGroupsOrder.Length; i++)
            leftGroupsOrder[i] = i;

        // Using our private random number generator
        var random = RandomSeed();

        // This array will hold short-url characters.
        // Allocate appropriate memory for the short-url.
        var shortUrl = new char[random.Next(length, length)];

        // Index of the last non-processed group.
        var lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

        // Generate short-url characters one at a time.
        for (var i = 0; i < shortUrl.Length; i++)
        {
            // If only one character group remained unprocessed, process it;
            // otherwise, pick a random character group from the unprocessed
            // group list. To allow a special character to appear in the
            // first position, increment the second parameter of the Next
            // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
            int nextLeftGroupsOrderIdx;
            if (lastLeftGroupsOrderIdx == 0)
                nextLeftGroupsOrderIdx = 0;
            else
                nextLeftGroupsOrderIdx = random.Next(0,
                    lastLeftGroupsOrderIdx);

            // Get the actual index of the character group, from which we will
            // pick the next character.
            var nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

            // Get the index of the last unprocessed characters in this group.
            var lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

            // If only one unprocessed character is left, pick it; otherwise,
            // get a random character from the unused character list.
            var nextCharIdx = lastCharIdx == 0 ? 0 : random.Next(0, lastCharIdx + 1);

            // Add this character to the short-url.
            shortUrl[i] = charGroups[nextGroupIdx][nextCharIdx];

            // If we processed the last character in this group, start over.
            if (lastCharIdx == 0)
            {
                charsLeftInGroup[nextGroupIdx] =
                    charGroups[nextGroupIdx].Length;
            }
            // There are more unprocessed characters left.
            else
            {
                // Swap processed character with the last unprocessed character
                // so that we don't pick it until we process all characters in
                // this group.
                if (lastCharIdx != nextCharIdx)
                    (charGroups[nextGroupIdx][lastCharIdx], charGroups[nextGroupIdx][nextCharIdx]) = (
                        charGroups[nextGroupIdx][nextCharIdx], charGroups[nextGroupIdx][lastCharIdx]);

                // Decrement the number of unprocessed characters in
                // this group.
                charsLeftInGroup[nextGroupIdx]--;
            }

            // If we processed the last group, start all over.
            if (lastLeftGroupsOrderIdx == 0)
            {
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            }
            // There are more unprocessed groups left.
            else
            {
                // Swap processed group with the last unprocessed group
                // so that we don't pick it until we process all groups.
                if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    (leftGroupsOrder[lastLeftGroupsOrderIdx], leftGroupsOrder[nextLeftGroupsOrderIdx]) = (
                        leftGroupsOrder[nextLeftGroupsOrderIdx], leftGroupsOrder[lastLeftGroupsOrderIdx]);

                // Decrement the number of unprocessed groups.
                lastLeftGroupsOrderIdx--;
            }
        }

        // Convert password characters into a string and return the result.
        return new string(shortUrl);
    }

    /// <summary>
    /// Username generator
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns><see cref="string"/></returns>
    /// <seealso cref="StringOptions"/>
    public static string GenerateUsername(StringOptions options = null)
    {
        options ??= new StringOptions
        {
            RequiredLength = 16,
            RequireDigit = true,
            RequireLowercase = true,
            RequireUppercase = true,
            RequiredUniqueChars = 4,
            RequireNonLetterOrDigit = false,
            RequireNonAlphanumeric = false
        };
        return GenerateRandomString(options);
    }

    /// <summary>
    /// Password generator
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns><see cref="string"/></returns>
    /// <seealso cref="StringOptions"/>
    public static string GeneratePassword(StringOptions options = null)
    {
        options ??= new StringOptions
        {
            RequiredLength = 16,
            RequireDigit = true,
            RequireLowercase = true,
            RequireUppercase = true,
            RequiredUniqueChars = 8,
            RequireNonLetterOrDigit = false,
            RequireNonAlphanumeric = false
        };
        return GenerateRandomString(options);
    }

    /// <summary>
    /// Random string generator with length
    /// </summary>
    /// <param name="length">The length.</param>
    /// <returns><see cref="string"/></returns>
    public static string GenerateRandomText(int length)
    {
        const string consonants = "bcdfghjklmnprstvxzBDFGHJKLMNPRSTVXZ";
        const string vowels = "aeiouyAEIOUY";

        var rndString = "";
        var randomNum = RandomSeed();

        while (rndString.Length < length)
        {
            rndString += consonants[randomNum.Next(consonants.Length)];
            if (rndString.Length < length)
                rndString += vowels[randomNum.Next(vowels.Length)];
        }

        return rndString;
    }

    /// <summary>
    /// Random string generator - string options
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns><see cref="string"/></returns>
    public static string GenerateRandomString(StringOptions options = null)
    {
        options ??= new StringOptions
        {
            RequiredLength = 16,
            RequireDigit = true,
            RequireLowercase = true,
            RequireUppercase = true,
            RequiredUniqueChars = 4,
            RequireNonLetterOrDigit = true,
            RequireNonAlphanumeric = true
        };

        var randomChars = new[]
        {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ", // uppercase 
            "abcdefghijkmnopqrstuvwxyz", // lowercase
            "0123456789", // digits
            "!@$?_-" // non-alphanumeric
        };

        // Using our private random number generator
        var rand = RandomSeed();

        var chars = new List<char>();

        if (options.RequireUppercase)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[0][rand.Next(0, randomChars[0].Length)]);

        if (options.RequireLowercase)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[1][rand.Next(0, randomChars[1].Length)]);

        if (options.RequireDigit)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[2][rand.Next(0, randomChars[2].Length)]);

        if (options.RequireNonAlphanumeric)
            chars.Insert(rand.Next(0, chars.Count),
                randomChars[3][rand.Next(0, randomChars[3].Length)]);

        var rcs = randomChars[rand.Next(0, randomChars.Length)];
        for (var i = chars.Count;
             i < options.RequiredLength
             || chars.Distinct().Count() < options.RequiredUniqueChars;
             i++)
            chars.Insert(rand.Next(0, chars.Count),
                rcs[rand.Next(0, rcs.Length)]);

        return new string(chars.ToArray());
    }

    /// <summary>
    /// Randomize random using RNGCrypto
    /// </summary>
    /// <returns><see cref="Random"/></returns>
    /// <remarks>derived from https://sourceforge.net/projects/shorturl-dotnet/</remarks>
    /// <seealso cref="RNGCryptoServiceProvider"/>
    public static Random RandomSeed()
    {
        // As the default Random is based on the current time
        // so it produces the same "random" number within a second
        // Use a crypto service to create the seed value

        // 4-byte array to fill with random bytes
        var randomBytes = new byte[4];

        // Generate 4 random bytes.
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }

        // Convert 4 bytes into a 32-bit integer value.
        var seed = ((randomBytes[0] & 0x7f) << 24) |
                   (randomBytes[1] << 16) |
                   (randomBytes[2] << 8) |
                   randomBytes[3];

        // Return a truly randomized random generator
        return new Random(seed);
    }
}