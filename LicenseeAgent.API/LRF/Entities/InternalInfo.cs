namespace LicenseeAgent.API.LRF.Entities
{
    /// <summary>
    /// LoginInfo class.
    /// </summary>
    public class InternalInfo
    {
        public int UserId { get; set; }

        public int OnUserId { get; set; }

        public int LicenseeId { get; set; }

        public bool IsInternal { get; set; }

        public string Language { get; set; }
    }
}