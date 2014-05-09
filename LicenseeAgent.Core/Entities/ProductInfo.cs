// --------------------------------------------------------------------------
// <copyright file="ProductInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Entities
{
    using System;

    /// <summary>
    /// Product Entity.
    /// </summary>
    [Serializable]
    public class ProductInfo
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
    }
}