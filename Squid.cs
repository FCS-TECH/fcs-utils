// ***********************************************************************
// Assembly         : Inno.Lib
// Author           : FH
// Created          : 2020-07-01
//
// Last Modified By : FH
// Last Modified On : 02-24-2022
// ***********************************************************************
// <copyright file="Squid.cs" company="FCS">
//    Copyright (C) 2022 FCS Frede's Computer Services.
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Affero General Public License as
//    published by the Free Software Foundation, either version 3 of the
//    License, or (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Affero General Public License for more details.
//
//    You should have received a copy of the GNU Affero General Public License
//    along with this program.  If not, see [https://www.gnu.org/licenses]
// </copyright>
// <summary>Derived from https:github.com/csharpvitamins/CSharpVitamins.ShortGuid</summary>
// ***********************************************************************

using System;
using System.Diagnostics;

namespace FCS.Lib.Utility
{
    /// <summary>
    /// A wrapper for handling URL-safe Base64 encoded globally unique identifiers (GUID).
    /// </summary>
    /// <remarks>Special characters are replaced (/, +) or removed (==).
    /// Derived from https:github.com/csharpvitamins/CSharpVitamins.ShortGuid</remarks>
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public readonly struct Squid : IEquatable<Squid>
    {
        /// <summary>
        ///     A read-only object of the Squid struct.
        ///     Value is guaranteed to be all zeroes.
        ///     Equivalent to <see cref="Guid.Empty" />.
        /// </summary>
        public static readonly Squid Empty = new(Guid.Empty);

        /// <summary>
        ///     Creates a new Squid from a Squid encoded string.
        /// </summary>
        /// <param name="value">A valid Squid encodd string.</param>
        public Squid(string value)
        {
            Value = value;
            Guid = DecodeSquid(value);
        }

        /// <summary>
        ///     Creates a new Squid with the given <see cref="System.Guid" />.
        /// </summary>
        /// <param name="obj">A valid System.Guid object.</param>
        public Squid(Guid obj)
        {
            Value = EncodeGuid(obj);
            Guid = obj;
        }

        /// <summary>
        ///     Gets the underlying <see cref="System.Guid" /> for the encoded Squid.
        /// </summary>
        /// <value>The unique identifier.</value>
#pragma warning disable CA1720 // Identifier contains type name
        public Guid Guid { get; }
#pragma warning restore CA1720 // Identifier contains type name

        /// <summary>
        ///     The encoded string value of the <see cref="Guid" />
        ///     as an URL-safe Base64 string.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; }

        /// <summary>
        ///     Returns the encoded URL-safe Base64 string.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        ///     Returns a value indicating whether this object and a specified object represent the same type and value.
        ///     Compares for equality against other string, Guid and Squid types.
        /// </summary>
        /// <param name="obj">A Systerm.String, System.Guid or Squid object</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is Squid other && Equals(other);
        }

        /// <summary>
        ///     Equality comparison
        /// </summary>
        /// <param name="obj">A valid Squid object</param>
        /// <returns>A boolean indicating equality.</returns>
        public bool Equals(Squid obj)
        {
            return Guid.Equals(obj.Guid) && Value == obj.Value;
        }

        /// <summary>
        ///     Returns the hash code for the underlying <see cref="System.Guid" />.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Guid.GetHashCode() * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }

        /// <summary>
        ///     Initialises a new object of the Squid using <see cref="Guid.NewGuid()" />.
        /// </summary>
        /// <returns>New Squid object</returns>
        public static Squid NewGuid()
        {
            return new(Guid.NewGuid());
        }

        /// <summary>
        ///     Encode string as a new Squid encoded string.
        ///     The encoding is similar to Base64 with
        ///     non-URL safe characters replaced, and padding removed.
        /// </summary>
        /// <param name="value">A valid <see cref="System.Guid" />.Tostring().</param>
        /// <returns>A 22 character URL-safe Base64 string.</returns>
        public static string EncodeString(string value)
        {
            var guid = new Guid(value);
            return EncodeGuid(guid);
        }

        /// <summary>
        ///     Encode a <see cref="System.Guid" /> object to Squid.
        ///     The encoding is similar to Base64 with
        ///     non-URL safe characters replaced, and padding removed.
        /// </summary>
        /// <param name="obj">A valid <see cref="System.Guid" /> object.</param>
        /// <returns>A 22 character URL-safe Base64 string.</returns>
        public static string EncodeGuid(Guid obj)
        {
            var encoded = Convert.ToBase64String(obj.ToByteArray());
            encoded = encoded
                .Replace("/", "_")
                .Replace("+", "-");
            return encoded.Substring(0, 22);
        }

        /// <summary>
        ///     Decode Squid string to a <see cref="System.Guid" />.
        ///     See also <seealso cref="TryDecode(string, out System.Guid)" /> or
        ///     <seealso cref="TryParse(string, out System.Guid)" />.
        /// </summary>
        /// <param name="value">A valid Squid encoded string.</param>
        /// <returns>A new <see cref="System.Guid" /> object from the parsed string.</returns>
        public static Guid DecodeSquid(string value)
        {
            if (value == null) return Empty;
            value = value
                .Replace("_", "/")
                .Replace("-", "+");

            var blob = Convert.FromBase64String(value + "==");
            return new Guid(blob);
        }

        /// <summary>
        ///     Squid to Guid.
        /// </summary>
        /// <param name="obj">A valid Squid object.</param>
        /// <returns>System.Guid object.</returns>
        public static Guid FromSquid(Squid obj)
        {
            return obj.Guid;
        }

        /// <summary>
        ///     String to Squid.
        /// </summary>
        /// <param name="value">String value to convert</param>
        /// <returns>A Squid object.</returns>
        public static Squid FromString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Empty;
            return TryParse(value, out Squid obj) ? obj : Empty;
        }

        /// <summary>
        ///     Decodes the given value to a <see cref="System.Guid" />.
        /// </summary>
        /// <param name="value">The Squid encoded string to decode.</param>
        /// <param name="obj">A new <see cref="System.Guid" /> object from the parsed string.</param>
        /// <returns>A boolean indicating if the decode was successful.</returns>
        public static bool TryDecode(string value, out Guid obj)
        {
            try
            {
                // Decode as Squid
                obj = DecodeSquid(value);
                return true;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // Return empty Guid
                obj = Guid.Empty;
                return false;
            }
        }

        /// <summary>
        ///     Tries to parse the given string value and
        ///     outputs the <see cref="Squid" /> object.
        /// </summary>
        /// <param name="value">The Squid encoded string or string representation of a Guid.</param>
        /// <param name="obj">A new <see cref="Squid" /> object from the parsed string.</param>
        /// <returns>A boolean indicating if the parse was successful.</returns>
        public static bool TryParse(string value, out Squid obj)
        {
            // Parse as Squid string.
            if (TryDecode(value, out var oGuid))
            {
                obj = oGuid;
                return true;
            }

            // Parse as Guid string.
            if (Guid.TryParse(value, out oGuid))
            {
                obj = oGuid;
                return true;
            }

            obj = Empty;
            return false;
        }

        /// <summary>
        ///     Tries to parse the string value and
        ///     outputs the underlying <see cref="System.Guid" /> object.
        /// </summary>
        /// <param name="value">The Squid encoded string or string representation of a Guid.</param>
        /// <param name="obj">A new <see cref="System.Guid" /> object from the parsed string.</param>
        /// <returns>A boolean indicating if the parse was successful.</returns>
        public static bool TryParse(string value, out Guid obj)
        {
            // Try a Squid string.
            if (TryDecode(value, out obj))
                return true;

            // Try a Guid string.
            if (Guid.TryParse(value, out obj))
                return true;

            obj = Guid.Empty;
            return false;
        }

        #region Operators

        /// <summary>
        ///     Determines if both Squid objects have the same
        ///     underlying <see cref="System.Guid" /> value.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Squid x, Squid y)
        {
            return x.Guid == y.Guid;
        }

        /// <summary>
        ///     Determines if both objects have the same
        ///     underlying <see cref="System.Guid" /> value.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Squid x, Guid y)
        {
            return x.Guid == y;
        }

        /// <summary>
        ///     Determines if both objects have the same
        ///     underlying <see cref="System.Guid" /> value.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Guid x, Squid y)
        {
            return y == x; // NB: order of arguments
        }

        /// <summary>
        ///     Determines if both Squid objects do not have the same
        ///     underlying <see cref="System.Guid" /> value.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Squid x, Squid y)
        {
            return !(x == y);
        }

        /// <summary>
        ///     Determines if both objects do not have the same
        ///     underlying <see cref="System.Guid" /> value.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Squid x, Guid y)
        {
            return !(x == y);
        }

        /// <summary>
        ///     Determines if both objects do not have the same
        ///     underlying <see cref="System.Guid" /> value.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Guid x, Squid y)
        {
            return !(x == y);
        }

        /// <summary>
        ///     Implicitly converts the Squid to
        ///     its string equivalent.
        /// </summary>
        /// <param name="oSquid">The o squid.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(Squid oSquid)
        {
            return oSquid.Value;
        }

        /// <summary>
        ///     Implicitly converts the Squid to
        ///     its <see cref="System.Guid" /> equivalent.
        /// </summary>
        /// <param name="oSquid">The o squid.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Guid(Squid oSquid)
        {
            return oSquid.Guid;
        }

        /// <summary>
        ///     Implicitly converts the string to a Squid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Squid(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Empty;

            return TryParse(value, out Squid oSquid) ? oSquid : Empty;
        }

        /// <summary>
        ///     Implicitly converts the <see cref="System.Guid" /> to a Squid.
        /// </summary>
        /// <param name="oGuid">The o unique identifier.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Squid(Guid oGuid)
        {
            return oGuid == Guid.Empty ? Empty : new Squid(oGuid);
        }

        #endregion
    }
}