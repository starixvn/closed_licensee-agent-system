namespace LicenseeAgent.API.LRF.Entities
{
    /// <summary>
    /// LicenseeDomain class.
    /// </summary>
    public class LicenseeDomain
    {
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// Licensee id.
        /// </value>
        public int LicenseeId { get; set; }
    }
}