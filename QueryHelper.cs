// ***********************************************************************
//  Solution         : Inno.Api.v2
//  Assembly         : FCS.Lib.Utility
//  Filename         : QueryHelper.cs
//  Created          : 2025-01-03 14:01
//  Last Modified By : dev
//  Last Modified On : 2025-01-08 13:01
//  ***********************************************************************
//  <copyright company="Frede Hundewadt">
//      Copyright (C) 2010-2025 Frede Hundewadt
//      This program is free software: you can redistribute it and/or modify
//      it under the terms of the GNU Affero General Public License as
//      published by the Free Software Foundation, either version 3 of the
//      License, or (at your option) any later version.
// 
//      This program is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Affero General Public License for more details.
// 
//      You should have received a copy of the GNU Affero General Public License
//      along with this program.  If not, see [https://www.gnu.org/licenses]
//  </copyright>
//  <summary></summary>
//  ***********************************************************************

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FCS.Lib.Utility;

/// <summary>
///     Provides utility methods for dynamically ordering sequences based on property names and sort directions.
/// </summary>
/// <remarks>
///     This static class contains extension methods for <see cref="IQueryable{T}" /> to enable dynamic sorting
///     using reflection. It supports both ascending and descending order and allows chaining of multiple sorting criteria.
/// </remarks>
public static class QueryHelper
{
    // https://stackoverflow.com/a/45761590


    /// <summary>
    ///     Orders the elements of a sequence based on the specified property name and sort direction.
    /// </summary>
    /// <typeparam name="TModel">The type of the elements in the sequence.</typeparam>
    /// <param name="q">The sequence to order.</param>
    /// <param name="name">The name of the property to sort by.</param>
    /// <param name="desc">
    ///     A boolean value indicating whether to sort in descending order.
    ///     If <c>true</c>, the sequence is sorted in descending order; otherwise, it is sorted in ascending order.
    /// </param>
    /// <returns>
    ///     An <see cref="IQueryable{T}" /> whose elements are sorted according to the specified property and direction.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="q" /> or <paramref name="name" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the specified property <paramref name="name" /> does not exist on the type <typeparamref name="TModel" />
    ///     .
    /// </exception>
    /// <remarks>
    ///     This method uses reflection to dynamically sort the sequence based on the specified property.
    /// </remarks>
    public static IQueryable<TModel> OrderBy<TModel>(this IQueryable<TModel> q, string name, bool desc)
    {
        var entityType = typeof(TModel);
        var p = entityType.GetProperty(name);
        var m = typeof(QueryHelper)
            .GetMethod("OrderByProperty")
            ?.MakeGenericMethod(entityType, p?.PropertyType);

        return (IQueryable<TModel>)m?.Invoke(null, [q, p, desc]);
    }


    /// <summary>
    ///     Performs a subsequent ordering of the elements in a sequence based on the specified property name and sort
    ///     direction.
    /// </summary>
    /// <typeparam name="TModel">The type of the elements in the sequence.</typeparam>
    /// <param name="q">The sequence to perform the subsequent ordering on.</param>
    /// <param name="name">The name of the property to sort by.</param>
    /// <param name="desc">
    ///     A boolean value indicating whether to sort in descending order.
    ///     If <c>true</c>, the sequence is sorted in descending order; otherwise, it is sorted in ascending order.
    /// </param>
    /// <returns>
    ///     An <see cref="IQueryable{T}" /> whose elements are sorted according to the specified property and direction.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="q" /> or <paramref name="name" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the specified property <paramref name="name" /> does not exist on the type <typeparamref name="TModel" />
    ///     .
    /// </exception>
    /// <remarks>
    ///     This method is used to apply additional sorting to a sequence that has already been ordered.
    ///     It uses reflection to dynamically sort the sequence based on the specified property.
    /// </remarks>
    public static IQueryable<TModel> ThenBy<TModel>(this IQueryable<TModel> q, string name, bool desc)
    {
        var entityType = typeof(TModel);
        var p = entityType.GetProperty(name);
        var m = typeof(QueryHelper)
            .GetMethod("OrderByProperty")
            ?.MakeGenericMethod(entityType, p?.PropertyType);

        return (IQueryable<TModel>)m?.Invoke(null, [q, p, desc]);
    }


    /// <summary>
    ///     Orders the elements of a sequence based on the specified property and sort direction.
    /// </summary>
    /// <typeparam name="TModel">The type of the elements in the sequence.</typeparam>
    /// <typeparam name="TRet">The type of the property used for sorting.</typeparam>
    /// <param name="q">The sequence to order.</param>
    /// <param name="p">The property information used for sorting.</param>
    /// <param name="desc">
    ///     A boolean value indicating whether to sort in descending order.
    ///     If <c>true</c>, the sequence is sorted in descending order; otherwise, it is sorted in ascending order.
    /// </param>
    /// <returns>
    ///     An <see cref="IQueryable{T}" /> whose elements are sorted according to the specified property and direction.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="q" /> or <paramref name="p" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method constructs a dynamic LINQ expression to sort the sequence based on the specified property.
    /// </remarks>
    public static IQueryable<TModel> OrderByProperty<TModel, TRet>(IQueryable<TModel> q, PropertyInfo p, bool desc)
    {
        var pe = Expression.Parameter(typeof(TModel));
        Expression se = Expression.Convert(Expression.Property(pe, p), typeof(object));
        var exp = Expression.Lambda<Func<TModel, TRet>>(se, pe);

        return desc ? q.OrderByDescending(exp) : q.OrderBy(exp);
    }
}