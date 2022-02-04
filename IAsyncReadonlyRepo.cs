// ***********************************************************************
// Assembly         : FCS.Lib
// Author           : FH
// Created          : 03-10-2015
//
// Last Modified By : FH
// Last Modified On : 12-31-2021
// ***********************************************************************
// <copyright file="IAsyncReadonlyRepo.cs" company="FCS">
//     Copyright © FCS 2015-2022
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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCS.Lib
{
    /// <summary>
    /// Interface IRepositoryAsync
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public interface IAsyncReadonlyRepo<TEntity> where TEntity : class
    {
        /// <summary>
        ///     Alls this instance.
        /// </summary>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        IQueryable<TEntity> All();

        /// <summary>
        ///     Alls the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;IList&lt;TEntity&gt;&gt;.</returns>
        Task<IList<TEntity>> AllAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Anies the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Finds the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Firsts the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Firsts the or default asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Anies the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Any(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TEntity.</returns>
        TEntity GetById(string id);
    }
}