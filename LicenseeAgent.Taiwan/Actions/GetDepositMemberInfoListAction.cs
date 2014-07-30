namespace LicenseeAgent.Taiwan.Actions
{
    using LicenseeAgent.Taiwan.Attributes;
    using LicenseeAgent.Taiwan.Enumerations;

    /// <summary>
    /// GetDepositMemberInfoListAction class.
    /// </summary>
    public class GetDepositMemberInfoListAction : BaseAction
    {
        public int LASAgentId { get; set; }

        public int DepositOption { get; set; }

        public string UserName { get; set; }

        public string IBCCustIDList { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "GetDepositMemberInfoList";
            }
        }
    }
}