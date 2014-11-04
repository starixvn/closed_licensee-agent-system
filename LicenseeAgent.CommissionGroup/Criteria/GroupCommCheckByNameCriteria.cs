namespace LicenseeAgent.CommissionGroup.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Group Commission Check By Name Criteria.
    /// </summary>
    public class GroupCommCheckByNameCriteria : CriteriaBase
    {
        public int LicenseeID { get; set; }

        public string GroupName { get; set; }

        public bool IsAvailable { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GroupCommCheckByName";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(GroupName);
        }
    }
}
