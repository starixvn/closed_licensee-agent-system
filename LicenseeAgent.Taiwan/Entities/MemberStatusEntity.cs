// ----------------------------------------------------------------------
// <copyright file="MemberStatusEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using LicenseeAgent.Taiwan.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [DataContract]
    public class MemberStatusEntity
    {
        [DataMember(Name = "IBCCustID")]
        public int IBCCustId { get; set; }

        [DataMember(Name = "Result")]
        public string Result { get; set; }

        [IgnoreDataMember]
        public short Status
        {
            get
            {
                if (Result.Equals(ServiceSetting.StatusSuccess, StringComparison.CurrentCultureIgnoreCase))
                {
                    return (short)LogType.Success;
                }

                return (short)LogType.Fail;
            }
        }
    }
}
