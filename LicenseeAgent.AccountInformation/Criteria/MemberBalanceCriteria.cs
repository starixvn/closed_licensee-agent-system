// ----------------------------------------------------------------------
// <copyright file="MemberBalanceCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// MemberBalanceCriteria class.
    /// </summary>
    public class MemberBalanceCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal GivenCredit { get; set; }

        public decimal BetCredit { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Balance_SelMemberBalance";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}
