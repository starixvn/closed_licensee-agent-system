namespace LicenseeAgent.BetList.ViewModels
{
    using System.Collections.ObjectModel;
    using Entities;

    /// <summary>
    /// Bet List Details View Model.
    /// </summary>
    public class BetListDetailsViewModel
    {
        public BetListDetailsViewModel()
        {
            BetListDetails = new Collection<BetListDetails>();
        }

        public Collection<BetListDetails> BetListDetails { get; set; }

        public bool IsDeposit { get; set; }
    }
}