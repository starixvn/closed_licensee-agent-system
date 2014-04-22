// --------------------------------------------------------------------------
// <copyright file="Product.cs" company="Nexcel Solutions Vietnam">
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
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
    }
}