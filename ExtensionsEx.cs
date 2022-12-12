// ***********************************************************************
// Assembly         : FCS.Lib.Utility
// Author           : FH
// Created          : 27-08-2016
//
// Last Modified By : Frede H.
// Last Modified On : 02-24-2022
// ***********************************************************************
// <copyright file="ExtensionsEx.cs" company="FCS">
//    Copyright (C) 2022 FCS Frede's Computer Services.
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the Affero GNU General Public License as
//    published by the Free Software Foundation, either version 3 of the
//    License, or (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    Affero GNU General Public License for more details.
//
//    You should have received a copy of the Affero GNU General Public License
//    along with this program.  If not, see [https://www.gnu.org/licenses/agpl-3.0.en.html]
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace FCS.Lib.Utility
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