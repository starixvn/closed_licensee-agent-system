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

        public decimal MinPTOutput { get; set; }

        public decimal MaxPTOutput { get; set; }

        public decimal MaxCreditOutput { get; set; }

        public decimal BetLimitOutput { get; set; }

        public bool IsLiveOutput { get; set; }

        public int BetLimitSportType { get; set; }

        public int BetLimitBetType { get; set; }

        public int PTSportType { get; set; }

        public int PTBetType { get; set; }
    }
}