namespace LicenseeAgent.CustomerList.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;

    /// <summary>
    /// PendingMemberListViewModel class.
    /// </summary>
    public class PendingMemberListViewModel : IValidatableObject
    {
        [Display(Name = "Member", ResourceType = typeof(Las))]
        public string MemberUserName { get; set; }

        [Display(Name = "DirectAgent", ResourceType = typeof(Las))]
        public string DirectAgentUserName { get; set; }

        public bool IsLastLevel { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var currentSession = SessionManager.CurrentSession;
            var agentInfo = currentSession.AgentInfo;
            IsLastLevel = agentInfo.IsLastLevel;
            if (!IsLastLevel && string.IsNullOrWhiteSpace(DirectAgentUserName))
            {
                yield return new ValidationResult(LangMsg.Msg_2110);
            }
        }
    }
}