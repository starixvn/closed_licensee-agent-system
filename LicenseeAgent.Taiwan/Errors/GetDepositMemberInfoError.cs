namespace LicenseeAgent.Taiwan.Errors
{
    /// <summary>
    /// GetDepositMemberInfoListError class.
    /// </summary>
    public enum GetDepositMemberInfoError : int
    {
        /// <summary>
        /// API is success.
        /// </summary>
        Success = 0,

        /// <summary>
        /// The data not found.
        /// </summary>
        DataNotFound = 1,

        /// <summary>
        /// The system error.
        /// </summary>
        SystemError = 2
    }
}