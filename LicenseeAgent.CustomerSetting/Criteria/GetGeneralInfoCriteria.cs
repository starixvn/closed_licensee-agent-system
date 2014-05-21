// --------------------------------------------------------------------------
// <copyright file="GetGeneralInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetGeneralInfoCriteria class.
    /// </summary>
    public class GetGeneralInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetGeneralInfo";
        }

        public override bool IsValid()
        {
            return AgentId > 0 &&
                CustId > 0;
        }
    }
}