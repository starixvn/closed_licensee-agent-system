namespace LicenseeAgent.Taiwan.Criteria
{
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Enumerations;

    /// <summary>
    /// SingleLogActionCriteria class.
    /// </summary>
    public abstract class SingleLogActionCriteria : LogActionCriteria
    {
        protected SingleLogActionCriteria(LogAction logAction)
            : base(logAction)
        {
            LogId = 0;
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// Status of API action.
        /// </value>
        public LogType Status
        {
            get
            {
                return LogId == 0
                    ? LogType.BeforeCall
                    : ApiErrCode == Result.SuccessValue ? LogType.Success : LogType.Fail;
            }
        }

        public long LogId { get; set; }

        public long LogIdReturn { get; set; }
    }
}