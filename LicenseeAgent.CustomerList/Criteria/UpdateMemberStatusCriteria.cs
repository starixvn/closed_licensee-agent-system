// -----------------------------------------------------------------------
// <copyright file="UpdateMemberStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// UpdateMemberStatusCriteria class.
    /// </summary>
    public class UpdateMemberStatusCriteria : CriteriaBase
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