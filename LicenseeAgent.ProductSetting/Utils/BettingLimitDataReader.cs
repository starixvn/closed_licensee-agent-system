namespace LicenseeAgent.ProductSetting.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using Fanex.Common.Extensions;
    using Fanex.Data.Repository;
    using LicenseeAgent.ProductSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class BettingLimitDataReader
    {
        /// <summary>
        /// Read bet limits setting.
        /// </summary>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="getUplineBetlimit">Get up line bet limits or not.</param>
        /// <returns>Returns list of betting limit item.</returns>
        public static IList<BettingLimitItem> ReadBetLimitData(
            IDataReader reader,
            bool getUplineBetlimit = false)
        {
            var bettingLimits = new List<BettingLimitItem>();
            while (reader.Read())
            {                
                var bettingLimit = MapBettingLimitData(reader);

                if (!getUplineBetlimit)
                {
                    bettingLimit.MaxMinBet = bettingLimit.MinBet;
                    bettingLimit.MinMaxBet = bettingLimit.MaxBet;
                    bettingLimit.MinMaxPerMatch = bettingLimit.MaxPerMatch;
                    bettingLimit.MinMaxPerBall = bettingLimit.MaxPerBall;
                }

                bettingLimits.Add(bettingLimit);
            }

            return bettingLimits.OrderBy(bt => bt.SportId).OrderBy(bt => bt.BetTypeId).ToList();
        }

        /// <summary>
        /// Map betting limit data.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Betting limit.</returns>
        private static BettingLimitItem MapBettingLimitData(IDataReader reader)
        {
            var betLimitItem = new BettingLimitItem();
            var numberItem = reader.FieldCount;
            var properties = betLimitItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            for (var index = 0; index < numberItem; index++)
            {
                var property = GetPropertyInfo(properties, reader.GetName(index));
                if (property != null)
                {
                    property.SetValue(betLimitItem, reader.GetValue(index), null);
                }
            }

            return betLimitItem;
        }

        /// <summary>
        /// Get property information.
        /// </summary>
        /// <param name="properties">Property information list.</param>
        /// <param name="fieldName">Field name for match property name.</param>
        /// <returns>Property information.</returns>
        private static PropertyInfo GetPropertyInfo(PropertyInfo[] properties, string fieldName)
        {
            PropertyInfo propertyInfo = null;

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var attribute = (SpParamAttribute)property.GetCustomAttributes(typeof(SpParamAttribute), false).FirstOrDefault();

                if (attribute != null)
                {
                    propertyName = attribute.SqlParamName;
                }

                if (propertyName.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase))
                {
                    propertyInfo = property;
                    break;
                }
            }

            return propertyInfo;
        }
    }
}