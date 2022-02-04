// ***********************************************************************
// Assembly         : FCS.Lib
// Author           : FH
// Created          : 01-01-2022
//
// Last Modified By : FH
// Last Modified On : 12-31-2021
// ***********************************************************************
// <copyright file="QueryHelper.cs" company="">
//     Copyright ©  2021
// </copyright>
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
        // https://stackoverflow.com/a/45761590

        public static IQueryable<TModel> OrderBy<TModel>
            (this IQueryable<TModel> q, string name, bool desc)
        {
            var entityType = typeof(TModel);
            var p = entityType.GetProperty(name);
            var m = typeof(QueryHelper)
                .GetMethod("OrderByProperty")
                ?.MakeGenericMethod(entityType, p.PropertyType);
            return(IQueryable<TModel>) m.Invoke(null, new object[] { 
                q, p , desc });
        }

        public static IQueryable<TModel> ThenBy<TModel>
            (this IQueryable<TModel> q, string name, bool desc)
        {
            var entityType = typeof(TModel);
            var p = entityType.GetProperty(name);
            var m = typeof(QueryHelper)
                .GetMethod("OrderByProperty")
                ?.MakeGenericMethod(entityType, p.PropertyType);
            return(IQueryable<TModel>) m.Invoke(null, new object[] { 
                q, p , desc });
        }


        public static IQueryable<TModel> OrderByProperty<TModel, TRet>
            (IQueryable<TModel> q, PropertyInfo p, bool desc)
        {
            var pe = Expression.Parameter(typeof(TModel));
            Expression se = Expression.Convert(Expression.Property(pe, p), typeof(object));
            var exp = Expression.Lambda<Func<TModel, TRet>>(se, pe);
            return desc ? q.OrderByDescending(exp) : q.OrderBy(exp);
        }

    }}