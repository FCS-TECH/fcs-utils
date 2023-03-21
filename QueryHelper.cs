// ***********************************************************************
// Assembly         : FCS.Lib.Utility
// Author          : fhdk
// Created          : 2022 12 17 13:33
// 
// Last Modified By: fhdk
// Last Modified On : 2023 03 14 09:16
// ***********************************************************************
// <copyright file="QueryHelper.cs" company="FCS">
//     Copyright (C) 2022-2023 FCS Frede's Computer Services.
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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FCS.Lib.Utility;

/// <summary>
/// Class QueryHelper.
/// </summary>
public static class QueryHelper
{
    // https://stackoverflow.com/a/45761590

    /// <summary>
    /// OrderBy
    /// </summary>
    /// <param name="q"></param>
    /// <param name="name"></param>
    /// <param name="desc"></param>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    public static IQueryable<TModel> OrderBy<TModel>(this IQueryable<TModel> q, string name, bool desc)
    {
        var entityType = typeof(TModel);
        var p = entityType.GetProperty(name);
        var m = typeof(QueryHelper)
            .GetMethod("OrderByProperty")
            ?.MakeGenericMethod(entityType, p.PropertyType);

        return (IQueryable<TModel>)m.Invoke(null, new object[] { q, p, desc });
    }

    /// <summary>
    /// ThenBy
    /// </summary>
    /// <param name="q"></param>
    /// <param name="name"></param>
    /// <param name="desc"></param>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    public static IQueryable<TModel> ThenBy<TModel>(this IQueryable<TModel> q, string name, bool desc)
    {
        var entityType = typeof(TModel);
        var p = entityType.GetProperty(name);
        var m = typeof(QueryHelper)
            .GetMethod("OrderByProperty")
            ?.MakeGenericMethod(entityType, p.PropertyType);

        return (IQueryable<TModel>)m.Invoke(null, new object[] { q, p, desc });
    }


    /// <summary>
    /// OrderByProperty
    /// </summary>
    /// <param name="q"></param>
    /// <param name="p"></param>
    /// <param name="desc"></param>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TRet"></typeparam>
    /// <returns></returns>
    public static IQueryable<TModel> OrderByProperty<TModel, TRet>(IQueryable<TModel> q, PropertyInfo p, bool desc)
    {
        var pe = Expression.Parameter(typeof(TModel));
        Expression se = Expression.Convert(Expression.Property(pe, p), typeof(object));
        var exp = Expression.Lambda<Func<TModel, TRet>>(se, pe);

        return desc ? q.OrderByDescending(exp) : q.OrderBy(exp);
    }
}