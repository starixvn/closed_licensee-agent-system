// -----------------------------------------------------------------------
// <copyright file="GetIpInformationCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetIpInformationCriteria : CriteriaBase
    {
        public string Ip { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_GetIPInfo_Full";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Ip);
        }
    }
}
