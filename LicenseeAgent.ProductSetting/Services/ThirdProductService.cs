namespace LicenseeAgent.ProductSetting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Exceptions;
    using LicenseeAgent.ProductSetting.Entities;
    using LicenseeAgent.ProductSetting.Services.ProductSetting;
    using LicenseeAgent.ProductSetting.Utils;

    /// <summary>
    /// ThirdProductService class.
    /// </summary>
    public abstract class ThirdProductService : SettingServiceBase
    {
        private const int Keno = 1500;

        public abstract ProductCode ProductId { get; }

        public abstract string Name { get; }

        public abstract ICriteria GetCriteriaForUpdateAgent(SettingContext settingContext);

        public abstract ICriteria GetCriteriaForUpdateMember(SettingContext settingContext);

        public CustomerSetting2 GetSettingForInsert(SettingContext settingContext, Result result)
        {
            return null;
        }

        /// <summary>
        /// Get customer setting for updating.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <param name="result">The error code.</param>
        /// <returns>
        /// Virtual Sports customer setting.
        /// </returns>
        public CustomerSetting2 GetSettingForUpdate(SettingContext settingContext, Result result)
        {
            var customerSetting = new CustomerSetting();
            result.ErrCode = Result.FailureValue;

            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                GetSettingForUpdateMember(settingContext, customerSetting, result);
            }
            else
            {
                GetSettingForUpdateAgent(settingContext, customerSetting, result);
            }

            return BuildCustomerSetting2(ProductId, customerSetting, settingContext, result);
        }

        /// <summary>
        /// Get default setting for edit customer.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="customerSetting">The customer setting.</param>
        /// <param name="result">The MSG unique identifier.</param>
        public void GetSettingForUpdateAgent(
            SettingContext settingContext,
            CustomerSetting customerSetting,
            Result result)
        {
            var criteria = GetCriteriaForUpdateAgent(settingContext);
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                var custInfo = new CustomerInfo();
                IList<BettingLimitItem> bettingLimiList = null;
                IList<PositionTakingItem> positionTakingList = null;
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        custInfo = CustomerInfoDataReader.ReaderAgentInfo(reader);

                        if (settingContext.FormId != FormType.EditIndirectMember)
                        {
                            if (reader.NextResult())
                            {
                                bettingLimiList = BettingLimitDataReader.ReadBetLimitData(reader, true);
                                bettingLimiList = bettingLimiList.Select(b => UpdateBettingLimit(b, settingContext.ProductId)).ToList();
                            }
                        }

                        if (reader.NextResult())
                        {
                            positionTakingList = PositionTakingDataReader.ReadAgentPositionTakingDataForUpdate(reader, settingContext.TargetRoleId);
                        }
                    }

                    var outputParams = objDb.GetOuputParams();
                    result.ErrCode = Converter.ToInt(outputParams["msgid"]);
                    SetCustomerSetting(customerSetting, custInfo, bettingLimiList, positionTakingList);
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Get default setting for edit member.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="customerSetting">The customer setting.</param>
        /// <param name="result">The MSG unique identifier.</param>
        public void GetSettingForUpdateMember(
            SettingContext settingContext,
            CustomerSetting customerSetting,
            Result result)
        {
            var criteria = GetCriteriaForUpdateMember(settingContext);
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                var custInfo = new CustomerInfo();
                IList<BettingLimitItem> bettingLimiList = null;
                IList<PositionTakingItem> positionTakingList = null;
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        string userGroup;
                        custInfo = CustomerInfoDataReader.ReaderMemberInfo(reader, out userGroup);

                        if (settingContext.FormId != FormType.EditIndirectMember)
                        {
                            if (reader.NextResult())
                            {
                                bettingLimiList = BettingLimitDataReader.ReadBetLimitData(reader, true);
                            }
                        }

                        if (reader.NextResult())
                        {
                            positionTakingList = PositionTakingDataReader.ReadMemberPositionTakingData(reader);
                        }
                    }

                    var outputParams = objDb.GetOuputParams();
                    result.ErrCode = Converter.ToInt(outputParams["msgid"]);
                    SetCustomerSetting(customerSetting, custInfo, bettingLimiList, positionTakingList);
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Create Customer setting Virtual Sports  for update.
        /// </summary>
        /// <param name="customerSetting">Customer setting.</param>
        /// <param name="custInfo">The customer info.</param>
        /// <param name="bettingLimiList">The betting limit info.</param>
        /// <param name="positionTakingList">The position taking info.</param>
        private void SetCustomerSetting(
            CustomerSetting customerSetting, 
            CustomerInfo custInfo, 
            IList<BettingLimitItem> bettingLimiList, 
            IList<PositionTakingItem> positionTakingList)
        {
            customerSetting.CustInfo = custInfo;
            customerSetting.BettingLimitSetting = bettingLimiList;
            customerSetting.PositionTakingSetting = positionTakingList;
        }

        /// <summary>
        /// Update betting limit.
        /// </summary>
        /// <param name="bettingLimit">Betting limit.</param>
        /// <param name="product">Product code.</param>
        /// <returns>Betting limit item.</returns>
        private BettingLimitItem UpdateBettingLimit(BettingLimitItem bettingLimit, int product)
        {
            switch (product)
            {
                case (int)ProductCode.Product_8:
                    bettingLimit.SportId = (int)SportType.SportType_202;
                    bettingLimit.BetTypeId = bettingLimit.GameTypeId + Keno;
                    break;
            }

            return bettingLimit;
        }
    }
}
