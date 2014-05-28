// --------------------------------------------------------------------------
// <copyright file="GetProductCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get List Product Criteria.
    /// </summary>
    public class GetProductCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Licensee_SelProductList";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0;
        }
    }
}
