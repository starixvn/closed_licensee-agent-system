namespace LicenseeAgent.Core.Utility
{
    using System;
    using Fanex.Common;

    /// <summary>
    /// DB Converter class.
    /// </summary>
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