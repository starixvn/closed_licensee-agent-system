// ----------------------------------------------------------------------
// <copyright file="UplineEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Entities
{
    using LicenseeAgent.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UplineEntity
    {
        public int UplineCustId { get; set; }

        public string UserName { get; set; }

        public int Level { get; set; }

        public string ShortUserName
        {
            get
            {
                if (UserName.Length > Constants.ShortStringLength)
                {
                    return string.Format("{0}...", UserName.Substring(0, Constants.ShortStringLength));
                }

                return UserName;
            }
        }

        public string MediumUserName
        {
            get
            {
                if (UserName.Length > Constants.MediumStringLength)
                {
                    return string.Format("{0}...", UserName.Substring(0, Constants.MediumStringLength));
                }

                return UserName;
            }
        }
    }
}