﻿using System;
using System.Text;

namespace FCS.Lib.Utility
{
    /// <summary>
    /// Used for generating UUID based on RFC 4122.
    /// </summary>
    /// <seealso href="http://www.ietf.org/rfc/rfc4122.txt">RFC 4122 - A Universally Unique IDentifier (UUID) URN Namespace</seealso>
    public static class GuidGenerator
    {
        
        /// <summary>
        /// number of bytes in guid
        /// </summary>
        public const int ByteArraySize = 16;

        /// <summary>
        /// multiplex variant info - variant byte
        /// </summary>
        public const int VariantByte = 8;

        /// <summary>
        /// multiplex variant info - variant byte mask
        /// </summary>
        public const int VariantByteMask = 0x3f;

        /// <summary>
        /// multiplex variant info - variant byte shift
        /// </summary>
        public const int VariantByteShift = 0x80;

        /// <summary>
        /// multiplex version info - version byte
        /// </summary>
        public const int VersionByte = 7;

        /// <summary>
        /// multiplex version info - version byte mask
        /// </summary>
        public const int VersionByteMask = 0x0f;

        /// <summary>
        /// multiplex version info version byte shift
        /// </summary>
        public const int VersionByteShift = 4;

        // indexes within the uuid array for certain boundaries
        private const byte TimestampByte = 0;
        private const byte GuidClockSequenceByte = 8;
        private const byte NodeByte = 10;

        // offset to move from 1/1/0001, which is 0-time for .NET, to gregorian 0-time of 10/15/1582
        private static readonly DateTimeOffset GregorianCalendarStart = new(1582, 10, 15, 0, 0, 0, TimeSpan.Zero);

        
        /// <summary>
        /// Default clock sequence
        /// </summary>
        public static byte[] DefaultClockSequence { get; set; }
        /// <summary>
        /// Default node
        /// </summary>
        public static byte[] DefaultNode { get; set; }

        static GuidGenerator()
        {
            DefaultClockSequence = new byte[2];
            DefaultNode = new byte[6];

            var random = new Random();
            random.NextBytes(DefaultClockSequence);
            random.NextBytes(DefaultNode);
        }

        /// <summary>
        /// Set default node
        /// </summary>
        /// <param name="nodeName"></param>
        public static void SetDefaultNode(string nodeName)
        {
            var x = nodeName.GetHashCode();
            var node = $"{x:X}";
            DefaultNode = Encoding.UTF8.GetBytes(node.ToCharArray(), 0, 6);
        }
        
        /// <summary>
        /// Get version
        /// </summary>
        /// <param name="guid"></param>
        /// <returns><see cref="GuidVersion"/></returns>
        public static GuidVersion GetVersion(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            return (GuidVersion)((bytes[VersionByte] & 0xFF) >> VersionByteShift);
        }

        /// <summary>
        /// Get date time offset from guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns><see cref="DateTimeOffset"/></returns>
        public static DateTimeOffset GetDateTimeOffset(Guid guid)
        {
            var bytes = guid.ToByteArray();

            // reverse the version
            bytes[VersionByte] &= VersionByteMask;
            bytes[VersionByte] |= (byte)GuidVersion.TimeBased >> VersionByteShift;

            var timestampBytes = new byte[8];
            Array.Copy(bytes, TimestampByte, timestampBytes, 0, 8);

            var timestamp = BitConverter.ToInt64(timestampBytes, 0);
            var ticks = timestamp + GregorianCalendarStart.Ticks;

            return new DateTimeOffset(ticks, TimeSpan.Zero);
        }

        /// <summary>
        /// get date time from guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns><see cref="DateTime"/></returns>
        public static DateTime GetDateTime(Guid guid)
        {
            return GetDateTimeOffset(guid).DateTime;
        }

        /// <summary>
        /// get local date time from guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns><see cref="DateTime"/></returns>
        public static DateTime GetLocalDateTime(Guid guid)
        {
            return GetDateTimeOffset(guid).LocalDateTime;
        }

        /// <summary>
        /// get utc date time from guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns><see cref="DateTime"/></returns>
        public static DateTime GetUtcDateTime(Guid guid)
        {
            return GetDateTimeOffset(guid).UtcDateTime;
        }

        /// <summary>
        /// Generate time based guid
        /// </summary>
        /// <returns><see cref="Guid"/></returns>
        public static Guid GenerateTimeBasedGuid()
        {
            return GenerateTimeBasedGuid(DateTimeOffset.UtcNow, DefaultClockSequence, DefaultNode);
        }

        /// <summary>
        /// Generate time based guid providing a NodeName string
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns><see cref="Guid"/></returns>
        public static Guid GenerateTimeBasedGuid(string nodeName)
        {
            var x = nodeName.GetHashCode();
            var node = $"{x:X}";
            var defaultNode = Encoding.UTF8.GetBytes(node.ToCharArray(), 0, 6);
            return GenerateTimeBasedGuid(DateTimeOffset.UtcNow, DefaultClockSequence, defaultNode);
        }

        /// <summary>
        /// Generate time based guid providing a valid DateTime object
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns><see cref="Guid"/></returns>
        public static Guid GenerateTimeBasedGuid(DateTime dateTime)
        {
            return GenerateTimeBasedGuid(dateTime, DefaultClockSequence, DefaultNode);
        }

        /// <summary>
        /// Generate time base guid providing a valid DateTimeOffset object
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns><see cref="Guid"/></returns>
        public static Guid GenerateTimeBasedGuid(DateTimeOffset dateTime)
        {
            return GenerateTimeBasedGuid(dateTime, DefaultClockSequence, DefaultNode);
        }

        /// <summary>
        /// Generate time based guid providing a date time, byte array for clock sequence and node
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="clockSequence"></param>
        /// <param name="node"></param>
        /// <returns><see cref="Guid"/></returns>
        public static Guid GenerateTimeBasedGuid(DateTime dateTime, byte[] clockSequence, byte[] node)
        {
            return GenerateTimeBasedGuid(new DateTimeOffset(dateTime), clockSequence, node);
        }

        /// <summary>
        /// Generate time based guid providing a valid DateTimeOffset Object and byte arrays for clock sequence and node
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="clockSequence"></param>
        /// <param name="node"></param>
        /// <returns><see cref="Guid"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Guid GenerateTimeBasedGuid(DateTimeOffset dateTime, byte[] clockSequence, byte[] node)
        {
            if (clockSequence == null)
                throw new ArgumentNullException(nameof(clockSequence));

            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (clockSequence.Length != 2)
                throw new ArgumentOutOfRangeException(nameof(clockSequence), "The clockSequence must be 2 bytes.");

            if (node.Length != 6)
                throw new ArgumentOutOfRangeException(nameof(node), "The node must be 6 bytes.");

            var ticks = (dateTime - GregorianCalendarStart).Ticks;
            var guid = new byte[ByteArraySize];
            var timestamp = BitConverter.GetBytes(ticks);

            // copy node
            Array.Copy(node, 0, guid, NodeByte, Math.Min(6, node.Length));

            // copy clock sequence
            Array.Copy(clockSequence, 0, guid, GuidClockSequenceByte, Math.Min(2, clockSequence.Length));

            // copy timestamp
            Array.Copy(timestamp, 0, guid, TimestampByte, Math.Min(8, timestamp.Length));

            // set the variant
            guid[VariantByte] &= VariantByteMask;
            guid[VariantByte] |= VariantByteShift;

            // set the version
            guid[VersionByte] &= VersionByteMask;
            guid[VersionByte] |= (byte)GuidVersion.TimeBased << VersionByteShift;

            return new Guid(guid);
        }
    }
}
