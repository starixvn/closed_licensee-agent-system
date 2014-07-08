namespace LicenseeAgent.CustomerSetting.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common.Extensions;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class CommissionDataReader
    {
        /// <summary>
        /// Read agent commission.
        /// </summary>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="getUplineCom">Get up line commission or not.</param>
        /// <returns>Returns commission.</returns>
        public static IList<CommissionItem> ReadAgentCommissionData(
            IDataReader reader,
            bool getUplineCom = true)
        {
            var commissionItems = new List<CommissionItem>();
            while (reader.Read())
            {
                var commissionItem = new CommissionItem
                {
                    SportId = reader.ToInt("SportType"),
                    BetTypeId = reader.ToInt("BetType"),
                    DiscountA = Math.Round((reader.ToDecimal("DiscountA") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                    DiscountB = Math.Round((reader.ToDecimal("DiscountB") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                    DiscountC = Math.Round((reader.ToDecimal("DiscountC") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                    DiscountD = Math.Round((reader.ToDecimal("DiscountD") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist)
                };

                if (getUplineCom)
                {
                    commissionItem.Upline = new CommissionItem
                    {
                        DiscountA = Math.Round((reader.ToDecimal("UplineMinDiscountA") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                        DiscountB = Math.Round((reader.ToDecimal("UplineMinDiscountB") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                        DiscountC = Math.Round((reader.ToDecimal("UplineMinDiscountC") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                        DiscountD = Math.Round((reader.ToDecimal("UplineMinDiscountD") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist)
                    };
                }
                else
                {
                    commissionItem.Upline = new CommissionItem
                    {
                        DiscountA = commissionItem.DiscountA,
                        DiscountB = commissionItem.DiscountB,
                        DiscountC = commissionItem.DiscountC,
                        DiscountD = commissionItem.DiscountD,
                    };
                }

                commissionItems.Add(commissionItem);
            }

            return commissionItems;
        }

        /// <summary>
        /// Read agent commission.
        /// </summary>
        /// <param name="reader">Reader for read data.</param>
        /// <param name="getUplineCom">Get up line commission or not.</param>
        /// <returns>Returns commission.</returns>
        public static IList<CommissionItem> ReadNumberGameAgentCommissionData(
            IDataReader reader,
            bool getUplineCom = true)
        {
            var commissionItems = new List<CommissionItem>();
            while (reader.Read())
            {
                var commissionItem = new CommissionItem
                {
                    SportId = reader.ToInt("SportType"),
                    BetTypeId = reader.ToInt("BetType"),
                    DiscountA = Math.Round((reader.ToDecimal("DiscountA") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist)
                };

                if (getUplineCom)
                {
                    commissionItem.Upline = new CommissionItem
                    {
                        DiscountA = Math.Round((reader.ToDecimal("UplineMinDiscountA") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                    };
                }
                else
                {
                    commissionItem.Upline = new CommissionItem
                    {
                        DiscountA = commissionItem.DiscountA,
                    };
                }

                commissionItems.Add(commissionItem);
            }

            return commissionItems;
        }

        /// <summary>
        /// Read member commission setting data.
        /// </summary>
        /// <param name="reader">Reader for reading data.</param>
        /// <returns>Returns commission.</returns>
        public static IList<CommissionItem> ReadNumberGameMemberCommissionData(IDataReader reader)
        {
            var commissionItems = new List<CommissionItem>();
            while (reader.Read())
            {
                commissionItems.Add(new CommissionItem
                {
                    SportId = reader.ToInt("SportType"),
                    BetTypeId = reader.ToInt("BetType"),
                    Discount = Math.Round((reader.ToDecimal("Discount") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                    Upline = new CommissionItem
                    {
                        DiscountA = Math.Round((reader.ToDecimal("UplineMinDiscountA") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                    }
                });
            }

            return commissionItems;
        }

        /// <summary>
        /// Read member commission setting data.
        /// </summary>
        /// <param name="reader">Reader for reading data.</param>
        /// <returns>Returns commission.</returns>
        public static IList<CommissionItem> ReadMemberCommissionData(IDataReader reader)
        {
            var commissionItems = new List<CommissionItem>();
            while (reader.Read())
            {
                commissionItems.Add(new CommissionItem
                {
                    SportId = reader.ToInt("SportType"),
                    BetTypeId = reader.ToInt("BetType"),
                    Discount = Math.Round((reader.ToDecimal("Discount") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                    Upline = new CommissionItem
                    {
                        DiscountA = Math.Round((reader.ToDecimal("UplineMinDiscountA") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                        DiscountB = Math.Round((reader.ToDecimal("UplineMinDiscountB") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                        DiscountC = Math.Round((reader.ToDecimal("UplineMinDiscountC") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist),
                        DiscountD = Math.Round((reader.ToDecimal("UplineMinDiscountD") * Constant.StepMulDiv) - Constant.RoundingNumber, Constant.NumberDigist)
                    }
                });
            }

            return commissionItems;
        }
    }
}