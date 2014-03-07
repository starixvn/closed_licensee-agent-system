// --------------------------------------------------------------------------
// <copyright file="Statement.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2014.2.21">Created</change>
// </history>

namespace LicenseeAgent.Report.Entities
{
    using System;
    using LicenseeAgent.Taiwan;

    /// <summary>
    /// Statement Entity.
    /// </summary>
    public class Statement
    {
        private string _fullName;

        [Service(Name = "IBCCustId")]
        public int CustId { get; set; }

        [Service(Name = "LASAgentId")]
        public int AgentId { get; set; }
        
        public DateTime Date { get; set; }

        public string Username { get; set; }

        public string FullName {
            get
            {
                if (_fullName == null)
                {
                    _fullName = LastName + " " + FirstName;
                }

                return _fullName;
            }

            set
            {
                _fullName = value;
            }
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal OpeningBalance { get; set; }

        public decimal Turnover { get; set; }

        public decimal Betting { get; set; }

        public decimal Winloss { get; set; }

        public decimal Outstanding { get; set; }

        public decimal Commission { get; set; }

        public decimal TransferAmt { get; set; }

        public decimal ClosingBalance { get; set; }
    }
}
