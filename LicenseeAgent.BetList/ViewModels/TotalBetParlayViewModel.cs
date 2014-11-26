namespace LicenseeAgent.BetList.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TotalBetParlayViewModel class.
    /// </summary>
    public class TotalBetParlayViewModel : IValidatableObject
    {
        [Required]
        public int BetType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (BetType != (int)LicenseeAgent.Core.Enumerations.BetType.BetType_9
                && BetType != (int)LicenseeAgent.Core.Enumerations.BetType.BetType_29)
            {
                result.Add(new ValidationResult("Invalid bet type"));
            }

            return result;
        }
    }
}