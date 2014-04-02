// ----------------------------------------------------------------------
// <copyright file="CheckUserNameAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CheckUserNameAction : BaseAction
    {
        public string UserName { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "CheckUserName";
            }
        }
    }
}
