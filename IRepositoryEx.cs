// ***********************************************************************
// Assembly         : FCS.Lib
// Author           : FH
// Created          : 03-10-2015
//
// Last Modified By : FH
// Last Modified On : 2021-02-24
// ***********************************************************************
// <copyright file="IRepositoryEx.cs" company="FCS">
//     Copyright © FCS 2015-2022
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCS.Lib
{
    /// <summary>
    ///     Interface IRepositoryEx
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity</typeparam>
    public interface IRepositoryEx<TEntity> where TEntity : class
    {
        /// <summary>
        ///     Get all entities asynchronous
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>Task&lt;System.Boolean&gt;</returns>
        Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Get all entities synchronous
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>Task&lt;System.Boolean&gt;</returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Find matching entity asynchronous
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>Task&lt;TEntity&gt;</returns>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Find first matching entity asynchronous
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>Task&lt;TEntity&gt;</returns>
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Get first entity matching query or default entity asynchronous
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>Task&lt;TEntity&gt;</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Add an entity
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(TEntity entity);

        /// <summary>
        ///     Attach the entity
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Attach(TEntity entity);

        /// <summary>
        ///     Delete the entity
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);

        /// <summary>
        ///     Anies the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Any(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Get all entities
        /// </summary>
        /// <returns>IQueryable&lt;TEntity&gt;</returns>
        IQueryable<TEntity> All();

        /// <summary>
        ///     Find all matching entities matching query
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>IQueryable&lt;TEntity&gt;</returns>
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Find first entity matching query
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>TEntity</returns>
        TEntity First(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Find first matching entity or default entity
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>TEntity</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Get entity by id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TEntity</returns>
        TEntity GetById(string id);
    }
}