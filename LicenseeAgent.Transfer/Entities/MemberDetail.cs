namespace LicenseeAgent.Transfer.Entities
{
    using System;

    /// <summary>
    /// MemberDetail class.
    /// </summary>
    public class MemberDetail
    {
        public string UserName { get; set; }

        public decimal BetCredit { get; set; }

        public decimal YesterdayBalance { get; set; }

        public DateTime LastLogin { get; set; }

        public DateTime LastTransaction { get; set; }
    }
}