namespace LicenseeAgent.Core.Entities
{
    using System;

    /// <summary>
    /// Licensee user information.
    /// </summary>
    [Serializable]
    public class LicenseeInfo
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int LicenseeId { get; set; }
    }
}