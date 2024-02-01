// ***********************************************************************
// Assembly         : FCS.Lib.Utility
// Author           : 
// Created          : 2023-10-02
// 
// Last Modified By : root
// Last Modified On : 2023-10-13 07:33
// ***********************************************************************
// <copyright file="StringOptions.cs" company="FCS">
//     Copyright (C) 2015 - 2023 FCS Frede's Computer Service.
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
///     Class StringOptions.
/// </summary>
public class StringOptions
{
    /// <summary>
    ///     Gets or sets the required length of a string
    /// </summary>
    /// <value>The length of the required.</value>
    public int RequiredLength { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether to [require non letter or digit].
    /// </summary>
    /// <value><c>true</c> if [require non letter or digit]; otherwise, <c>false</c>.</value>
    public bool RequireNonLetterOrDigit { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether to [require digit].
    /// </summary>
    /// <value><c>true</c> if [require digit]; otherwise, <c>false</c>.</value>
    public bool RequireDigit { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether to [require lowercase].
    /// </summary>
    /// <value><c>true</c> if [require lowercase]; otherwise, <c>false</c>.</value>
    public bool RequireLowercase { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether to [require uppercase].
    /// </summary>
    /// <value><c>true</c> if [require uppercase]; otherwise, <c>false</c>.</value>
    public bool RequireUppercase { get; set; }

    /// <summary>
    ///     Gets or sets the required unique chars.
    /// </summary>
    /// <value>The required unique chars.</value>
    public int RequiredUniqueChars { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether to [require non alphanumeric].
    /// </summary>
    /// <value><c>true</c> if [require non alphanumeric]; otherwise, <c>false</c>.</value>
    public bool RequireNonAlphanumeric { get; set; }
}