namespace LicenseeAgent.CustomerSetting.Utils
{
    using System.Data;
    using Fanex.Common.Extensions;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class CustomerInfoDataReader
    {
        /// <summary>
        /// Readers the customer information data.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Returns agent information.</returns>
        public static CustomerInfo ReaderAgentInfo(IDataReader reader)
        {
            var customerInfo = new CustomerInfo();
            while (reader.Read())
            {
                customerInfo.UserName = reader.ToString("username");
                customerInfo.FirstName = reader.ToString("firstname");
                customerInfo.LastName = reader.ToString("lastname");
                customerInfo.Status = reader.ToByte("status");
                customerInfo.Credit = reader.ToDecimal("credit");
            }

            return customerInfo;
        }

        /// <summary>
        /// Readers the member information data.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="userGroup">The user group.</param>
        /// <returns>Returns member information.</returns>
        public static CustomerInfo ReaderMemberInfo(IDataReader reader, out string userGroup)
        {
            var customerInfo = new CustomerInfo();
            userGroup = string.Empty;
            while (reader.Read())
            {
                customerInfo.UserName = reader.ToString("username");
                customerInfo.FirstName = reader.ToString("firstname");
                customerInfo.LastName = reader.ToString("lastname");
                customerInfo.Status = reader.ToByte("status");
                userGroup = reader.ToString("ugroup");
            }

            return customerInfo;
        }
    }
}