// ----------------------------------------------------------------------
// <copyright file="UplineEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Account.Entities
{
    using LicenseeAgent.Account.Core;

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
                if(UserName.Length > Constants.LeftStringLength)
                {
                    return UserName.Substring(0, Constants.LeftStringLength);
                }

                return UserName;
            }
        }
    }
}