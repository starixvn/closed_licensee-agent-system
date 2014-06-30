namespace LicenseeAgent.Core.Entities
{
    /// <summary>
    /// LicenseeDomain class.
    /// </summary>
    public class LicenseeDomain
    {
        public string DomainName { get; set; }

        public int LicenseeId { get; set; }

        public string LicenseeName { get; set; }

        public string Environment { get; set; }
    }
}