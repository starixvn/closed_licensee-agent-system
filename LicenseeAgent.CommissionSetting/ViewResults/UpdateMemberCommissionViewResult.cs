namespace LicenseeAgent.CommissionSetting.ViewResults
{
    using System.Collections.Generic;
    using LicenseeAgent.CommissionSetting.Entities;

    /// <summary>
    /// UpdateMemberCommissionViewResult class.
    /// </summary>
    public class UpdateMemberCommissionViewResult
    {
        public string Message { get; set; }

        public IList<MemberCommission> MemberCommissions { get; set; }
    }
}