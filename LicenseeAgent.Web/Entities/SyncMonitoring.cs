namespace LicenseeAgent.Web.Entities
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Monitoring Data's synchronization.
    /// </summary>
    public class SyncMonitoring
    {
        public int KeyId { get; set; }

        public int CustId { get; set; }

        public int UserId { get; set; }

        public string ActionName { get; set; }

        public int ActionId { get; set; }

        public string Command { get; set; }

        public DateTime LogDate { get; set; }

        public string InputParams { get; set; }

        public string Remark { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Verified")]
        public string Url { get; set; }

        public int RunCount { get; set; }
    }
}