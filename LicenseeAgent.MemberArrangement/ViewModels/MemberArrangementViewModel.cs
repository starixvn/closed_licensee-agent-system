namespace LicenseeAgent.MemberArrangement.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.MemberArrangement.Enumerations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// Member Arrangement View Model.
    /// </summary>
    public class MemberArrangementViewModel : IValidatableObject
    {
        [Display(Name = "Agent", ResourceType = typeof(Las))]
        public string AgentName { get; set; }

        [Display(Name = "Member", ResourceType = typeof(Las))]
        [Required(ErrorMessageResourceName = "MemberIsEmpty", ErrorMessageResourceType = typeof(Las))]
        public string MemberName { get; set; }

        [Required]
        public ArrangementMode Mode { get; set; }

        [Display(Name = "OldAgent", ResourceType = typeof(Las))]
        public string OldAgentName { get; set; }

        [Display(Name = "NewAgent", ResourceType = typeof(Las))]
        public string NewAgentName { get; set; }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (Mode == ArrangementMode.SwitchFromNormal)
            {
                if (string.IsNullOrEmpty(AgentName))
                {
                    result.Add(new ValidationResult(Las.AgentUserNameIsEmpty));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(OldAgentName))
                {
                    result.Add(new ValidationResult(Las.OldAgentIsEmpty));
                }

                if (string.IsNullOrEmpty(NewAgentName))
                {
                    result.Add(new ValidationResult(Las.NewAgentIsEmpty));
                }
            }

            return result;
        }
    }
}