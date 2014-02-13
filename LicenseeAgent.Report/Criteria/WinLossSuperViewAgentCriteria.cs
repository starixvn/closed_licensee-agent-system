// --------------------------------------------------------------------------
// <copyright file="WinLossSuperViewAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2014.2.12">Created</change>
// </history>

namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class WinLossSuperViewAgentCriteria : WinlossBase
    {
        public override string GetSettingKey()
        {
            return "LAS_Rpt_WinLoss_SuperViewAgent";
        }
    }
}
