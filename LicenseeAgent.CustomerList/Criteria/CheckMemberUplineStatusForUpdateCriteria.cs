// -----------------------------------------------------------------------
// <copyright file="CheckMemberUpLineStatusForUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// CheckMemberUpLineStatusForUpdateCriteria class.
    /// </summary>
    public class CheckMemberUpLineStatusForUpdateCriteria : CriteriaBase
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