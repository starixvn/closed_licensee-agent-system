// -----------------------------------------------------------------------
// <copyright file="MessageType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Message.Enumerations
{
    /// <summary>
    /// Message type.
    /// </summary>
    public static class MessageType
    {
        /// <summary>
        /// Public Announcement.
        /// </summary>
        public const byte Public = 0;

        /// <summary>
        /// Special Announcement.
        /// </summary>
        public const byte Special = 1;

        /// <summary>
        /// Personal Announcement.
        /// </summary>
        public const byte Personal = 2;
    }
}