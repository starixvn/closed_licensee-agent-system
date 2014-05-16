// --------------------------------------------------------------------------
// <copyright file="InsertResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities
{
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// Result of insert command.
    /// </summary>
    public class InsertResult : Result
    {
        public InsertResult()
            : this(0, string.Empty)
        {
        }

        public InsertResult(int msgId, string msgContent)
        {
            ErrCode = msgId;
            ErrMsg = msgContent;
        }

        public int CustIdReturn { get; set; }

        public string IBCUserName { get; set; }

        public decimal MinPT { get; set; }

        public decimal MaxPT { get; set; }

        public decimal MaxCredit { get; set; }

        public decimal BetLimit { get; set; }

        public bool IsLive { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }
    }
}