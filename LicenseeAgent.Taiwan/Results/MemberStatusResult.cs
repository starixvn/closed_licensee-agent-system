// ----------------------------------------------------------------------
// <copyright file="MemberStatusResult.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Results
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [DataContract]
    public class MemberStatusResult : ResponseResult
    {
        public MemberStatusResult(int errorCode, string errorMessage)
               : base(errorCode, errorMessage)
        {
        }

        [DataMember(Name = "Data")]
        public Collection<MemberStatusEntity> MemberStatuses { get; set; }
    }
}
