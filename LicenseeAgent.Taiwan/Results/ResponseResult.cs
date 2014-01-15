// ----------------------------------------------------------------------
// <copyright file="ResponseResult.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Results
{
    using System.Runtime.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [DataContract]
    public class ResponseResult
    {
        [DataMember]
        public int ErrCode { get; set; }

        [DataMember]
        public string ErrMsg { get; set; }
    }
}
