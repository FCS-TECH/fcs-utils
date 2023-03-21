// ***********************************************************************
// Assembly         : FCS.Lib.Utility
// Author          : fhdk
// Created          : 2022 12 17 13:33
// 
// Last Modified By: fhdk
// Last Modified On : 2023 03 14 09:16
// ***********************************************************************
// <copyright file="IRepository.cs" company="FCS">
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

namespace FCS.Lib.Utility;

/// <summary>
/// Interface IRepository
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey">The type of the TKey.</typeparam>
public interface IRepository<T, in TKey> where T : class
{
    /// <summary>
    ///     Gets the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>T.</returns>
    T GetById(TKey id);

    /// <summary>
    ///     Creates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void Create(T entity);

    /// <summary>
    ///     Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void Update(T entity);

    /// <summary>
    ///     Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    void Delete(TKey id);
}