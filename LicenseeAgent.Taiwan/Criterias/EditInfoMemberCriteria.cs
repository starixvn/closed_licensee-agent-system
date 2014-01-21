// -----------------------------------------------------------------------
// <copyright file="EditInfoMemberCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditInfoMemberCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public int CustId { get; set; }

        public string Doing { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_EditInfo_Member_Writelog";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0 && UserId > 0 && CustId > 0;
        }
    }
}
