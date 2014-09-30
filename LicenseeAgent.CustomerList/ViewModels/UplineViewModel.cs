namespace LicenseeAgent.Customer.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.Customer.Entities;

    /// <summary>
    /// AgentInfoViewModel class.
    /// </summary>
    public class UplineViewModel
    {
        public int CustId { get; set; }

        public string Product { get; set; }

        public IList<AgentEntity> AgentList { get; set; }
    }
}