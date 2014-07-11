namespace LicenseeAgent.Core.Enumerations
{
    /// <summary>
    /// List of account types in LAS system.
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// Used in Authorization.
        /// </summary>
        All,

        /// <summary>
        /// Account type is CSD/IT or Licensee.
        /// </summary>
        Admin,

        /// <summary>
        /// Account type is CSD/IT.
        /// </summary>
        CSDIT,

        /// <summary>
        /// Account type is Licensee.
        /// </summary>
        Licensee,

        /// <summary>
        /// Account type is Agent (any levels).
        /// </summary>
        Agent,

        /// <summary>
        /// The agent level is 0.
        /// </summary>
        AgentLevel0,

        /// <summary>
        /// The agent level is 1.
        /// </summary>
        AgentLevel1,

        /// <summary>
        /// The agent level is k.
        /// </summary>
        AgentLevelk,

        /// <summary>
        /// The agent last level.
        /// </summary>
        AgentLeveln,

        /// <summary>
        /// Account type is Member.
        /// </summary>
        Member
    }
}