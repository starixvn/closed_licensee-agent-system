namespace LicenseeAgent.Core.Entities
{
    using System;

    /// <summary>
    /// SelectBalanceUpDay class.
    /// </summary>
    [Serializable]
    public class SelectBalanceUpDay
    {
        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }
    }
}