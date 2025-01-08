// // ***********************************************************************
// // Solution         : Inno.Api.v2
// // Assembly         : FCS.Lib.Utility
// // Filename         : Mogrify.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FCS.Lib.Utility;

/// <summary>
///     Provides a collection of utility methods for various data transformations and operations.
/// </summary>
/// <remarks>
///     This static class includes methods for handling date and time conversions,
///     string manipulations, enumeration conversions, and other general-purpose utilities.
///     It is designed to simplify common operations and ensure consistency across the application.
/// </remarks>
public static class Mogrify
{
    /// <summary>
    ///     Calculates the Unix timestamp for the start of a virtual month based on the provided date and starting day.
    /// </summary>
    /// <param name="date">The date in string format (e.g., "yyyy-MM-dd") to calculate the virtual month's start.</param>
    /// <param name="beginAt">The day of the month that marks the beginning of the virtual month (e.g., 1 for the 1st day).</param>
    /// <returns>The Unix timestamp representing the start of the virtual month.</returns>
    public static long VirtualMonthStartTimestamp(string date, int beginAt)
    {
        return DateTimeToTimeStamp(VirtualMonthStart(date, beginAt));
    }


    /// <summary>
    ///     Calculates the Unix timestamp for the virtual end of a month based on the provided date and the specified starting
    ///     day of the month.
    /// </summary>
    /// <param name="date">The date in string format (e.g., ISO 8601) used to determine the virtual month's end.</param>
    /// <param name="beginAt">The day of the month that marks the start of the virtual month (e.g., 1 for the 1st day).</param>
    /// <returns>The Unix timestamp representing the virtual month's end.</returns>
    public static long VirtualMonthEndTimestamp(string date, int beginAt)
    {
        return DateTimeToTimeStamp(VirtualMonthEnd(date, beginAt));
    }


    /// <summary>
    ///     Calculates the start date of a virtual month based on the given date and the specified starting day of the month.
    /// </summary>
    /// <param name="date">The date in string format to determine the virtual month's start date.</param>
    /// <param name="beginAt">The day of the month that marks the start of the virtual month.</param>
    /// <returns>A <see cref="DateTime" /> representing the start date of the virtual month.</returns>
    /// <exception cref="FormatException">Thrown when the <paramref name="date" /> is not in a valid date format.</exception>
    public static DateTime VirtualMonthStart(string date, int beginAt)
    {
        // create dateTime object from string
        var dt = DateTime.Parse(date);
        // if the day number of the date parameter is less then beginAtDay
        if (dt.Day < beginAt)
            // if month number of the date parameter is 1
            return dt.Month == 1
                // month virtual month started in december the preceding year
                // return a new datetime object
                ? new DateTime(dt.Year - 1, 12, beginAt)
                // otherwise the month started in the preceding month
                // return a new datetime object
                : new DateTime(dt.Year, dt.Month - 1, beginAt);
        // return a new dateTime object from 
        return new DateTime(dt.Year, dt.Month, beginAt);
    }

    /// <summary>
    ///     Calculates the virtual month-end date based on the provided date and the starting day of the virtual month.
    /// </summary>
    /// <param name="date">The date in string format to calculate the virtual month-end for.</param>
    /// <param name="beginAt">The starting day of the virtual month.</param>
    /// <returns>A <see cref="DateTime" /> representing the virtual month-end date.</returns>
    public static DateTime VirtualMonthEnd(string date, int beginAt)
    {
        var dt = DateTime.Parse(date);
        var endDay = EndDay(dt, beginAt);
        if (dt.Day < beginAt)
            return dt.Month == 12
                ? new DateTime(dt.Year + 1, 1, endDay)
                : new DateTime(dt.Year, dt.Month, endDay);

        if (dt.Month == 12)
            return beginAt == 1
                ? new DateTime(dt.Year, dt.Month, endDay)
                : new DateTime(dt.Year + 1, 1, endDay);

        return beginAt == 1
            ? new DateTime(dt.Year, dt.Month, endDay)
            : new DateTime(dt.Year, dt.Month + 1, endDay);
    }


    /// <summary>
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="beginAt"></param>
    /// <returns></returns>
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
    ///     Sanitizes the provided phone number by removing specific country codes and non-numeric characters.
    /// </summary>
    /// <param name="phone">The phone number to sanitize.</param>
    /// <returns>
    ///     A sanitized phone number containing only numeric characters.
    ///     Returns an empty string if the input is null, empty, or consists solely of whitespace.
    /// </returns>
    public static string SanitizePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return "";
        phone = phone.Replace("+45", "").Replace("+46", "").Replace("+47", "");
        var regexObj = new Regex(@"[^\d]");
        return regexObj.Replace(phone, "");
    }


    /// <summary>
    ///     Sanitizes the provided zip code by removing all non-numeric characters.
    /// </summary>
    /// <param name="zipCode">The zip code to sanitize.</param>
    /// <returns>
    ///     A sanitized zip code containing only numeric characters. Returns an empty string if the input is null, empty,
    ///     or consists only of whitespace.
    /// </returns>
    public static string SanitizeZipCode(string zipCode)
    {
        if (string.IsNullOrWhiteSpace(zipCode))
            return "";
        var regexObj = new Regex(@"[^\d]");
        return regexObj.Replace(zipCode, "");
    }


    /// <summary>
    ///     Extracts the month component from a given Unix timestamp.
    /// </summary>
    /// <param name="timeStamp">The Unix timestamp to extract the month from.</param>
    /// <returns>An integer representing the month (1 for January, 2 for February, etc.).</returns>
    public static int MonthFromTimestamp(long timeStamp)
    {
        return TimeStampToDateTime(timeStamp).Month;
    }

    /// <summary>
    ///     Determines whether the specified Unix timestamp falls within the specified month.
    /// </summary>
    /// <param name="timestamp">The Unix timestamp to evaluate.</param>
    /// <param name="month">The month to check against, represented as an integer (1 for January, 2 for February, etc.).</param>
    /// <returns>
    ///     <c>true</c> if the timestamp corresponds to the specified month; otherwise, <c>false</c>.
    /// </returns>
    public static bool TimestampInMonth(long timestamp, int month)
    {
        return TimeStampToDateTime(timestamp).Month == month;
    }


    /// <summary>
    ///     Converts a given <see cref="DateTime" /> to a range of Unix timestamps representing the start and end of the day.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime" /> to be converted.</param>
    /// <returns>
    ///     A <see cref="Dictionary{TKey, TValue}" /> containing two key-value pairs:
    ///     <list type="bullet">
    ///         <item>
    ///             <term>"lower"</term>
    ///             <description>The Unix timestamp for the start of the day (00:00:00).</description>
    ///         </item>
    ///         <item>
    ///             <term>"upper"</term>
    ///             <description>The Unix timestamp for the end of the day (23:59:59).</description>
    ///         </item>
    ///     </list>
    /// </returns>
    /// <remarks>
    ///     This method is useful for generating a timestamp range for a specific day,
    ///     which can be used in filtering or querying operations.
    /// </remarks>
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
    ///     Converts a Unix timestamp to an ISO 8601 formatted string.
    /// </summary>
    /// <param name="timestamp">The Unix timestamp to convert.</param>
    /// <returns>An ISO 8601 formatted string representing the given timestamp.</returns>
    public static string TimestampToIso8601(long timestamp)
    {
        return DateTimeIso8601(TimeStampToDateTime(timestamp));
    }


    /// <summary>
    ///     Converts the specified <see cref="DateTime" /> to an ISO 8601 formatted string.
    /// </summary>
    /// <param name="date">The <see cref="DateTime" /> to be converted.</param>
    /// <returns>A string representation of the <paramref name="date" /> in ISO 8601 format.</returns>
    public static string DateTimeIso8601(DateTime date)
    {
        return date.ToString("o", CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Converts an ISO 8601 formatted date string to a Unix timestamp.
    /// </summary>
    /// <param name="isoDateString">The ISO 8601 formatted date string to convert.</param>
    /// <returns>
    ///     The Unix timestamp representing the specified date, or 0 if the input string is invalid
    ///     or does not match the ISO 8601 format.
    /// </returns>
    public static long IsoDateToTimestamp(string isoDateString)
    {
        var result = DateTime.TryParse(isoDateString, out var test);
        if (!result)
            return 0;
        return $"{test:yyyy-MM-dd}" == isoDateString ? DateTimeToTimeStamp(test) : 0;
    }


    /// <summary>
    ///     Converts a Unix timestamp to an ISO 8601 date string in the format "yyyy-MM-dd".
    /// </summary>
    /// <param name="timestamp">The Unix timestamp to convert.</param>
    /// <returns>A string representing the ISO 8601 date.</returns>
    public static string TimestampToIsoDate(long timestamp)
    {
        return $"{TimeStampToDateTime(timestamp):yyyy-MM-dd}";
    }

    /// <summary>
    ///     Converts the current date and time to a Unix timestamp.
    /// </summary>
    /// <returns>
    ///     A <see cref="long" /> representing the current date and time as a Unix timestamp.
    /// </returns>
    public static long CurrentDateTimeToTimeStamp()
    {
        return Convert.ToUInt32(DateTimeToTimeStamp(DateTime.Now));
    }


    /// <summary>
    ///     Converts a <see cref="DateTime" /> object to a Unix timestamp.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime" /> to convert.</param>
    /// <returns>
    ///     A <see cref="long" /> representing the Unix timestamp, which is the number of seconds
    ///     that have elapsed since January 1, 1970 (UTC).
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
    ///     Converts a Unix timestamp to a <see cref="DateTime" /> object.
    /// </summary>
    /// <param name="timeStamp">The Unix timestamp to convert, representing the number of seconds since January 1, 1970 (UTC).</param>
    /// <returns>A <see cref="DateTime" /> object representing the specified timestamp in UTC.</returns>
    public static DateTime TimeStampToDateTime(long timeStamp)
    {
        var nixDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return nixDate.AddSeconds(timeStamp);
    }


    /// <summary>
    ///     Converts a <see cref="TimeSpan" /> to its equivalent total number of seconds.
    /// </summary>
    /// <param name="timespan">The <see cref="TimeSpan" /> to convert.</param>
    /// <returns>The total number of seconds represented by the <paramref name="timespan" />.</returns>
    public static long TimeSpanToSeconds(TimeSpan timespan)
    {
        return Convert.ToUInt32(timespan.Ticks / 10000000L);
    }

    /// <summary>
    ///     Converts a given number of seconds into a <see cref="TimeSpan" /> object.
    /// </summary>
    /// <param name="seconds">The number of seconds to convert.</param>
    /// <returns>A <see cref="TimeSpan" /> representing the specified number of seconds.</returns>
    public static TimeSpan SecondsToTimeSpan(long seconds)
    {
        return TimeSpan.FromTicks(10000000L * seconds);
    }


    /// <summary>
    ///     Converts a given <see cref="TimeSpan" /> to its equivalent total number of minutes.
    /// </summary>
    /// <param name="timespan">The <see cref="TimeSpan" /> to be converted.</param>
    /// <returns>The total number of minutes represented by the <paramref name="timespan" />.</returns>
    public static long TimespanToMinutes(TimeSpan timespan)
    {
        return Convert.ToUInt32(timespan.Ticks / 10000000L) / 60;
    }

    /// <summary>
    ///     Reverses the given boolean value.
    /// </summary>
    /// <param name="value">The boolean value to be reversed.</param>
    /// <returns>
    ///     <c>true</c> if <paramref name="value" /> is <c>false</c>;
    ///     otherwise, <c>false</c> if <paramref name="value" /> is <c>true</c>.
    /// </returns>
    public static bool BoolReverse(bool value)
    {
        return !value;
    }


    /// <summary>
    ///     Converts a boolean value to its integer representation.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <returns>Returns 1 if <paramref name="value" /> is <c>true</c>, otherwise returns 0.</returns>
    public static int BoolToInt(bool value)
    {
        return value ? 1 : 0;
    }


    /// <summary>
    ///     Converts a boolean value to its string representation.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <returns>
    ///     A string representation of the boolean value: "true" if the value is <c>true</c>,
    ///     or "false" if the value is <c>false</c>.
    /// </returns>
    public static string BoolToString(bool value)
    {
        return value ? "true" : "false";
    }


    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IntToBool(int value)
    {
        return value is > 0 or < 0;
    }


    /// <summary>
    ///     Converts an enumeration value to its equivalent integer representation.
    /// </summary>
    /// <param name="enumeration">The enumeration value to be converted. Must be a valid enumeration type.</param>
    /// <returns>The integer representation of the specified enumeration value.</returns>
    /// <exception cref="InvalidCastException">
    ///     Thrown if the provided <paramref name="enumeration" /> is not a valid enumeration type.
    /// </exception>
    /// <remarks>
    ///     This method uses <see cref="Convert.ToInt32(object, IFormatProvider)" /> to perform the conversion.
    ///     Ensure that the input is a valid enumeration value to avoid runtime exceptions.
    /// </remarks>
    public static int EnumToInt(object enumeration)
    {
        return Convert.ToInt32(enumeration, CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Converts the specified enumeration value to its string representation.
    /// </summary>
    /// <param name="value">The enumeration value to convert. If <c>null</c>, an empty string is returned.</param>
    /// <returns>The string representation of the enumeration value, or an empty string if the value is <c>null</c>.</returns>
    public static string EnumToString(Enum value)
    {
        return value == null ? string.Empty : value.ToString();
    }


    /// <summary>
    ///     Retrieves a collection of all values of a specified enumeration type.
    /// </summary>
    /// <typeparam name="T">
    ///     The enumeration type whose values are to be retrieved. This type parameter must be an enumeration.
    /// </typeparam>
    /// <returns>
    ///     An <see cref="IEnumerable{T}" /> containing all values of the specified enumeration type.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <typeparamref name="T" /> is not an enumeration type.
    /// </exception>
    public static IEnumerable<T> GetEnumList<T>()
    {
        return (T[])Enum.GetValues(typeof(T));
    }


    /// <summary>
    ///     Converts an integer value to its corresponding enumeration value of type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The enumeration type to which the integer value will be converted.</typeparam>
    /// <param name="value">The integer value to be converted to the enumeration.</param>
    /// <returns>The enumeration value of type <typeparamref name="T" /> that corresponds to the provided integer value.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <typeparamref name="T" /> is not an enumeration type.
    /// </exception>
    public static T IntToEnum<T>(int value)
    {
        return (T)Enum.ToObject(typeof(T), value);
    }


    /// <summary>
    ///     Converts a list of elements into a single string, with elements separated by a default delimiter.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list of elements to be converted into a string.</param>
    /// <returns>
    ///     A string representation of the list, where each element is separated by a semicolon (";").
    ///     If the list is <c>null</c>, an empty string is returned.
    /// </returns>
    public static string ListToString<T>(List<T> list)
    {
        return ListToString(list, ";");
    }

    /// <summary>
    ///     Converts a list of elements to a single string, with elements separated by a specified delimiter.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">
    ///     The list of elements to convert to a string. If the list is <c>null</c>, an empty string is
    ///     returned.
    /// </param>
    /// <param name="delimiter">The string used to separate the elements in the resulting string.</param>
    /// <returns>
    ///     A string representation of the list, with elements separated by the specified delimiter.
    ///     If the list is empty or <c>null</c>, an empty string is returned.
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
    ///     Converts a PascalCase string to a lowercase string with words separated by hyphens.
    /// </summary>
    /// <param name="value">The PascalCase string to be converted.</param>
    /// <returns>A lowercase string with words separated by hyphens.</returns>
    public static string PascalToLower(string value)
    {
        var result = string.Join("-", Regex.Split(value, @"(?<!^)(?=[A-Z])").ToArray());
        return result.ToLower(CultureInfo.InvariantCulture);
    }


    /// <summary>
    ///     Converts a string representation of a boolean value to its <see cref="bool" /> equivalent.
    /// </summary>
    /// <param name="value">
    ///     The string to convert. Accepted values are "true", "false", "1", or "0",
    ///     in a case-insensitive manner. Any other value or <c>null</c> will result in <c>false</c>.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the input string represents "true" or "1"; otherwise, <c>false</c>.
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
    ///     Converts a string representation of a number into a nullable decimal value.
    /// </summary>
    /// <param name="inString">
    ///     The input string containing the number to be converted.
    ///     It may include commas or periods as separators.
    /// </param>
    /// <returns>
    ///     A nullable decimal value representing the converted number.
    ///     Returns <c>null</c> if the conversion fails or the input string is invalid.
    /// </returns>
    /// <remarks>
    ///     This method attempts to parse the input string as a number, removing any commas or periods.
    ///     If the parsing is successful, the resulting number is divided by 100 to produce the final decimal value.
    /// </remarks>
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
    ///     Converts a string representation of an enumeration value to its strongly-typed enumeration equivalent.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration to convert to. Must be an enumeration type.</typeparam>
    /// <param name="value">The string representation of the enumeration value.</param>
    /// <returns>The enumeration value of type <typeparamref name="T" /> corresponding to the specified string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if <typeparamref name="T" /> is not an enumeration type or if
    ///     <paramref name="value" /> is not a valid enumeration name.
    /// </exception>
    public static T StringToEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }


    /// <summary>
    ///     Converts a delimited string into a list of strongly-typed elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the resulting list.</typeparam>
    /// <param name="value">The input string containing delimited values.</param>
    /// <returns>
    ///     A list of elements of type <typeparamref name="T" /> parsed from the input string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="value" /> is <c>null</c> or empty.
    /// </exception>
    /// <remarks>
    ///     This method uses a default delimiter of ";" to split the input string into individual elements.
    /// </remarks>
    public static List<T> StringToList<T>(string value)
    {
        return StringToList<T>(value, ";");
    }

    /// <summary>
    ///     Converts a delimited string into a list of strongly-typed elements.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of elements in the resulting list. Supported types include <see cref="string" />, <see cref="int" />, and
    ///     <see cref="Guid" />.
    /// </typeparam>
    /// <param name="value">
    ///     The input string to be converted. Each element in the string should be separated by the specified delimiter.
    /// </param>
    /// <param name="delimiter">
    ///     The string used to separate elements in the input string.
    /// </param>
    /// <returns>
    ///     A list of elements of type <typeparamref name="T" /> parsed from the input string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="value" /> or <paramref name="delimiter" /> is <c>null</c> or empty.
    /// </exception>
    /// <remarks>
    ///     This method supports parsing elements of type <see cref="string" />, <see cref="int" />, and <see cref="Guid" />.
    ///     For other types, ensure that the input string can be converted to the specified type.
    /// </remarks>
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
    ///     Converts a string into a stream using the default encoding.
    /// </summary>
    /// <param name="value">The string to be converted into a stream. If <c>null</c>, an empty stream will be returned.</param>
    /// <returns>
    ///     A <see cref="Stream" /> containing the encoded bytes of the input string using the default encoding.
    /// </returns>
    public static Stream StringToStream(string value)
    {
        return StringToStream(value, Encoding.Default);
    }


    /// <summary>
    ///     Converts a string into a stream using the specified encoding.
    /// </summary>
    /// <param name="value">The string to be converted into a stream. If <c>null</c>, an empty stream will be returned.</param>
    /// <param name="encoding">
    ///     The character encoding to use for the conversion. If <c>null</c>, the method returns <c>null</c>
    ///     .
    /// </param>
    /// <returns>
    ///     A <see cref="Stream" /> containing the encoded bytes of the input string, or <c>null</c> if the encoding is
    ///     <c>null</c>.
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