namespace LicenseeAgent.Report.Entities
{
    using System;

    /// <summary>
    /// League class.
    /// </summary>
    public class League
    {
        public string LeagueName { get; set; }

        public int LeagueID { get; set; }

        public DateTime EventDate { get; set; }

        public int SportType { get; set; }
    }
}