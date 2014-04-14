// --------------------------------------------------------------------------
// <copyright file="GetMemberDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class GetMemberDetailCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Transfer_MemberDetail";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}