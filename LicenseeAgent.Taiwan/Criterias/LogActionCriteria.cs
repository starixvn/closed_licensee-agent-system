// ----------------------------------------------------------------------
// <copyright file="LogActionCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class LogActionCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user call API.
        /// </value>
        public int UserId { get; set; }

        public string Doing { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// Error message from API.
        /// </value>
        public string ErrMsg { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public int ErrCode { get; set; }

        public override bool IsValid()
        {
            return LicenseeId > 0 && UserId > 0 && !string.IsNullOrEmpty(Doing);
        }
    }
}