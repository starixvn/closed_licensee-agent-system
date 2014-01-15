// ----------------------------------------------------------------------
// <copyright file="MemberResult.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Results
{
    using System.Runtime.Serialization;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [DataContract]
    public class MemberResult : ResponseResult
    {
        [DataMember(Name = "Data")]
        public ServiceMemberEntity Member { get; set; }
    }
}
