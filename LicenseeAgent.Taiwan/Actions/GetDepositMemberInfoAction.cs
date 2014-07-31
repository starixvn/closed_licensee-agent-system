namespace LicenseeAgent.Taiwan.Actions
{
    using LicenseeAgent.Taiwan.Attributes;

    /// <summary>
    /// GetMemberInfoAction class.
    /// </summary>
    public class GetDepositMemberInfoAction : BaseAction
    {
        public string UserName { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "GetDepositMemberInfo";
            }
        }
    }
}