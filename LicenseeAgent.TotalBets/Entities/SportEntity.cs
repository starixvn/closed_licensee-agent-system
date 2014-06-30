namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// League Entity.
    /// </summary>
    public class SportEntity
    {
        private string _sportName;

        public int SportType { get; set; }

        public string SportName
        {
            get
            {
                return _sportName ?? (_sportName = DataRefWrapper.GetSportName(SportType));
            }

            set
            {
                _sportName = value;
            }
        }
    }
}