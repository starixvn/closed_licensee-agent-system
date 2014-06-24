namespace LicenseeAgent.API.LRF.Entities
{
    using System;

    /// <summary>
    /// LoginInfo class.
    /// </summary>
    [Serializable]
    public class LoginInfo : IEquatable<LoginInfo>
    {
        public LoginInfo()
        {
            CreatedTime = DateTime.Now.ToUniversalTime();
        }

        public int UserId { get; set; }

        public int OnUserId { get; set; }

        public int LicenseeId { get; set; }

        public string Language { get; set; }

        public DateTime CreatedTime { get; private set; }

        public bool Equals(LoginInfo other)
        {
            return CreatedTime == other.CreatedTime
                && UserId == other.UserId
                && OnUserId == other.OnUserId
                && LicenseeId == other.LicenseeId
                && Language == other.Language;
        }
    }
}