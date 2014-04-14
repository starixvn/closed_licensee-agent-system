// ----------------------------------------------------------------------
// <copyright file="CustomerEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Entities
{
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Taiwan.Attributes;

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

        public string FullName
        {
            get
            {
                if ((string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
                    || (FirstName.Equals(Constants.EmptyString) && LastName.Equals(Constants.EmptyString)))
                {
                    return Constants.EmptyString;
                }

                string fullName = string.IsNullOrEmpty(FirstName) ? LastName : string.Format("{0} {1}", FirstName, LastName);

                if (fullName.Length > Constants.LargeStringLength)
                {
                    return string.Format("{0}...", fullName.Substring(0, Constants.LargeStringLength));
                }

                return fullName;
            }
        }

        public string PhoneDisplay
        {
            get
            {
                if (string.IsNullOrEmpty(Phone))
                {
                    return Constants.EmptyString;
                }

                if (Phone.Length > Constants.LargeStringLength)
                {
                    return string.Format("{0}...", Phone.Substring(0, Constants.LargeStringLength));
                }

                return Phone;
            }
        }

        public string MobileDisplay
        {
            get
            {
                if (string.IsNullOrEmpty(Mobile))
                {
                    return Constants.EmptyString;
                }

                if (Mobile.Length > Constants.LargeStringLength)
                {
                    return string.Format("{0}...", Mobile.Substring(0, Constants.LargeStringLength));
                }

                return Mobile;
            }
        }

        public string FaxDisplay
        {
            get
            {
                if (string.IsNullOrEmpty(Fax))
                {
                    return Constants.EmptyString;
                }

                if (Fax.Length > Constants.LargeStringLength)
                {
                    return string.Format("{0}...", Fax.Substring(0, Constants.LargeStringLength));
                }

                return Fax;
            }
        }
    }
}