// ***********************************************************************
// Assembly         : FCS.Lib
// Author           : FH
// Created          : 03-10-2015
//
// Last Modified By : FH
// Last Modified On : 2021-03-27
// ***********************************************************************
// <copyright file="IRepositoryAsync.cs" company="FCS">
//     Copyright © FCS 2015-2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCS.Lib
{
    /// <summary>
    ///     Interface IRepositoryAsync
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