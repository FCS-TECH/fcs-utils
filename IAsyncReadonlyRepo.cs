// // ***********************************************************************
// // Solution         : Inno.Api.v2
// // Assembly         : FCS.Lib.Utility
// // Filename         : IAsyncReadonlyRepo.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCS.Lib.Utility;

/// <summary>
///     Provides an asynchronous read-only repository interface for accessing entities of type
///     <typeparamref name="TEntity" />.
/// </summary>
/// <typeparam name="TEntity">The type of the entity managed by the repository. Must be a reference type.</typeparam>
public interface IAsyncReadonlyRepo<TEntity> where TEntity : class
{
    /// <summary>
    ///     Retrieves all entities of type <typeparamref name="TEntity" /> as an <see cref="IQueryable{T}" />.
    /// </summary>
    /// <returns>
    ///     An <see cref="IQueryable{T}" /> representing all entities of type <typeparamref name="TEntity" /> in the
    ///     repository.
    /// </returns>
    IQueryable<TEntity> All();


    /// <summary>
    ///     Asynchronously retrieves a list of entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">
    ///     An expression to filter the entities.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of entities
    ///     that satisfy the specified predicate.
    /// </returns>
    Task<IList<TEntity>> AllAsync(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    ///     Determines asynchronously whether any entities in the repository satisfy the specified condition.
    /// </summary>
    /// <param name="predicate">
    ///     An expression to test each entity for a condition.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains <c>true</c> if any entities satisfy the
    ///     condition specified by <paramref name="predicate" />; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    ///     Asynchronously finds a single entity of type <typeparamref name="TEntity" /> that matches the specified
    ///     <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">
    ///     An expression to test each entity for a condition.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the entity of type
    ///     <typeparamref name="TEntity" />
    ///     that matches the specified <paramref name="predicate" />, or <c>null</c> if no such entity is found.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the <paramref name="predicate" /> is <c>null</c>.
    /// </exception>
    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    ///     Asynchronously retrieves the first entity of type <typeparamref name="TEntity" />
    ///     that satisfies the specified <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">
    ///     An expression to test each entity for a condition.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     the first entity that matches the specified condition.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if no entity satisfies the condition specified by <paramref name="predicate" />.
    /// </exception>
    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    ///     Asynchronously retrieves the first entity that matches the specified predicate or a default value if no such entity
    ///     is found.
    /// </summary>
    /// <param name="predicate">
    ///     An expression to test each entity for a condition.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the first entity that matches the
    ///     predicate,
    ///     or the default value for the entity type if no such entity is found.
    /// </returns>
    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    bool Any(Expression<Func<TEntity, bool>> predicate);


    /// <summary>
    ///     Retrieves an entity of type <typeparamref name="TEntity" /> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>
    ///     The entity of type <typeparamref name="TEntity" /> that matches the specified identifier, or <c>null</c> if no
    ///     such entity exists.
    /// </returns>
    TEntity GetById(string id);
}