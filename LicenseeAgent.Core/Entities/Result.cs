// --------------------------------------------------------------------------
// <copyright file="Result.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Core.Entities
{
    using System;
    using LicenseeAgent.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class Result
    {
        public const int SuccessValue = 0;
        public const int FailureValue = -1;

        public Result()
            : this(SuccessValue, string.Empty)
        {
        }

        public Result(string msgContent)
            : this(FailureValue, msgContent)
        {
        }

        public Result(int msgId)
            : this(msgId, ResourceManager.GetErrorMsg(msgId))
        {
        }

        public Result(int msgId, string msgContent)
        {
            MsgId = msgId;
            MsgContent = msgContent;
        }

        public int MsgId { get; set; }

        public string MsgContent { get; set; }
    }
}