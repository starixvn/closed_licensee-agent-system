// -----------------------------------------------------------------------
// <copyright file="DbConverter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System;
    using Fanex.Common;

    public static class DbConverter
    {
        public static decimal? ToDecimal(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return Converter.ToDecimal(value);
        }
    }
}