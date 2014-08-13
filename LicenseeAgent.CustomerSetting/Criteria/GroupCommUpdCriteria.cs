namespace LicenseeAgent.CustomerSetting.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Group Commission Update Criteria.
    /// </summary>
    public class GroupCommUpdCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int GroupCommId { get; set; }

        public string GroupName { get; set; }

        public decimal SBComm { get; set; }

        public decimal NGComm { get; set; }

        public decimal RacingComm { get; set; }

        public decimal LCComm { get; set; }

        public decimal RNGComm { get; set; }

        public bool IsUpdateNG { get; set; }

        public bool IsUpdateRacing { get; set; }

        public bool IsUpdateLC { get; set; }

        public bool IsUpdateRNG { get; set; }

        public bool IsDefault { get; set; }

        public bool IsResetDefault { get; set; }

        public int AdminId { get; set; }

        public string IP { get; set; }

        public string Langkey { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string StrCustID { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeGroupCommUpd";
        }

        public override bool IsValid()
        {
            return GroupCommId > 0;
        }
    }
}
