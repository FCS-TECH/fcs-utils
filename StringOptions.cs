// // ***********************************************************************
// // Solution         : Inno.Api.v2
// // Assembly         : FCS.Lib.Utility
// // Filename         : StringOptions.cs
// // Created          : 2025-01-03 14:01
// // Last Modified By : dev
// // Last Modified On : 2025-01-04 12:01
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

namespace FCS.Lib.Utility;

/// <summary>
///     Represents a set of options that define the requirements for generating strings, such as passwords or usernames.
/// </summary>
/// <remarks>
///     This class provides properties to specify various constraints, including the required length,
///     the inclusion of specific character types (e.g., digits, lowercase, uppercase, non-alphanumeric characters),
///     and the minimum number of unique characters.
/// </remarks>
public class StringOptions
{
    /// <summary>
    ///     Gets or sets the required minimum length for the generated string.
    /// </summary>
    /// <value>
    ///     An integer representing the minimum number of characters the string must contain.
    /// </value>
    /// <remarks>
    ///     This property is used to enforce a specific length constraint on generated strings,
    ///     such as passwords or usernames. The default value may vary depending on the context
    ///     in which the <see cref="StringOptions" /> class is used.
    /// </remarks>
    public int RequiredLength { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the generated string must include at least one
    ///     character that is neither a letter nor a digit.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the string must include at least one non-letter or non-digit character; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    ///     This property is useful for enforcing the inclusion of special characters in generated strings,
    ///     such as passwords or usernames, to meet specific security or formatting requirements.
    /// </remarks>
    public bool RequireNonLetterOrDigit { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the generated string must include at least one digit.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the string must contain at least one digit; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    ///     This property is used to enforce the inclusion of numeric characters in generated strings,
    ///     such as passwords or usernames, to meet specific security or formatting requirements.
    /// </remarks>
    public bool RequireDigit { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the generated string must include at least one lowercase letter.
    /// </summary>
    /// <value>
    ///     <c>true</c> if at least one lowercase letter is required; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    ///     This property is used to enforce the inclusion of lowercase letters in generated strings, such as passwords or
    ///     usernames.
    /// </remarks>
    public bool RequireLowercase { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the generated string must include at least one uppercase letter.
    /// </summary>
    /// <value>
    ///     <c>true</c> if at least one uppercase letter is required; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    ///     This property is used to enforce the inclusion of uppercase letters in generated strings,
    ///     such as passwords or usernames, to meet specific security or formatting requirements.
    /// </remarks>
    public bool RequireUppercase { get; set; }

    /// <summary>
    ///     Gets or sets the minimum number of unique characters required in the generated string.
    /// </summary>
    /// <value>
    ///     An integer representing the minimum number of unique characters that must be present in the string.
    /// </value>
    /// <remarks>
    ///     This property ensures that the generated string contains at least the specified number of unique characters.
    ///     It is particularly useful for enforcing diversity in passwords or usernames.
    /// </remarks>
    public int RequiredUniqueChars { get; set; }

/// <summary>
/// Gets or sets a value indicating whether the generated string must include at least one non-alphanumeric character.
/// </summary>
/// <value>
/// <c>true</c> if the generated string must include at least one non-alphanumeric character; otherwise, <c>false</c>.
/// </value>
/// <remarks>
/// Non-alphanumeric characters are symbols that are neither letters nor digits, such as punctuation marks or special characters.
/// This property is typically used to enforce stronger security requirements in generated strings, such as passwords.
/// </remarks>
    public bool RequireNonAlphanumeric { get; set; }
}