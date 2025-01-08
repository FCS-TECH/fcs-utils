// // ***********************************************************************
// // Solution         : Inno.Api.v2
// // Assembly         : FCS.Lib.Utility
// // Filename         : IRepositoryEx.cs
// // Created          : 2025-01-03 14:01
// // Last Modified By : dev
// // Last Modified On : 2025-01-04 12:01
// // ***********************************************************************
// // <copyright company="Frede Hundewadt">
// //     Copyright (C) 2010-2025 Frede Hundewadt
// //     This program is free software: you can redistribute it and/or modify
// //     it under the terms of the GNU Affero General Public License as
// //     published by the Free Software Foundation, either version 3 of the
// //     License, or (at your option) any later version.
// //
// //     This program is distributed in the hope that it will be useful,
// //     but WITHOUT ANY WARRANTY; without even the implied warranty of
// //     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //     GNU Affero General Public License for more details.
// //
// //     You should have received a copy of the GNU Affero General Public License
// //     along with this program.  If not, see [https://www.gnu.org/licenses]
// // </copyright>
// // <summary></summary>
// // ***********************************************************************

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCS.Lib.Utility;

/// <summary>
///     Defines a repository interface for managing entities of type <typeparamref name="TEntity" />.
///     Provides methods for querying, adding, updating, and deleting entities.
/// </summary>
/// <typeparam name="TEntity">
///     The type of the entity that the repository manages. Must be a reference type.
/// </typeparam>
public interface IRepositoryEx<TEntity> where TEntity : class
{
    /// <summary>
    ///     Asynchronously determines whether any entities in the repository satisfy the specified condition.
    /// </summary>
    /// <param name="predicate">
    ///     An expression that defines the condition to test against the entities.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains <c>true</c> if any entities satisfy the
    ///     condition; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    ///     Asynchronously finds an entity of type <typeparamref name="TEntity" /> that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">
    ///     An expression that defines the conditions of the entity to find.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the entity of type
    ///     <typeparamref name="TEntity" />
    ///     that matches the specified predicate, or <c>null</c> if no such entity is found.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the <paramref name="predicate" /> is <c>null</c>.
    /// </exception>
    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    ///     Asynchronously retrieves the first entity that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">
    ///     An expression to test each entity for a condition.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the first entity
    ///     that matches the specified predicate.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if no entity matches the predicate.
    /// </exception>
    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    ///     Asynchronously retrieves the first entity that matches the specified predicate or a default value if no such entity
    ///     is found.
    /// </summary>
    /// <param name="predicate">
    ///     An expression to test each entity for a condition.
    /// </param>
    /// <typeparam name="TEntity">
    ///     The type of the entity that the repository manages.
    /// </typeparam>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the first entity that matches the
    ///     predicate,
    ///     or the default value for <typeparamref name="TEntity" /> if no such entity is found.
    /// </returns>
    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    ///     Adds the specified entity to the repository.
    /// </summary>
    /// <param name="entity">
    ///     The entity to add to the repository. Must not be <c>null</c>.
    /// </param>
    /// <remarks>
    ///     The added entity will be tracked by the repository and persisted to the underlying data store
    ///     when changes are saved.
    /// </remarks>
    void Add(TEntity entity);


    /// <summary>
    ///     Attaches the specified entity to the repository context,
    ///     allowing it to be tracked by the underlying data store without marking it as modified.
    /// </summary>
    /// <param name="entity">
    ///     The entity to attach to the repository context. Must not be <c>null</c>.
    /// </param>
    /// <remarks>
    ///     This method is typically used to associate an existing entity with the repository
    ///     when it is not currently being tracked by the context.
    /// </remarks>
    void Attach(TEntity entity);


    /// <summary>
    ///     Deletes the specified entity from the repository.
    /// </summary>
    /// <param name="entity">
    ///     The entity to be deleted. Must not be <c>null</c>.
    /// </param>
    /// <remarks>
    ///     This method removes the entity from the repository. Ensure that the entity exists in the repository before calling
    ///     this method.
    /// </remarks>
    void Delete(TEntity entity);

    /// <summary>
    ///     Determines whether any entities in the repository satisfy the specified condition.
    /// </summary>
    /// <param name="predicate">
    ///     An expression that defines the condition to test against the entities.
    /// </param>
    /// <returns>
    ///     <c>true</c> if any entities satisfy the condition specified by <paramref name="predicate" />; otherwise,
    ///     <c>false</c>.
    /// </returns>
    bool Any(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    ///     Retrieves an entity of type <typeparamref name="TEntity" /> by its unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the entity to retrieve.
    /// </param>
    /// <returns>
    ///     The entity of type <typeparamref name="TEntity" /> if found; otherwise, <c>null</c>.
    /// </returns>
    TEntity GetById(string id);


    /// <summary>
    ///     Retrieves the first entity in the repository that satisfies the specified condition.
    /// </summary>
    /// <param name="predicate">
    ///     An expression that defines the condition to test against the entities.
    /// </param>
    /// <returns>
    ///     The first entity that satisfies the specified condition.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if no entity satisfies the condition.
    /// </exception>
    TEntity First(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    ///     Retrieves the first entity from the repository that satisfies the specified condition,
    ///     or a default value if no such entity is found.
    /// </summary>
    /// <param name="predicate">
    ///     An expression that defines the condition to test against the entities.
    /// </param>
    /// <returns>
    ///     The first entity that satisfies the specified condition, or <c>null</c> if no such entity is found.
    /// </returns>
    TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    ///     Retrieves a collection of entities from the repository that satisfy the specified condition.
    /// </summary>
    /// <param name="predicate">
    ///     An expression that defines the condition to filter the entities.
    /// </param>
    /// <returns>
    ///     An <see cref="IQueryable{TEntity}" /> representing the collection of entities that match the specified condition.
    /// </returns>
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IQueryable<TEntity> All();
}