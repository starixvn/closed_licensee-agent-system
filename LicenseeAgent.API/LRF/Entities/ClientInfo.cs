namespace LicenseeAgent.API.LRF.Entities
{
    /// <summary>
    /// ClientInfo class.
    /// </summary>
    public class ClientInfo
    {
        public string Ip { get; set; }

        public string DomainName { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }
    }
}