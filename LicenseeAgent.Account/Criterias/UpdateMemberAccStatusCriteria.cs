// -----------------------------------------------------------------------
// <copyright file="UpdateMemberAccStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateMemberAccStatusCriteria : CriteriaBase
    {
        public int Updater { get; set; }

        public int CustId { get; set; }

        public byte AccStatus { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_UpdateMemberAccStatus";
        }

        public override bool IsValid()
        {
            return CustId > 0 && Updater > 0;
        }
    }
}
