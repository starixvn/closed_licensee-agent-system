namespace LicenseeAgent.Core.Enumerations
{
    /// <summary>
    /// Agent Password Status.
    /// </summary>
    public enum PasswordStatus
    {
        /// <summary>
        /// Password status is normal.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Password is going to be expired.
        /// </summary>
        Alert = 1,

        /// <summary>
        /// Password is expired.
        /// </summary>
        Expired = 2,

        /// <summary>
        /// Password must be changed on first login.
        /// </summary>
        FirstLogin = 3
    }
}