namespace LicenseeAgent.CustomerSetting.Services.VirtualSports
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Exceptions;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CustomerSetting.Criteria.VirtualSports;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Utils;
    using LicenseeAgent.CustomerSetting.ViewModels;

    /// <summary>
    /// Customer repository.
    /// </summary>
    public class SettingService : SettingServiceBase, ISettingService
    {
        public Product ProductId
        {
            get
            {
                return Product.Product_5;
            }
        }

        public string Name
        {
            get
            {
                return "virtualSportsSetting";
            }
        }

        public CustomerSetting2 GetSettingForInsert(SettingContext settingContext, out int errCode)
        {
            errCode = Result.FailureValue;
            return null;
        }

        /// <summary>
        /// Get customer setting for updating.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>
        /// Virtual Sports customer setting.
        /// </returns>
        public CustomerSetting2 GetSettingForUpdate(SettingContext settingContext, out int errCode)
        {
            CustomerSetting customerSetting;
            errCode = Result.FailureValue;
            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                GetSettingForUpdateMember(settingContext, out customerSetting, out errCode);
            }
            else
            {
                GetSettingForUpdateAgent(settingContext, out customerSetting, out errCode);
            }

            return BuildCustomerSetting2(ProductId, customerSetting, settingContext, ref errCode);
        }

        /// <summary>
        /// Insert customer setting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Status of insert command.
        /// </returns>
        public IList<UpdateResult> UpdateCustomer(UpdateVirtualSportsCustomerViewModel model)
        {
            var criteria = CreateUpdateCustomerCriteria(model);
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var parameters = criteria.GetSpParams();
                    objDb.ExecuteNonQuery(parameters);

                    return ObjectSerializer.DeserializeList<UpdateResult>(Convert.ToString(parameters["settingoutput"]));
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Get default setting for edit customer.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="customerSetting">The customer setting.</param>
        /// <param name="errCode">The MSG unique identifier.</param>
        private void GetSettingForUpdateAgent(
            SettingContext settingContext,
            out CustomerSetting customerSetting,
            out int errCode)
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
                            }
                        }

                        if (reader.NextResult())
                        {
                            positionTakingList = PositionTakingDataReader.ReadAgentPositionTakingDataForUpdate(reader, settingContext.TargetRoleId);
                        }
                    }

                    var outputParams = objDb.GetOuputParams();
                    errCode = Converter.ToInt(outputParams["msgid"]);
                    customerSetting = CreateCustomerSetting(custInfo, bettingLimiList, positionTakingList);
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
        /// <param name="errCode">The MSG unique identifier.</param>
        private void GetSettingForUpdateMember(
            SettingContext settingContext,
            out CustomerSetting customerSetting,
            out int errCode)
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
                    errCode = Converter.ToInt(outputParams["msgid"]);
                    customerSetting = CreateCustomerSetting(custInfo, bettingLimiList, positionTakingList);
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Initializes the criteria for getting Virtual Sports setting for update.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Criteria for getting Virtual Sports setting.</returns>
        private ICriteria GetCriteriaForUpdateAgent(SettingContext settingContext)
        {
            ICriteria criteria = null;

            if (settingContext.TargetRoleId == (int)Role.Agent)
            {
                criteria = new AgentGetAgentSettingForUpdateCriteria
                {
                    AgentId = settingContext.TargetCustId
                };
            }
            else if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                criteria = new LicenseeGetAgentSettingForUpdateCriteria
                {
                    AgentId = settingContext.TargetCustId,
                };
            }

            return criteria;
        }

        /// <summary>
        /// Initializes the criteria for getting Virtual Sports setting for update.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Criteria for getting Virtual Sports setting.</returns>
        private ICriteria GetCriteriaForUpdateMember(SettingContext settingContext)
        {
            ICriteria criteria = null;

            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                if (settingContext.FormId == FormType.EditIndirectMember)
                {
                    criteria = new AgentGetMemberIndirectSettingForUpdateCriteria
                    {
                        AgentId = settingContext.CustId,
                        CustId = settingContext.TargetCustId
                    };
                }
                else
                {
                    criteria = new AgentGetMemberSettingForUpdateCriteria
                    {
                        CustId = settingContext.TargetCustId
                    };
                }
            }

            return criteria;
        }

        /// <summary>
        /// Create Customer setting Virtual Sports  for update.
        /// </summary>
        /// <param name="custInfo">The customer info.</param>
        /// <param name="bettingLimiList">The betting limit info.</param>
        /// <param name="positionTakingList">The position taking info.</param>
        /// <returns>Customer Setting Virtual Sports.</returns>
        private CustomerSetting CreateCustomerSetting(
            CustomerInfo custInfo,
            IList<BettingLimitItem> bettingLimiList,
            IList<PositionTakingItem> positionTakingList)
        {
            return new CustomerSetting
            {
                CustInfo = custInfo,
                BettingLimitSetting = bettingLimiList,
                PositionTakingSetting = positionTakingList
            };
        }

        /// <summary>
        /// Creates the update customer criteria.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns criteria.</returns>
        private ICriteria CreateUpdateCustomerCriteria(UpdateVirtualSportsCustomerViewModel model)
        {
            var betLimit3RDXml = string.Empty;
            var positionTaking3RDXml = string.Empty;
            positionTaking3RDXml = XmlBuilder.Build(model.SettingContext, model.VirtualSportsSetting.PositionTakingSetting);

            if (model.SettingContext.FormId != FormType.EditIndirectMember)
            {
                betLimit3RDXml = XmlBuilder.Build(model.VirtualSportsSetting.BettingLimitSetting);
            }

            if (model.SettingContext.TargetRoleId == (int)Role.Member)
            {
                return CreateUpdateMemberCriteria(model, positionTaking3RDXml, betLimit3RDXml);
            }
            else
            {
                if (model.SettingContext.TargetRoleId == (int)Role.AgentLevel1)
                {
                    return new LicenseeUpdateAgentCriteria()
                    {
                        AgentId = model.SettingContext.TargetCustId,
                        BetLimit3RD = betLimit3RDXml,
                        PositionTaking3RD = positionTaking3RDXml,
                        Ip = model.SettingContext.Ip,
                        Updater = model.SettingContext.LoginUserId
                    };
                }
                else
                {
                    return new AgentUpdateAgentCriteria()
                    {
                        AgentId = model.SettingContext.TargetCustId,
                        BetLimit3RD = betLimit3RDXml,
                        PositionTaking3RD = positionTaking3RDXml,
                        Ip = model.SettingContext.Ip,
                        Updater = model.SettingContext.LoginUserId
                    };
                }
            }
        }

        /// <summary>
        /// Creates the update customer criteria.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="positionTaking">The position taking 3RD.</param>
        /// <param name="betLimit">The betLimit 3RD.</param>
        /// <returns>Returns criteria.</returns>
        private ICriteria CreateUpdateMemberCriteria(UpdateVirtualSportsCustomerViewModel model, string positionTaking, string betLimit)
        {
            if (model.SettingContext.FormId == FormType.EditIndirectMember)
            {
                return new AgentUpdateMemberIndirectCriteria
                {
                    AgentId = model.SettingContext.CustId,
                    CustId = model.SettingContext.TargetCustId,
                    PositionTaking3RD = positionTaking,
                    Ip = model.SettingContext.Ip,
                    Updater = model.SettingContext.LoginUserId
                };
            }
            else
            {
                return new AgentUpdateMemberCriteria
                {
                    AgentId = model.SettingContext.CustId,
                    CustId = model.SettingContext.TargetCustId,
                    PositionTaking3RD = positionTaking,
                    BetLimit3RD = betLimit,
                    Ip = model.SettingContext.Ip,
                    Updater = model.SettingContext.LoginUserId
                };
            }
        }
    }
}