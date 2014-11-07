namespace LicenseeAgent.MemberArrangement.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Agent Info By Member Criteria.
    /// </summary>
    public class GetAgentInfoByMemberCriteria : CriteriaBase
    {
        public int LiceneeId { get; set; }

        public string MemberName { get; set; }

        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_GetAgentInfoByMember";
        }

        public override bool IsValid()
        {
            return CustId > 0 || !string.IsNullOrEmpty(MemberName);
        }
    }
}