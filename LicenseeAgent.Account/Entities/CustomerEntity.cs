// ----------------------------------------------------------------------
// <copyright file="CustomerEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Account.Entities
{
    using LicenseeAgent.Account.Core;
    using LicenseeAgent.Taiwan;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerEntity
    {
        [Service(Name = "IBCCustId")]
        public int CustId { get; set; }

        public int Currency { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Phone { get; set; }

        [Service(Name = "MobilePhone")]
        public string Mobile { get; set; }

        public string Fax { get; set; }

        public int Level { get; set; }

        public int NumberOfLevel { get; set; }

        public string LargeFullName
        {
            get
            {
                if (FirstName == Constants.DefaultFullName && LastName == Constants.DefaultFullName)
                {
                    return Constants.DefaultFullName;
                }

                string fullName = string.Format("{0} {1}", FirstName, LastName);

                if (fullName.Length > Constants.LargeStringLength)
                {
                    return string.Format("{0}...", fullName.Substring(0, Constants.LargeStringLength));
                }

                return fullName;
            }
        }

        public string LargePhone
        {
            get
            {
                if (Phone.Length > Constants.LargeStringLength)
                {
                    return string.Format("{0}...", Phone.Substring(0, Constants.LargeStringLength));
                }

                return Phone;
            }
        }

        public string LargeMobile
        {
            get
            {
                if (Mobile.Length > Constants.LargeStringLength)
                {
                    return string.Format("{0}...", Mobile.Substring(0, Constants.LargeStringLength));
                }

                return Mobile;
            }
        }

        public string LargeFax
        {
            get
            {
                if (Fax.Length > Constants.LargeStringLength)
                {
                    return string.Format("{0}...", Fax.Substring(0, Constants.LargeStringLength));
                }

                return Fax;
            }
        }
    }
}