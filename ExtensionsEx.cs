// ***********************************************************************
// Assembly         : FCS.Lib
// Author           : FH
// Created          : 27-08-2016
//
// Last Modified By : Frede H.
// Last Modified On : 12-31-2021
// ***********************************************************************
// <copyright file="ExtensionsEx.cs" company="FCS">
//     Copyright © FCS 2015-2022
// <summary>
//        Part of FCS.Lib - a set of utilities for C# - pieced together from fragments
//        Copyright (C) 2021  FCS
//
//        This program is free software: you can redistribute it and/or modify
//        it under the terms of the GNU Affero General Public License as
//        published by the Free Software Foundation, either version 3 of the
//        License, or (at your option) any later version.
//
//        This program is distributed in the hope that it will be useful,
//        but WITHOUT ANY WARRANTY; without even the implied warranty of
//        MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//        GNU Affero General Public License for more details.
//
//        You should have received a copy of the GNU Affero General Public License
//        along with this program.  If not, see <https://www.gnu.org/licenses/>.
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace FCS.Lib
{
    /// <summary>
    /// Class ExtensionsEx.
    /// </summary>
    public static class ExtensionsEx
    {
        /// <summary>
        ///     ForEach loop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
    }
}