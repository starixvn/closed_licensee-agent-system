namespace LicenseeAgent.Taiwan.Actions
{
    using LicenseeAgent.Taiwan.Attributes;

    /// <summary>
    /// CheckUserNameAction class.
    /// </summary>
    public class CheckUserNameAction : BaseAction
    {
        public string UserName { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "CheckUserName";
            }
        }
    }
}