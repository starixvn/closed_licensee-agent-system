namespace LicenseeAgent.Core.Entities
{
    using System;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// Information for CSD/IT and Licensee Admin.
    /// </summary>
    [Serializable]
    public class AdminInfo
    {
        private AccountType _accountType;

        public int UserId { get; set; }

        public string UserName { get; set; }

        public AccountType AccountType
        {
            get
            {
                return _accountType;
            }

            set
            {
                _accountType = value;
                if (_accountType != AccountType.CSDIT && _accountType != AccountType.Licensee)
                {
                    throw new InvalidOperationException("Invalid admin role.");
                }
            }
        }
    }
}