namespace LicenseeAgent.MemberArrangement.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Member Information Criteria.
    /// </summary>
    public class GetMemberInfoCriteria : CriteriaBase
    {
        public int LiceneeId { get; set; }

        public string MemberName { get; set; }

        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_GetMemberInfo";
        }

        public override bool IsValid()
        {
            return CustId > 0 || !string.IsNullOrEmpty(MemberName);
        }
    }
}