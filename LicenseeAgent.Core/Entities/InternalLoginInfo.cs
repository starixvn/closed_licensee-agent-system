// --------------------------------------------------------------------------
// <copyright file="InternalLoginInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Entities
{
    using System;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class InternalLoginInfo
    {
        public InternalLoginInfo()
        {
            Timeout = DateTime.Now.AddMinutes(3);
        }

        /// <summary>
        /// Gets or sets the type of the account who created this object. It can be CSD/IT or licensee user.
        /// </summary>
        /// <value>
        /// The type of the account.
        /// </value>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// Gets or sets the name of the admin.
        /// </summary>
        /// <value>
        /// The name of the admin.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the licensee ID.
        /// </summary>
        /// <value>
        /// The licensee ID.
        /// </value>
        public int LicenseeId { get; set; }

        /// <summary>
        /// Gets or sets the currency ID.
        /// </summary>
        /// <value>
        /// The currency ID.
        /// </value>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the agent.
        /// </summary>
        /// <value>
        /// The name of the agent.
        /// </value>
        public string AgentName { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>
        /// The user ID.
        /// </value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the on user ID.
        /// </summary>
        /// <value>
        /// The on user ID.
        /// </value>
        public int OnUserId { get; set; }

        public DateTime Timeout { get; private set; }

        public bool IsExpired
        {
            get
            {
                return DateTime.Now > Timeout;
            }
        }
    }
}