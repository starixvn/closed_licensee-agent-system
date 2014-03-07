// --------------------------------------------------------------------------
// <copyright file="SignInViewResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Account.ViewResults
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SignInViewResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show confirm dialog.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is confirm]; otherwise, <c>false</c>.
        /// </value>
        public bool IsConfirm { get; set; }

        public string Message { get; set; }
    }
}