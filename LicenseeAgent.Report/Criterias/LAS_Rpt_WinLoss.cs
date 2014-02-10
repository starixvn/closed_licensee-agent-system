// --------------------------------------------------------------------------
// <copyright file="LAS_Rpt_WinLoss.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2014.2.7">Created</change>
// </history>

namespace LicenseeAgent.Report.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class LAS_Rpt_WinLoss : CriteriaBase
    {
        public int AgentID { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public byte DrawType { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Rpt_WinLoss";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
