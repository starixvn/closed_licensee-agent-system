namespace LicenseeAgent.CommissionGroup.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Group Commission Insert Criteria.
    /// </summary>
    public class GroupCommInsCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public string GroupName { get; set; }

        public decimal SBComm { get; set; }

        public decimal RacingComm { get; set; }

        public decimal NGComm { get; set; }

        public decimal LCComm { get; set; }

        public decimal RNGComm { get; set; }

        public int AdminId { get; set; }

        public string IP { get; set; }

        public int GroupCommId { get; set; }

        public bool IsDefault { get; set; }

        public bool IsResetDefault { get; set; }

        public string Langkey { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeGroupCommIns";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(GroupName);
        }
    }
}
