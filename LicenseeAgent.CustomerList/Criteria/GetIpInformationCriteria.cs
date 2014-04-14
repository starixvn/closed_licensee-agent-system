// -----------------------------------------------------------------------
// <copyright file="GetIpInformationCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.NAP.Framework.Data;

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