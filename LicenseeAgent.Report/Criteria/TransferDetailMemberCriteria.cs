// --------------------------------------------------------------------------
// <copyright file="TransferDetailMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Transfer Detail Member Criteria.
    /// </summary>
    public class TransferDetailMemberCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Statement_MemberTransferDetail";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}