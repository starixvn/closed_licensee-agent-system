// -----------------------------------------------------------------------
// <copyright file="InternalLoginAsUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// InternalLoginAsUserCriteria class.
    /// </summary>
    public class InternalLoginAsUserCriteria : CriteriaBase
    {
        public int AdminId { get; set; }

        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public string UserName { get; set; }

        public string LoginIp { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_InternalLoginAsUser";
        }

        public override bool IsValid()
        {
            return AdminId > 0
                && LicenseeId > 0
                && CurrencyId > 0;
        }
    }
}