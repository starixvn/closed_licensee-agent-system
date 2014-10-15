namespace LicenseeAgent.Core.Enumerations
{
    /// <summary>
    /// TicketStatus class.
    /// </summary>
    public enum TicketStatus
    {
        /// <summary>
        /// Running ticket.
        /// </summary>
        Running = 0,

        /// <summary>
        /// Waiting ticket.
        /// </summary>
        Waiting = 1,

        /// <summary>
        /// Reject ticket.
        /// </summary>
        Reject = 101,

        /// <summary>
        /// Void ticket.
        /// </summary>
        Void = 102,

        /// <summary>
        /// Refund ticket.
        /// </summary>
        Refund = 103,

        /// <summary>
        /// Draw ticket.
        /// </summary>
        Draw = 111,

        /// <summary>
        /// Won ticket.
        /// </summary>
        Won = 112,

        /// <summary>
        /// Lose ticket.
        /// </summary>
        Lose = 113,

        /// <summary>
        /// Completed ticket.
        /// </summary>
        Completed = 131,

        /// <summary>
        /// Approved ticket.
        /// </summary>
        Approved = 132,

        /// <summary>
        /// Balance ticket.
        /// </summary>
        Balance = 241,

        /// <summary>
        /// Sum ticket.
        /// </summary>
        Sum = 142
    }
}