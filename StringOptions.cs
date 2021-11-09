// ***********************************************************************
// Assembly         : FCS.Lib
// Author           : FH
// Created          : 2020-09-09
//
// Last Modified By : FH
// Last Modified On : 2021-02-24
// ***********************************************************************
// <copyright file="StringOptions.cs" company="Frede Hundewadt">
//     Copyright © FCS 2015-2022
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FCS.Lib
{
    /// <summary>
    ///     Class StringOptions.
    /// </summary>
    public class StringOptions
    {
        /// <summary>
        ///     Gets or sets the length of the required.
        /// </summary>
        /// <value>The length of the required.</value>
        public int RequiredLength { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [require non letter or digit].
        /// </summary>
        /// <value><c>true</c> if [require non letter or digit]; otherwise, <c>false</c>.</value>
        public bool RequireNonLetterOrDigit { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [require digit].
        /// </summary>
        /// <value><c>true</c> if [require digit]; otherwise, <c>false</c>.</value>
        public bool RequireDigit { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [require lowercase].
        /// </summary>
        /// <value><c>true</c> if [require lowercase]; otherwise, <c>false</c>.</value>
        public bool RequireLowercase { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [require uppercase].
        /// </summary>
        /// <value><c>true</c> if [require uppercase]; otherwise, <c>false</c>.</value>
        public bool RequireUppercase { get; set; }

        /// <summary>
        ///     Gets or sets the required unique chars.
        /// </summary>
        /// <value>The required unique chars.</value>
        public int RequiredUniqueChars { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [require non alphanumeric].
        /// </summary>
        /// <value><c>true</c> if [require non alphanumeric]; otherwise, <c>false</c>.</value>
        public bool RequireNonAlphanumeric { get; set; }
    }
}