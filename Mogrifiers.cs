// ***********************************************************************
// Assembly         : Inno.Lib
// Author           : FH
// Created          : 27-08-2016
//
// Last Modified By : Frede H.
// Last Modified On : 12-31-2021
// ***********************************************************************
// <copyright file="Mogrifiers.cs" company="FCS">
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
//    along with this program.  If not, see <https://www.gnu.org/licenses/>
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FCS.Lib
{
    /// <summary>
    /// Class Converters
    /// </summary>
    public static class Mogrifiers
    {
        /// <summary>
        ///     Reverse boolean
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool BoolReverse(bool value)
        {
            return !value;
        }

        /// <summary>
        ///     Boolean to integer
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>System.Int32.</returns>
        public static int BoolToInt(bool value)
        {
            return value ? 1 : 0;
        }

        /// <summary>
        ///     Boolean to string
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>System.String.</returns>
        public static string BoolToString(bool value)
        {
            return value ? "true" : "false";
        }


        /// <summary>
        ///     Enum to integer
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        /// <returns>System.Int32.</returns>
        public static int EnumToInt(object enumeration)
        {
            return Convert.ToInt32(enumeration, CultureInfo.InvariantCulture);
        }


        /// <summary>
        ///     Enum to string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string EnumToString(Enum value)
        {
            return value == null ? string.Empty : value.ToString();
        }

        public static IEnumerable<T> GetEnumList<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        /// <summary>
        ///     Integer to boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool IntToBool(int value)
        {
            return value > 0 || value < 0;
        }

        /// <summary>
        ///     Integer to enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>T.</returns>
        public static T IntToEnum<T>(int value)
        {
            return (T) Enum.ToObject(typeof(T), value);
        }

        /// <summary>
        ///     Integer to letter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string IntToLetter(int value)
        {
            var empty = string.Empty;
            var num = 97;
            var str = "";
            var num1 = 0;
            var num2 = 97;
            for (var i = 0; i <= value; i++)
            {
                num1++;
                empty = string.Concat(str, Convert.ToString(Convert.ToChar(num), CultureInfo.InvariantCulture));
                num++;
                if (num1 != 26) continue;
                num1 = 0;
                str = Convert.ToChar(num2).ToString(CultureInfo.InvariantCulture);
                num2++;
                num = 97;
            }

            return empty;
        }

        /// <summary>
        ///     Lists to string using semicolon(;)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns>System.String.</returns>
        public static string ListToString<T>(List<T> list)
        {
            return ListToString(list, ";");
        }

        /// <summary>
        ///     Lists to string userdefined delimiter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>System.String.</returns>
        public static string ListToString<T>(List<T> list, string delimiter)
        {
            var empty = string.Empty;
            if (list == null) return empty;
            var enumerator = (IEnumerator) list.GetType().GetMethod("GetEnumerator")?.Invoke(list, null);
            while (enumerator != null && enumerator.MoveNext())
                if (enumerator.Current != null)
                    empty = string.Concat(empty, enumerator.Current.ToString(), delimiter);

            return empty;
        }

        /// <summary>
        /// Pascals to lower.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string PascalToLower(string value)
        {
            var result = string.Join("-", Regex.Split(value, @"(?<!^)(?=[A-Z])").ToArray());
            return result.ToLower(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     String to bool.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool StringToBool(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            var flag = false;
            var upper = value.ToUpperInvariant();
            if (string.Compare(upper, "true", StringComparison.OrdinalIgnoreCase) == 0)
            {
                flag = true;
            }
            else if (string.CompareOrdinal(upper, "false") == 0)
            {
            }
            else if (string.CompareOrdinal(upper, "1") == 0)
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        ///     String to decimal.
        /// </summary>
        /// <param name="inString">The in string.</param>
        /// <returns>System.Nullable&lt;System.Decimal&gt;.</returns>
        public static decimal? StringToDecimal(string inString)
        {
            if (string.IsNullOrEmpty(inString)) return 0;
            return
                !decimal.TryParse(inString.Replace(",", "").Replace(".", ""), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out var num)
                    ? (decimal?) null
                    : decimal.Divide(num, new decimal((long) 100));
        }

        /// <summary>
        ///     String to enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>T.</returns>
        public static T StringToEnum<T>(string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        ///     String to list using semicolon(;).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<T> StringToList<T>(string value)
        {
            return StringToList<T>(value, ";");
        }

        /// <summary>
        ///     String to list userdefined delimiter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>List&lt;T&gt;.</returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="ArgumentNullException">delimiter</exception>
        public static List<T> StringToList<T>(string value, string delimiter)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(delimiter)) throw new ArgumentNullException(nameof(delimiter));

            var ts = new List<T>();
            var strArrays = value.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in strArrays)
            {
                var o = typeof(T).FullName;
                if (o == null) continue;
                var upperInvariant = o.ToUpperInvariant();
                if (string.CompareOrdinal(upperInvariant, "system.string") == 0)
                {
                    ts.Add((T) Convert.ChangeType(str, typeof(T), CultureInfo.InvariantCulture));
                }
                else if (string.CompareOrdinal(upperInvariant, "system.int32") == 0)
                {
                    ts.Add((T) Convert.ChangeType(str, typeof(T), CultureInfo.InvariantCulture));
                }
                else if (string.CompareOrdinal(upperInvariant, "system.guid") == 0)
                {
                    var guid = new Guid(str);
                    ts.Add((T) Convert.ChangeType(guid, typeof(T), CultureInfo.InvariantCulture));
                }
            }

            return ts;
        }

        /// <summary>
        ///     String to stream using system default encoding.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Stream.</returns>
        public static Stream StringToStream(string value)
        {
            return StringToStream(value, Encoding.Default);
        }


        /// <summary>
        ///     Strings to stream with userdefined encoding.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>Stream.</returns>
        public static Stream StringToStream(string value, Encoding encoding)
        {
            return encoding == null ? null : new MemoryStream(encoding.GetBytes(value ?? ""));
        }

        /// <summary>
        ///     Returns timestamp for current date-time object.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public static long CurrentDateTimeToTimeStamp()
        {
            return Convert.ToUInt32(DateTimeToTimeStamp(DateTime.Now));
        }

        /// <summary>
        ///     Currents the date time to alpha.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string CurrentDateTimeToAlpha()
        {
            var dt = DateTime.UtcNow.ToString("yy MM dd HH MM ss");
            var sb = new StringBuilder();
            var dts = dt.Split(' ');
            sb.Append((char) int.Parse(dts[0]) + 65);
            sb.Append((char) int.Parse(dts[1]) + 65);
            sb.Append((char) int.Parse(dts[2]) + 97);
            sb.Append((char) int.Parse(dts[3]) + 65);
            sb.Append((char) int.Parse(dts[4]) + 97);
            sb.Append((char) int.Parse(dts[5]) + 97);
            return sb.ToString();
        }

        /// <summary>
        ///     Convert a DateTime object to timestamp
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>System.Int64.</returns>
        public static long DateTimeToTimeStamp(DateTime dateTime)
        {
            var bigDate = new DateTime(2038, 1, 19, 0, 0, 0, 0);
            var nixDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            if (dateTime >= bigDate)
                return Convert.ToInt64((bigDate - nixDate).TotalSeconds) +
                       Convert.ToInt64((dateTime - bigDate).TotalSeconds);

            return Convert.ToInt64((dateTime - nixDate).TotalSeconds);
        }

        /// <summary>
        ///     Convert timestamp to DataTime format
        /// </summary>
        /// <param name="timeStamp">The time stamp.</param>
        /// <returns>DateTime.</returns>
        public static DateTime TimeStampToDateTime(long timeStamp)
        {
            var nixDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return nixDate.AddSeconds(timeStamp);
        }

        /// <summary>
        ///     Convert timespan to seconds
        /// </summary>
        /// <param name="timespan">The timespan.</param>
        /// <returns>System.Int32.</returns>
        public static long TimeSpanToSeconds(TimeSpan timespan)
        {
            return Convert.ToUInt32(timespan.Ticks / 10000000L);
        }

        /// <summary>
        ///     Converts seconds to timespan
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns>TimeSpan.</returns>
        public static TimeSpan SecondsToTimeSpan(long seconds)
        {
            return TimeSpan.FromTicks(10000000L * seconds);
        }

        /// <summary>
        ///     Converts timespan to minutes
        /// </summary>
        /// <param name="timespan">The timespan.</param>
        /// <returns>System.Int32.</returns>
        public static long TimespanToMinutes(TimeSpan timespan)
        {
            return Convert.ToUInt32(timespan.Ticks / 10000000L) / 60;
        }
    }
}