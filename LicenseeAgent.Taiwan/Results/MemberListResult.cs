// ----------------------------------------------------------------------
// <copyright file="MemberListResult.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Results
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [DataContract]
    public class MemberListResult : ResponseResult
    {
        public MemberListResult(int errorCode, string errorMessage)
               : base(errorCode, errorMessage)
        {
        }

        [DataMember(Name = "Data")]
        public List<ServiceMemberEntity> Members { get; set; }
    }
}
