// ----------------------------------------------------------------------
// <copyright file="CheckUserNameAction.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CheckUserNameAction : IAction
    {
        public string SiteName { get; set; }

        public string UserName { get; set; }

        [Service(IsParamAction = false)]
        public string ActionName
        {
            get
            {
                return "CheckUserName";
            }
        }
    }
}
