// ***********************************************************************
// Assembly         : FCS.Lib
// Author           : FH
// Created          : 2020-07-01
//
// Last Modified By : FH
// Last Modified On : 2021-02-24
// ***********************************************************************
// <copyright file="QueryHelper.cs" company="Frede Hundewadt">
//     Copyright © FCS 2015-2022
// </copyright>
// <summary>
//        Implementation of https://stackoverflow.com/a/45761590
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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FCS.Lib
{
    /// <summary>
    /// Class QueryHelper.
    /// </summary>
    public static class QueryHelper
    {
        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <param name="q">The q.</param>
        /// <param name="name">The name.</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <returns>IQueryable&lt;TModel&gt;.</returns>
        public static IQueryable<TModel> OrderBy<TModel>(this IQueryable<TModel> q, string name, bool desc)
        {
            var entityType = typeof(TModel);

            var p = entityType.GetProperty(name);

            var m = typeof(QueryHelper).GetMethod("OrderByProperty")?.MakeGenericMethod(entityType, p.PropertyType);

            return (IQueryable<TModel>)m.Invoke(null, new object[] { q, p, desc });
        }

        /// <summary>
        /// Thens the by.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <param name="q">The q.</param>
        /// <param name="name">The name.</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <returns>IQueryable&lt;TModel&gt;.</returns>
        public static IQueryable<TModel> ThenBy<TModel>(this IQueryable<TModel> q, string name, bool desc)
        {
            var entityType = typeof(TModel);

            var p = entityType.GetProperty(name);

            var m = typeof(QueryHelper).GetMethod("OrderByProperty")?.MakeGenericMethod(entityType, p.PropertyType);

            return (IQueryable<TModel>)m.Invoke(null, new object[] { q, p, desc });
        }


        /// <summary>
        /// Orders the by property.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TRet">The type of the t ret.</typeparam>
        /// <param name="q">The q.</param>
        /// <param name="p">The p.</param>
        /// <param name="desc">if set to <c>true</c> [desc].</param>
        /// <returns>IQueryable&lt;TModel&gt;.</returns>
        public static IQueryable<TModel> OrderByProperty<TModel, TRet>(IQueryable<TModel> q, PropertyInfo p, bool desc)
        {
            var pe = Expression.Parameter(typeof(TModel));

            Expression se = Expression.Convert(Expression.Property(pe, p), typeof(object));

            var exp = Expression.Lambda<Func<TModel, TRet>>(se, pe);

            return desc ? q.OrderByDescending(exp) : q.OrderBy(exp);
        }
    }
}