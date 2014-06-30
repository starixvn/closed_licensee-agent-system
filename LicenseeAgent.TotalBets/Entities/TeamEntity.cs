namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// TeamEntity class.
    /// </summary>
    public class TeamEntity
    {
        private string _teamName;

        public int TeamId { get; set; }

        public string TeamName
        {
            get
            {
                return _teamName ?? (_teamName = DataRefWrapper.GetTeamName(TeamId));
            }

            set
            {
                _teamName = value;
            }
        }
    }
}