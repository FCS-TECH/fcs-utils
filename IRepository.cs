// // ***********************************************************************
// // Solution         : Inno.Api.v2
// // Assembly         : FCS.Lib.Utility
// // Filename         : IRepository.cs
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

namespace FCS.Lib.Utility;

/// <summary>
///     Represents a generic repository interface for managing entities of type <typeparamref name="T" />
///     with keys of type <typeparamref name="TKey" />.
/// </summary>
/// <typeparam name="T">The type of the entity managed by the repository. Must be a reference type.</typeparam>
/// <typeparam name="TKey">The type of the key used to identify entities.</typeparam>
public interface IRepository<T, in TKey> where T : class
{
    /// <summary>
    ///     Retrieves an entity of type <typeparamref name="T" /> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>
    ///     The entity of type <typeparamref name="T" /> associated with the specified identifier,
    ///     or <c>null</c> if no entity is found.
    /// </returns>
    T GetById(TKey id);


    /// <summary>
    ///     Adds a new entity of type <typeparamref name="T" /> to the repository.
    /// </summary>
    /// <param name="entity">The entity to add to the repository. Must not be <c>null</c>.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="entity" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method persists the provided entity into the repository.
    ///     Ensure that the entity satisfies any required constraints or validations before calling this method.
    /// </remarks>
    void Create(T entity);


    /// <summary>
    ///     Updates an existing entity of type <typeparamref name="T" /> in the repository.
    /// </summary>
    /// <param name="entity">The entity to update. Must not be <c>null</c>.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="entity" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method modifies the existing entity in the repository with the provided entity's data.
    ///     Ensure that the entity exists in the repository before calling this method.
    /// </remarks>
    void Update(T entity);


    /// <summary>
    ///     Deletes an entity of type <typeparamref name="T" /> from the repository using its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <remarks>
    ///     This method removes the entity associated with the specified identifier from the repository.
    ///     Ensure that the identifier corresponds to an existing entity before calling this method.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="id" /> is <c>null</c>.</exception>
    void Delete(TKey id);
}