namespace LicenseeAgent.CustomerList.ViewModels.Member
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// MemberStatusViewModel class.
    /// </summary>
    public class MemberStatusViewModel
    {
        [Required]
        public int MemberId { get; set; }

        [Required]
        public int AgentId { get; set; }

        [Required]
        public int Status { get; set; }
    }
}