// ----------------------------------------------------------------------
// <copyright file="ResponseResult.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Results
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ResponseResult
    {
        public ResponseResult(int errorCode, string errorMessage)
        {
            ErrCode = errorCode;
            ErrMsg = errorMessage;
            ApiErrCode = errorCode;
            ApiErrMsg = errorMessage;
        }

        [DataMember]
        public int ErrCode { get; set; }

        [DataMember]
        public string ErrMsg { get; set; }

        public int ApiErrCode { get; set; }

        public string ApiErrMsg { get; set; }
    }
}