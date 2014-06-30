namespace LicenseeAgent.Account.Entities
{
    using System;

    /// <summary>
    /// NewMemberOfMonthEntity class.
    /// </summary>
    public class NewMemberOfMonthEntity
    {
        public string Username { get; set; }

        public decimal Credit { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}