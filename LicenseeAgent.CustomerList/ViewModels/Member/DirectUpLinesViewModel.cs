namespace LicenseeAgent.CustomerList.ViewModels.Member
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.CustomerList.Entities.Agent;

    /// <summary>
    /// DirectUpLinesViewModel class.
    /// </summary>
    public class DirectUpLinesViewModel
    {
        [Required]
        public int CustId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public IList<AgentEntity> AgentList { get; set; }
    }
}