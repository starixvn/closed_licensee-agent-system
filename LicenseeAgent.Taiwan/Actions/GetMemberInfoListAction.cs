namespace LicenseeAgent.Taiwan.Actions
{
    using LicenseeAgent.Taiwan.Attributes;

    /// <summary>
    /// GetMemberInfoListAction class.
    /// </summary>
    public class GetMemberInfoListAction : BaseAction
    {
        public int LASAgentId { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "GetMemberInfoList";
            }
        }
    }
}