// ***********************************************************************
// Assembly         : FCS.Lib
// Author           : FH
// Created          : 05-13-2020
//
// Last Modified By : FH
// Last Modified On : 2021-02-24
// ***********************************************************************
// <copyright file="IRepository.cs" company="FCS">
//     Copyright © FCS 2015-2020
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FCS.Lib
{
    /// <summary>
    ///     Interface IRepository
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
}