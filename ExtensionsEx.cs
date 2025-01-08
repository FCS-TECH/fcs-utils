// // ***********************************************************************
// // Solution         : Inno.Api.v2
// // Assembly         : FCS.Lib.Utility
// // Filename         : ExtensionsEx.cs
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
using System.Collections.Generic;

namespace FCS.Lib.Utility;

/// <summary>
///     Provides a set of extension methods for enhancing the functionality of enumerable collections and other types.
/// </summary>
public static class ExtensionsEx
{
    /// <summary>
    ///     Executes the specified action on each element of the enumerable collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="items">The enumerable collection of items to iterate over.</param>
    /// <param name="action">The action to perform on each element of the collection.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="items" /> or <paramref name="action" /> is <c>null</c>.
    /// </exception>
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach (var item in items)
            action(item);
    }
}