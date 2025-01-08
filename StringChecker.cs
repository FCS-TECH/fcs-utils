// // ***********************************************************************
// // Solution         : Inno.Api.v2
// // Assembly         : FCS.Lib.Utility
// // Filename         : StringChecker.cs
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
///     Provides utility methods for validating and checking string values.
/// </summary>
public static class StringChecker
{
    /// <summary>
    ///     Determines whether the specified search phrase is valid.
    /// </summary>
    /// <param name="searchPhrase">The search phrase to validate.</param>
    /// <param name="length">The maximum allowed length for the search phrase. Defaults to 50.</param>
    /// <returns>
    ///     <c>true</c> if the search phrase is not null, not empty, not whitespace, and does not exceed the specified length;
    ///     otherwise, <c>false</c>.
    /// </returns>
    public static bool SearchPhraseValid(string searchPhrase, int length = 50)
    {
        return !string.IsNullOrWhiteSpace(searchPhrase) && searchPhrase.Length <= length;
    }
}