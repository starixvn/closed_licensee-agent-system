// ----------------------------------------------------------------------
// <copyright file="ResetPasswordAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ResetPasswordAction : LogAction, IAction
    {
        public string SiteName { get; set; }

        public int IBCCustId { get; set; }

        [XmlIgnore]
        public string Password { get; set; }

        [Service]
        [XmlElement(ElementName = "Password")]
        public string EncryptedPassword { get; set; }

        [Service]
        public string ActionName
        {
            get
            {
                return "ResetPassword";
            }
        }
    }
}
