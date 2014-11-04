namespace LicenseeAgent.CommissionGroup.ViewModels
{
    /// <summary>
    /// Commission Group Base View Model class.
    /// </summary>
    public class CommissionGroupBaseViewModel
    {
        public bool HasSportbook { get; set; }

        public bool HasRacing { get; set; }

        public bool HasNumberGame { get; set; }

        public bool HasLiveCasino { get; set; }

        public bool HasRNGCasino { get; set; }

        public int ProductCount { get; set; }
    }
}