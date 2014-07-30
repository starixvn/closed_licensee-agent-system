namespace LicenseeAgent.Taiwan.Entities
{
    /// <summary>
    /// DepositMemberInfo class.
    /// </summary>
    public class DepositMemberInfo
    {
        public int IBCCustID { get; set; }

        public string Username { get; set; }

        public bool IsClosed { get; set; }

        public int LASAgentID { get; set; }
    }
}