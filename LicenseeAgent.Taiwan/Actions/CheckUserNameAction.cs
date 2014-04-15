// ----------------------------------------------------------------------
// <copyright file="CheckUserNameAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    using LicenseeAgent.Taiwan.Attributes;

    /// <summary>
    /// CheckUserNameAction class.
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
