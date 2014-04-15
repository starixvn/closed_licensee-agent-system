// --------------------------------------------------------------------------
// <copyright file="ClientInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Entities
{
    /// <summary>
    /// Client information.
    /// </summary>
    public class ClientInfo
    {
        public string Ip { get; set; }

        public string DomainName { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }
    }
}