// --------------------------------------------------------------------------
// <copyright file="Result.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Entities
{
    using System;
    using System.Xml.Serialization;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// Result class.
    /// </summary>
    [Serializable]
    public class Result
    {
        public const int SuccessValue = 0;
        public const int FailureValue = -1;
        public const int NoCurrencyPermission = -2;
        public const int UnderMaintenance = -3;
        public const int InvalidModel = -4;
        public const int Unauthorized = -5;
        public const int AccessDenied = -6;
        public const int ApiError = -7;

        public Result()
            : this(SuccessValue, string.Empty)
        {
        }

        public Result(int errCode)
            : this(errCode, ResourceManager.GetErrMsg(errCode))
        {
        }

        public Result(int errCode, string errMsg)
        {
            ErrCode = errCode;
            ErrMsg = errMsg;
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}