// ***********************************************************************
// Assembly         : FCS.Lib
// Author           : FH
// Created          : 2020-07-01
//
// Last Modified By : FH
// Last Modified On : 2021-02-24
// ***********************************************************************
// <copyright file="Generators.cs" company="Frede Hundewadt">
//     Copyright © FCS 2015-2022
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace FCS.Lib
{
    /// <summary>
    ///     Class Generators
    ///     <remarks>generates varioous kinds of random strings. </remarks>
    /// </summary>
    public static class Generators
    {
        /// <summary>
        ///     Shorts the URL generator.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string ShortUrlGenerator()
        {
            return ShortUrlGenerator(6);
        }

        /// <summary>
        ///     Randoms the string.
        /// </summary>
        /// <param name="length">The lengt  h.</param>
        /// <returns>System.String.</returns>
        /// <remarks>derived from https://sourceforge.net/projects/shorturl-dotnet/</remarks>
        public static string ShortUrlGenerator(int length)
        {
            const string charsLower = "cdfghjkmnpqrstvwxyz";
            const string charsUpper = "BCDFGHJKLMNPQRSTVWXYZ-_";
            const string charsNumeric = "23456789";

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

            // Using our private randomizer
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
                    {
                        var temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                            charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }

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
                    {
                        var temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                            leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }

                    // Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx--;
                }
            }

            // Convert password characters into a string and return the result.
            return new string(shortUrl);
        }

        /// <summary>
        ///     Generates the username.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>System.String.</returns>
        public static string GenerateUsername(StringOptions options = null)
        {
            options ??= new StringOptions
            {
                RequiredLength = 10,
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
        ///     Generates the password.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>System.String.</returns>
        public static string GeneratePassword(StringOptions options = null)
        {
            options ??= new StringOptions
            {
                RequiredLength = 10,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
                RequiredUniqueChars = 4,
                RequireNonLetterOrDigit = true,
                RequireNonAlphanumeric = true
            };
            return GenerateRandomString(options);
        }

        /// <summary>
        ///     Generates the random text.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string GenerateRandomText(int length)
        {
            const string consonants = "bdfghjklmnprstvyBDFGHJKLMNPRSTVY";
            const string vowels = "aeiouAEIOU";

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
        ///     Generates the random password.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>System.String.</returns>
        public static string GenerateRandomString(StringOptions options = null)
        {
            options ??= new StringOptions
            {
                RequiredLength = 10,
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

            // Using our private randomizer
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
        ///     Randoms the seed.
        /// </summary>
        /// <returns>Random.</returns>
        /// <remarks>derived from https://sourceforge.net/projects/shorturl-dotnet/</remarks>
        public static Random RandomSeed()
        {
            // As the default randomizer is based on the current time
            // so it produces the same "random" number within a second
            // Use a crypto randomizer to create the seed value

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
}