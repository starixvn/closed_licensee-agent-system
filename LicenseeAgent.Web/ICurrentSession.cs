namespace LicenseeAgent.Web
{
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// Contains all properties of an authenticated user.
    /// </summary>
    public interface ICurrentSession
    {
        AdminInfo AdminInfo { get; }

        AgentInfo AgentInfo { get; }

        int LoginUserId { get; }

        string LoginUserName { get; }

        AccountType LoginAccountType { get; }

        int OnUserId { get; }

        int OnLicenseeId { get; }

        string Language { get; set; }

        bool IsValid { get; }

        bool IsAdmin { get; }

        bool IsCSDIT { get; }

        /// <summary>
        /// Gets a value indicating whether internal login from LRF.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is internal; otherwise, <c>false</c>.
        /// </value>
        bool IsInternalLRF { get; }

        bool IsLicensee { get; }

        void UpdateAgent(AgentInfo agentInfo);
    }
}