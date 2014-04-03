// ----------------------------------------------------------------------
// <copyright file="LogActionCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criterias
{
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Taiwan.Actions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class LogActionCriteria : CriteriaBase
    {
        public LogActionCriteria(LogAction logAction)
        {
            Url = logAction.Url;
            ApiMethod = logAction.ActionName;
        }

        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public string Url { get; private set; }

        public string ApiMethod { get; private set; }

        public string Doing { get; set; }

        [SpParam(SqlParamName = "ErrMsg")]
        public int ApiErrCode { get; set; }

        public string ApiErrMsg { get; set; }

        public override bool IsValid()
        {
            return LicenseeId > 0 && UserId > 0 && !string.IsNullOrEmpty(Doing);
        }
    }
}