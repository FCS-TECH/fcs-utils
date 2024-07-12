// ***********************************************************************
// Assembly         : FCS.Lib.Utility
// Filename         : Mogrify.cs
// Author           : Frede Hundewadt
// Created          : 2024 03 29 12:36
// 
// Last Modified By : root
// Last Modified On : 2024 04 11 13:03
// ***********************************************************************
// <copyright company="FCS">
//     Copyright (C) 2024-2024 FCS Frede's Computer Service.
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace FCS.Lib.Utility;

/// <summary>
///     Mogrify between units
/// </summary>
public static class Mogrify
{
    /// <summary>
    /// Virtual Month Start Timestamp from date and day number
    /// </summary>
    /// <param name="date"></param>
    /// <param name="beginAt"></param>
    /// <returns>
    /// returns a timestamp value for the beginning of the virtual month
    /// </returns>
    public static long VirtualMonthStartTimestamp(string date, int beginAt)
    {
        return DateTimeToTimeStamp(VirtualMonthStart(date, beginAt));
    }


    /// <summary>
    /// Virtual Month End Timestamp from date and day number
    /// </summary>
    /// <param name="date"></param>
    /// <param name="beginAt"></param>
    /// <returns>
    /// returns a timestamp value for the en of the virtual month
    /// </returns>
    public static long VirtualMonthEndTimestamp(string date, int beginAt)
    {
        return DateTimeToTimeStamp(VirtualMonthEnd(date, beginAt));
    }


    /// <summary>
    /// Virtual Month from date and day number
    /// </summary>
    /// <param name="date"></param>
    /// <param name="beginAt"></param>
    /// <returns>
    /// a new datetime object as the beginning af the virtual month
    /// </returns>
    public static DateTime VirtualMonthStart(string date, int beginAt)
    {
        // create dateTime object from string
        var dt = DateTime.Parse(date);
        // if the day number of the date parameter is less then beginAtDay
        if (dt.Day < beginAt)
        {
            // if month number of the date parameter is 1
            return dt.Month == 1
                // month virtual month started in december the preceding year
                // return a new datetime object
                ? new DateTime(dt.Year -1, 12, beginAt)
                // otherwise the month started in the preceding month
                // return a new datetime object
                : new DateTime(dt.Year, dt.Month - 1, beginAt);
        }
        // return a new dateTime object from 
        return new DateTime(dt.Year, dt.Month, beginAt);
    }


    /// <summary>
    /// Virtual Month end from date and day number
    /// </summary>
    /// <param name="date"></param>
    /// <param name="beginAt"></param>
    /// <returns>
    /// returns a new datetime object as the end of the virtual month
    /// </returns>
    public static DateTime VirtualMonthEnd(string date, int beginAt)
    {
        var dt = DateTime.Parse(date);
        var endDay = EndDay(dt, beginAt);
        if (dt.Day < beginAt)
        {
            return dt.Month == 12
                ? new DateTime(dt.Year + 1, 1, endDay)
                : new DateTime(dt.Year, dt.Month, endDay);
        }

        if (dt.Month == 12)
        {
            return beginAt == 1
                ? new DateTime(dt.Year, dt.Month, endDay)
                : new DateTime(dt.Year + 1, 1, endDay);

        }

        return beginAt == 1
            ? new DateTime(dt.Year, dt.Month, endDay)
            : new DateTime(dt.Year, dt.Month + 1, endDay);
    }


    /// <summary>
    /// Internal helper
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="beginAt"></param>
    /// <returns>return the number of the day if begin day is first day of month</returns>
    private static int EndDay(DateTime dt, int beginAt)
    {
        var endDay = beginAt - 1;
        if (endDay != 0) return endDay;
        if (DateTime.IsLeapYear(dt.Year) && dt.Month == 2)
            return 29;
        return dt.Month switch
        {
            1 or 3 or 5 or 7 or 8 or 10 or 12 => 31,
            4 or 6 or 9 or 11 => 30,
            2 => 28,
            _ => endDay
        };
    }


    /// <summary>
    ///     Sanitize phone number string - remove countrycode and alpha characters
    /// </summary>
    /// <param name="phone"></param>
    /// <returns>returns a string with unneeded chars removed</returns>
    public static string SanitizePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return "";
        phone = phone.Replace("+45", "").Replace("+46", "").Replace("+47", "");
        var regexObj = new Regex(@"[^\d]");
        return regexObj.Replace(phone, "");
    }


    /// <summary>
    ///     Sanitize zipcode string - alpha characters
    /// </summary>
    /// <param name="zipCode"></param>
    /// <returns></returns>
    public static string SanitizeZipCode(string zipCode)
    {
        if (string.IsNullOrWhiteSpace(zipCode))
            return "";
        var regexObj = new Regex(@"[^\d]");
        return regexObj.Replace(zipCode, "");
    }


    /// <summary>
    ///     Get month from timestamp
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns>integer</returns>
    public static int MonthFromTimestamp(long timeStamp)
    {
        return TimeStampToDateTime(timeStamp).Month;
    }


    /// <summary>
    ///     validate if timestamp occurs in month
    /// </summary>
    /// <param name="timestamp"></param>
    /// <param name="month"></param>
    /// <returns>boolean</returns>
    public static bool TimestampInMonth(long timestamp, int month)
    {
        return TimeStampToDateTime(timestamp).Month == month;
    }


    /// <summary>
    ///     Get timestamp range for given datetime
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns>dictionary</returns>
    public static Dictionary<string, long> DateToTimestampRange(DateTime dateTime)
    {
        var dt1 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        var dt2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        return new Dictionary<string, long>
        {
            { "lower", DateTimeToTimeStamp(dt1) },
            { "upper", DateTimeToTimeStamp(dt2) }
        };
    }


    /// <summary>
    ///     return iso8601 string from timestamp
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static string TimestampToIso8601(long timestamp)
    {
        return DateTimeIso8601(TimeStampToDateTime(timestamp));
    }


    /// <summary>
    ///     return date as ISO 2000-01-01T00:00:00.0000000
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string DateTimeIso8601(DateTime date)
    {
        return date.ToString("o", CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     ISO date 2000-01-01 to timestamp
    /// </summary>
    /// <param name="isoDateString"></param>
    /// <returns>long</returns>
    public static long IsoDateToTimestamp(string isoDateString)
    {
        var result = DateTime.TryParse(isoDateString, out var test);
        if (!result)
            return 0;
        return $"{test:yyyy-MM-dd}" == isoDateString ? DateTimeToTimeStamp(test) : 0;
    }


    /// <summary>
    ///     ISO date from timestamp 2000-01-01
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns>string yyyy-MM-dd</returns>
    public static string TimestampToIsoDate(long timestamp)
    {
        return $"{TimeStampToDateTime(timestamp):yyyy-MM-dd}";
    }


    /// <summary>
    ///     get timestamp from current date time
    /// </summary>
    /// <returns>
    ///     <see cref="long" />
    /// </returns>
    public static long CurrentDateTimeToTimeStamp()
    {
        return Convert.ToUInt32(DateTimeToTimeStamp(DateTime.Now));
    }


    /// <summary>
    ///     get timestamp from date time
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns>
    ///     <see cref="long" />
    /// </returns>
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
    ///     get date time from timestamp
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns>
    ///     <see cref="DateTime" />
    /// </returns>
    public static DateTime TimeStampToDateTime(long timeStamp)
    {
        var nixDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return nixDate.AddSeconds(timeStamp);
    }


    /// <summary>
    ///     get seconds from timespan
    /// </summary>
    /// <param name="timespan"></param>
    /// <returns>
    ///     <see cref="long" />
    /// </returns>
    public static long TimeSpanToSeconds(TimeSpan timespan)
    {
        return Convert.ToUInt32(timespan.Ticks / 10000000L);
    }


    /// <summary>
    ///     get timespan from seconds
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns>
    ///     <see cref="TimeSpan" />
    /// </returns>
    public static TimeSpan SecondsToTimeSpan(long seconds)
    {
        return TimeSpan.FromTicks(10000000L * seconds);
    }


    /// <summary>
    ///     get minutes from timespan
    /// </summary>
    /// <param name="timespan"></param>
    /// <returns>
    ///     <see cref="long" />
    /// </returns>
    public static long TimespanToMinutes(TimeSpan timespan)
    {
        return Convert.ToUInt32(timespan.Ticks / 10000000L) / 60;
    }


    /// <summary>
    ///     reverse boolean
    /// </summary>
    /// <param name="value"></param>
    /// <returns>bool</returns>
    public static bool BoolReverse(bool value)
    {
        return !value;
    }


    /// <summary>
    ///     number from bool
    /// </summary>
    /// <param name="value"></param>
    /// <returns>integer</returns>
    public static int BoolToInt(bool value)
    {
        return value ? 1 : 0;
    }


    /// <summary>
    ///     string from bool
    /// </summary>
    /// <param name="value"></param>
    /// <returns>string true/false</returns>
    public static string BoolToString(bool value)
    {
        return value ? "true" : "false";
    }


    /// <summary>
    ///     get bool from integer
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IntToBool(int value)
    {
        return value is > 0 or < 0;
    }


    /// <summary>
    ///     get the number value from enum
    /// </summary>
    /// <param name="enumeration"></param>
    /// <returns>int</returns>
    public static int EnumToInt(object enumeration)
    {
        return Convert.ToInt32(enumeration, CultureInfo.InvariantCulture);
    }


    /// <summary>
    ///     get string from enum
    /// </summary>
    /// <param name="value"></param>
    /// <returns>string - name of the enum</returns>
    public static string EnumToString(Enum value)
    {
        return value == null ? string.Empty : value.ToString();
    }


    /// <summary>
    ///     get list of enum of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>list of enum</returns>
    public static IEnumerable<T> GetEnumList<T>()
    {
        return (T[])Enum.GetValues(typeof(T));
    }


    /// <summary>
    ///     get enum from integer of type T
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T IntToEnum<T>(int value)
    {
        return (T)Enum.ToObject(typeof(T), value);
    }


    /// <summary>
    ///     get string from list using semicolon separator
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string ListToString<T>(List<T> list)
    {
        return ListToString(list, ";");
    }


    /// <summary>
    ///     get string from list using delimiter
    /// </summary>
    /// <param name="list"></param>
    /// <param name="delimiter"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    ///     <see cref="string" />
    /// </returns>
    public static string ListToString<T>(List<T> list, string delimiter)
    {
        var empty = string.Empty;
        if (list == null) return empty;
        var enumerator = (IEnumerator)list.GetType().GetMethod("GetEnumerator")?.Invoke(list, null);
        while (enumerator != null && enumerator.MoveNext())
            if (enumerator.Current != null)
                empty = string.Concat(empty, enumerator.Current.ToString(), delimiter);

        return empty;
    }


    /// <summary>
    ///     get lowercase dash separated string from Pascal case
    /// </summary>
    /// <param name="value"></param>
    /// <returns>
    ///     <see cref="string" />
    /// </returns>
    public static string PascalToLower(string value)
    {
        var result = string.Join("-", Regex.Split(value, @"(?<!^)(?=[A-Z])").ToArray());
        return result.ToLower(CultureInfo.InvariantCulture);
    }


    /// <summary>
    ///     get bool from string
    /// </summary>
    /// <param name="value"></param>
    /// <returns>
    ///     <see cref="bool" />
    /// </returns>
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
    ///     get decimal from string
    /// </summary>
    /// <param name="inString"></param>
    /// <returns>
    ///     <see cref="decimal" />
    /// </returns>
    public static decimal? StringToDecimal(string inString)
    {
        if (string.IsNullOrEmpty(inString)) return 0;
        return
            !decimal.TryParse(inString.Replace(",", "").Replace(".", ""), NumberStyles.Number,
                CultureInfo.InvariantCulture, out var num)
                ? null
                : decimal.Divide(num, new decimal((long)100));
    }


    /// <summary>
    ///     get enum of type T from string
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    ///     <see cref="Enum" />
    /// </returns>
    public static T StringToEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }


    /// <summary>
    ///     get list from string using semicolon
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    ///     <see cref="List{T}" />
    /// </returns>
    public static List<T> StringToList<T>(string value)
    {
        return StringToList<T>(value, ";");
    }


    /// <summary>
    ///     get list from string using delimiter
    /// </summary>
    /// <param name="value"></param>
    /// <param name="delimiter"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    ///     <see cref="List{T}" />
    /// </returns>
    /// <exception cref="ArgumentNullException"></exception>
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
                ts.Add((T)Convert.ChangeType(str, typeof(T), CultureInfo.InvariantCulture));
            }
            else if (string.CompareOrdinal(upperInvariant, "system.int32") == 0)
            {
                ts.Add((T)Convert.ChangeType(str, typeof(T), CultureInfo.InvariantCulture));
            }
            else if (string.CompareOrdinal(upperInvariant, "system.guid") == 0)
            {
                var guid = new Guid(str);
                ts.Add((T)Convert.ChangeType(guid, typeof(T), CultureInfo.InvariantCulture));
            }
        }

        return ts;
    }


    /// <summary>
    ///     get string from stream (default encoding)
    /// </summary>
    /// <param name="value"></param>
    /// <returns>
    ///     <see cref="Stream" />
    /// </returns>
    public static Stream StringToStream(string value)
    {
        return StringToStream(value, Encoding.Default);
    }


    /// <summary>
    ///     get stream from string (using encoding)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="encoding"></param>
    /// <returns>
    ///     <see cref="Stream" />
    /// </returns>
    public static Stream StringToStream(string value, Encoding encoding)
    {
        return encoding == null ? null : new MemoryStream(encoding.GetBytes(value ?? ""));
    }


    ///// <summary>
    ///// get string from date time
    ///// </summary>
    ///// <returns></returns>
    //public static string CurrentDateTimeToAlpha()
    //{
    //    var dt = DateTime.UtcNow.ToString("yy MM dd HH MM ss");
    //    var sb = new StringBuilder();
    //    var dts = dt.Split(' ');
    //    sb.Append((char) int.Parse(dts[0]) + 65);
    //    sb.Append((char) int.Parse(dts[1]) + 65);
    //    sb.Append((char) int.Parse(dts[2]) + 97);
    //    sb.Append((char) int.Parse(dts[3]) + 65);
    //    sb.Append((char) int.Parse(dts[4]) + 97);
    //    sb.Append((char) int.Parse(dts[5]) + 97);
    //    return sb.ToString();
    //}

    ///// <summary>
    ///// integer to letter
    ///// </summary>
    ///// <param name="value"></param>
    ///// <returns>string</returns>
    //public static string IntToLetter(int value)
    //{
    //    var empty = string.Empty;
    //    var num = 97;
    //    var str = "";
    //    var num1 = 0;
    //    var num2 = 97;
    //    for (var i = 0; i <= value; i++)
    //    {
    //        num1++;
    //        empty = string.Concat(str, Convert.ToString(Convert.ToChar(num), CultureInfo.InvariantCulture));
    //        num++;
    //        if (num1 != 26) continue;
    //        num1 = 0;
    //        str = Convert.ToChar(num2).ToString(CultureInfo.InvariantCulture);
    //        num2++;
    //        num = 97;
    //    }
    //    return empty;
    //}
}