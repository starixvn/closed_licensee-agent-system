// --------------------------------------------------------------------------
// <copyright file="CheckCurrencyPermissionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class CheckCurrencyPermissionCriteria : CriteriaBase
    {
        public int AdminId { get; set; }

        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_CheckCurrencyPermission";
        }

        public override bool IsValid()
        {
            return AdminId > 0
                && LicenseeId > 0
                && CurrencyId > 0;
        }
    }
}