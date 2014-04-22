// --------------------------------------------------------------------------
// <copyright file="GetProductCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class GetProductCriteria : CriteriaBase
    {
        public int LicenseeID { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Licensee_SelProductList";
        }

        public override bool IsValid()
        {
            return LicenseeID > 0;
        }
    }
}
