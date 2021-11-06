// ***********************************************************************
// Assembly         : FCS.Lib
// Author           : FH
// Created          : 27-08-2016
//
// Last Modified By : Frede H.
// Last Modified On : 2021-02-24
// ***********************************************************************
// <copyright file="ExtensionsEx.cs" company="FCS">
//     Copyright © FCS 2015-2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace FCS.Lib
{
    /// <summary>
    ///     Class ExtensionsEx.
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