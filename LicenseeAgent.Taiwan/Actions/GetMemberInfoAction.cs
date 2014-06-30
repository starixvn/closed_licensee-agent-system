namespace LicenseeAgent.Taiwan.Actions
{
    using LicenseeAgent.Taiwan.Attributes;

    /// <summary>
    /// GetMemberInfoAction class.
    /// </summary>
    public class GetMemberInfoAction : BaseAction
    {
        public int IBCCustId { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "GetMemberInfo";
            }
        }
    }
}