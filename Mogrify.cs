// ***********************************************************************
// Assembly         : FCS.Lib.Utility
// Author           : FH
// Created          : 27-08-2016
//
// Last Modified By : Frede H.
// Last Modified On : 02-24-2022
// ***********************************************************************
// <copyright file="Mogrify.cs" company="FCS">
//    Copyright (C) 2022 FCS Frede's Computer Services.
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the Affero GNU General Public License as
//    published by the Free Software Foundation, either version 3 of the
//    License, or (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    Affero GNU General Public License for more details.
//
//    You should have received a copy of the Affero GNU General Public License
//    along with this program.  If not, see [https://www.gnu.org/licenses/agpl-3.0.en.html]
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

namespace FCS.Lib.Utility
{
    public static class Mogrify
    {
        public static int MonthFromTimestamp(long timeStamp)
        {
            return TimeStampToDateTime(timeStamp).Month;
        }

        public static bool TimestampInMonth(long timestamp, int month)
        {
            return TimeStampToDateTime(timestamp).Month == month;
        }

        public static Dictionary<string,long> DateToTimestampRange(DateTime dateTime)
        {
            var dt1 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,0,0,0);
            var dt2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
            return new Dictionary<string, long>{
                { "lower", DateTimeToTimeStamp(dt1) },
                { "upper", DateTimeToTimeStamp(dt2) }
            };
        }

        public static long IsoDateToTimestamp(string isoDateString)
        {
            var result = DateTime.TryParse(isoDateString, out var test);
            if (!result) 
                return 0;
            return $"{test:yyyy-MM-dd}" == isoDateString ? DateTimeToTimeStamp(test) : 0;
        }

        public static string TimestampToIsoDate(long timestamp)
        {
            return $"{TimeStampToDateTime(timestamp):yyyy-MM-dd}";
        }

        public static bool BoolReverse(bool value)
        {
            return !value;
        }

        public static int BoolToInt(bool value)
        {
            return value ? 1 : 0;
        }

        public static string BoolToString(bool value)
        {
            return value ? "true" : "false";
        }

        public static int EnumToInt(object enumeration)
        {
            return Convert.ToInt32(enumeration, CultureInfo.InvariantCulture);
        }

        public static string EnumToString(Enum value)
        {
            return value == null ? string.Empty : value.ToString();
        }

        public static IEnumerable<T> GetEnumList<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static bool IntToBool(int value)
        {
            return value > 0 || value < 0;
        }

        public static T IntToEnum<T>(int value)
        {
            return (T) Enum.ToObject(typeof(T), value);
        }

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
        
        public static string ListToString<T>(List<T> list)
        {
            return ListToString(list, ";");
        }
        
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
        
        public static string PascalToLower(string value)
        {
            var result = string.Join("-", Regex.Split(value, @"(?<!^)(?=[A-Z])").ToArray());
            return result.ToLower(CultureInfo.InvariantCulture);
        }
        
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
        public static decimal? StringToDecimal(string inString)
        {
            if (string.IsNullOrEmpty(inString)) return 0;
            return
                !decimal.TryParse(inString.Replace(",", "").Replace(".", ""), NumberStyles.Number,
                    CultureInfo.InvariantCulture, out var num)
                    ? (decimal?) null
                    : decimal.Divide(num, new decimal((long) 100));
        }
        public static T StringToEnum<T>(string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }
        public static List<T> StringToList<T>(string value)
        {
            return StringToList<T>(value, ";");
        }
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
        public static Stream StringToStream(string value)
        {
            return StringToStream(value, Encoding.Default);
        }
        public static Stream StringToStream(string value, Encoding encoding)
        {
            return encoding == null ? null : new MemoryStream(encoding.GetBytes(value ?? ""));
        }
        public static long CurrentDateTimeToTimeStamp()
        {
            return Convert.ToUInt32(DateTimeToTimeStamp(DateTime.Now));
        }
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
        
        public static long DateTimeToTimeStamp(DateTime dateTime)
        {
            var bigDate = new DateTime(2038, 1, 19, 0, 0, 0, 0);
            var nixDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            if (dateTime >= bigDate)
                return Convert.ToInt64((bigDate - nixDate).TotalSeconds) +
                       Convert.ToInt64((dateTime - bigDate).TotalSeconds);

            return Convert.ToInt64((dateTime - nixDate).TotalSeconds);
        }
        
        public static DateTime TimeStampToDateTime(long timeStamp)
        {
            var nixDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return nixDate.AddSeconds(timeStamp);
        }
        
        public static long TimeSpanToSeconds(TimeSpan timespan)
        {
            return Convert.ToUInt32(timespan.Ticks / 10000000L);
        }
        
        public static TimeSpan SecondsToTimeSpan(long seconds)
        {
            return TimeSpan.FromTicks(10000000L * seconds);
        }
        
        public static long TimespanToMinutes(TimeSpan timespan)
        {
            return Convert.ToUInt32(timespan.Ticks / 10000000L) / 60;
        }
    }
}