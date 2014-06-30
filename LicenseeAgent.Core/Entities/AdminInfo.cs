namespace LicenseeAgent.Core.Entities
{
    using System;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// CSD/IT sign-in information.
    /// </summary>
    [Serializable]
    public class AdminInfo
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserRole { get; set; }

        public AccountType AccountType
        {
            get
            {
                if (string.Equals(UserRole, "Admin", StringComparison.InvariantCultureIgnoreCase))
                {
                    return AccountType.CSD_IT;
                }

                return AccountType.Licensee;
            }
        }
    }
}