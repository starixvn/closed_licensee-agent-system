// -----------------------------------------------------------------------
// <copyright file="CheckMemberUplineStatusForUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class CheckMemberUplineStatusForUpdateCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_CheckMemberUplineStatusForUpdate";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}