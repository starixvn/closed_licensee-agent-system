namespace LicenseeAgent.CustomerList.Criteria.Member
{
    using Fanex.Data.Repository;

    /// <summary>
    /// UpdateMemberStatusCriteria class.
    /// </summary>
    public class UpdateMemberStatusCriteria : CriteriaBase
    {
        public int Updater { get; set; }

        public int CustId { get; set; }

        public byte AccStatus { get; set; }

        [SpParam(SqlParamName = "MsgId")]
        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_UpdateMemberAccStatus";
        }

        public override bool IsValid()
        {
            return CustId > 0 && Updater > 0;
        }
    }
}