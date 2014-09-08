namespace LicenseeAgent.Account.Entities.Internal
{
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// LicenseeEntity class.
    /// </summary>
    public class LicenseeEntity
    {
        public int LicenseeId { get; set; }

        public string LicenseeName { get; set; }

        public int LegendType { get; set; }
    }
}