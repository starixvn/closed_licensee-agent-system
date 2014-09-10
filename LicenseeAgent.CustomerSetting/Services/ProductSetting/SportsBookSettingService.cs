namespace LicenseeAgent.CustomerSetting.Services.ProductSetting
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Exceptions;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Criteria.SportsBook;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Services;
    using LicenseeAgent.CustomerSetting.Utils;
    using LicenseeAgent.CustomerSetting.ViewModels;

    /// <summary>
    /// Customer repository.
    /// </summary>
    public class SportsBookSettingService : SettingServiceBase, ISportsBookSettingService
    {
        public Product ProductId
        {
            get
            {
                return Product.Product_0;
            }
        }

        public string Name
        {
            get
            {
                return "sportsBookSetting";
            }
        }

        /// <summary>
        /// Get Agent setting for insert new.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>
        /// Sports book customer setting.
        /// </returns>
        public CustomerSetting2 GetSettingForInsert(SettingContext settingContext, out int errCode)
        {
            CustomerSetting customerSetting;
            errCode = Result.FailureValue;
            var maxLicenseePt = 0;
            GetSettingForInsert(
                settingContext,
                out customerSetting,
                out errCode,
                out maxLicenseePt);

            return BuildCustomerSetting2(ProductId, customerSetting, settingContext, ref errCode, maxLicenseePt);
        }

        /// <summary>
        /// Get Agent setting for updating.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>
        /// Sports book customer setting.
        /// </returns>
        public CustomerSetting2 GetSettingForUpdate(SettingContext settingContext, out int errCode)
        {
            CustomerSetting customerSetting;
            errCode = Result.FailureValue;
            GetSettingForUpdate(
                settingContext,
                out customerSetting,
                out errCode);

            return BuildCustomerSetting2(ProductId, customerSetting, settingContext, ref errCode);
        }

        /// <summary>
        /// Insert customer setting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Status of insert command.
        /// </returns>
        public InsertResult InsertCustomer(InsertCustomerViewModel model)
        {
            var criteria = CreateInsertCustomerCriteria(model);
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
                    return new InsertResult
                    {
                        ErrCode = Converter.ToInt(parameters["msgid"]),
                        MinPT = Converter.To<short>(parameters["minptoutput"]),
                        MaxPT = Converter.To<short>(parameters["maxptoutput"]),
                        MaxCredit = Converter.ToDecimal(parameters["maxcreditoutput"]),
                        BetLimit = Converter.ToDecimal(parameters["betlimitoutput"]),
                        SportType = Converter.ToInt(parameters["betlimitsporttype"]) == 0
                            ? Converter.ToInt(parameters["ptsporttype"])
                            : Converter.ToInt(parameters["betlimitsporttype"]),
                        BetType = Converter.ToInt(parameters["betlimitbettype"]) == 0
                            ? Converter.ToInt(parameters["ptbettype"])
                            : Converter.ToInt(parameters["betlimitbettype"]),
                        IsLive = Converter.ToBool(parameters["isliveoutput"]),
                        CustIdReturn = parameters.ContainsKey("agentid") ? Converter.ToInt(parameters["agentid"]) : Converter.ToInt(parameters["custid"]),
                        IBCUserName = parameters.ContainsKey("agentid") ? string.Empty : Converter.ToString2(parameters["ibcusername"])
                    };
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Insert customer setting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Status of insert command.
        /// </returns>
        public IList<UpdateResult> UpdateCustomer(UpdateSportsBookCustomerViewModel model)
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
        /// Insert customer setting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Status of insert command.
        /// </returns>
        public IList<UpdateResult> UpdateIndirectCustomer(UpdateSportsBookCustomerViewModel model)
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
        /// Creates the update customer criteria.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns criteria.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private ICriteria CreateUpdateCustomerCriteria(UpdateSportsBookCustomerViewModel model)
        {
            var discountXml = string.Empty;
            var betLimitXml = string.Empty;
            var positionTakingXml = string.Empty;
            var discount3RDXml = string.Empty;
            var betLimit3RDXml = string.Empty;
            var positionTaking3RDXml = string.Empty;
            if (model.SportsBookSetting != null)
            {
                positionTakingXml = model.SportsBookSetting.PositionTakingSetting != null
                                            ? XmlBuilder.Build(model.SettingContext, model.SportsBookSetting.PositionTakingSetting)
                                            : string.Empty;

                if (model.SettingContext.FormId != FormType.EditIndirectMember)
                {
                    discountXml = model.SportsBookSetting.CommissionSetting != null
                                    ? XmlBuilder.Build(model.SettingContext, model.SportsBookSetting.CommissionSetting)
                                    : string.Empty;
                    betLimitXml = model.SportsBookSetting.BettingLimitSetting != null
                                    ? XmlBuilder.Build(model.SportsBookSetting.BettingLimitSetting)
                                    : string.Empty;
                }
            }

            if (model.NumberGameSetting != null)
            {
                positionTaking3RDXml = XmlBuilder.Build(model.SettingContext, model.NumberGameSetting.PositionTakingSetting);

                if (model.SettingContext.FormId != FormType.EditIndirectMember)
                {
                    discount3RDXml = model.NumberGameSetting.CommissionSetting != null
                                        ? XmlBuilder.Build(model.SettingContext, model.NumberGameSetting.CommissionSetting)
                                        : string.Empty;
                    betLimit3RDXml = model.NumberGameSetting.BettingLimitSetting != null
                                        ? XmlBuilder.BuildForNumberGame(model.NumberGameSetting.BettingLimitSetting)
                                        : string.Empty;
                }
            }

            var baseCriteria = new UpdateCustomerCriteria
            {
                Discount = discountXml,
                BetLimit = betLimitXml,
                PositionTaking = positionTakingXml,
                Discount3RD = discount3RDXml,
                BetLimit3RD = betLimit3RDXml,
                PositionTaking3RD = positionTaking3RDXml,
                Ip = model.SettingContext.Ip,
                Updater = model.SettingContext.LoginUserId
            };

            if (model.SettingContext.TargetRoleId == (int)Role.Member)
            {
                if (model.SettingContext.FormId == FormType.EditIndirectMember)
                {
                    return new AgentEditMemberIndirectCriteria
                    {
                        Updater = model.SettingContext.LoginUserId,
                        AgentId = model.SettingContext.CustId,
                        CustId = model.SettingContext.TargetCustId,
                        PositionTaking = baseCriteria.PositionTaking,
                        PositionTaking3RD = baseCriteria.PositionTaking3RD,
                        Ip = baseCriteria.Ip
                    };
                }
                else
                {
                    return new AgentUpdateMemberCriteria(baseCriteria)
                    {
                        CustId = model.SettingContext.TargetCustId
                    };
                }
            }
            else
            {
                if (model.SettingContext.TargetRoleId == (int)Role.AgentLevel1)
                {
                    return new LicenseeUpdateAgentCriteria(baseCriteria)
                    {
                        AgentId = model.SettingContext.TargetCustId
                    };
                }
                else
                {
                    return new AgentUpdateAgentCriteria(baseCriteria)
                    {
                        AgentId = model.SettingContext.TargetCustId
                    };
                }
            }
        }

        /// <summary>
        /// Get default setting for insert new agent.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="customerSetting">The customer setting.</param>
        /// <param name="errCode">The MSG unique identifier.</param>
        /// <param name="maxLicenseePt">Max PT in case insert agent level 1.</param>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void GetSettingForInsert(
            SettingContext settingContext,
            out CustomerSetting customerSetting,
            out int errCode,
            out int maxLicenseePt)
        {
            var criteria = GetCriteriaForInsert(settingContext);
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                maxLicenseePt = 0;
                var custInfo = new CustomerInfo();
                var commissionSetting = new CommissionSetting();
                IList<BettingLimitItem> bettingLimitList = null;
                IList<PositionTakingItem> positionTakingList = null;
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        commissionSetting.CommissionItems = CommissionDataReader.ReadAgentCommissionData(reader, false);
                        if (reader.NextResult())
                        {
                            bettingLimitList = BettingLimitDataReader.ReadBetLimitData(reader, false);
                        }

                        if (settingContext.TargetRoleId != (int)Role.AgentLevel1 && reader.NextResult())
                        {
                            positionTakingList = PositionTakingDataReader.ReadAgentPositionTakingData(reader, false);
                        }
                    }

                    var outputParams = objDb.GetOuputParams();
                    errCode = Converter.ToInt(outputParams["msgid"]);
                    custInfo.MaxCredit = DbConverter.ToDecimal(outputParams["maxcredit"]);
                    if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
                    {
                        maxLicenseePt = Converter.ToInt(outputParams["maxptoflicensee"]) / Constant.StepMulDiv;
                    }
                }

                customerSetting = new CustomerSetting
                {
                    CustInfo = custInfo,
                    CommissionSetting = commissionSetting,
                    BettingLimitSetting = bettingLimitList,
                    PositionTakingSetting = positionTakingList
                };
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
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private void GetSettingForUpdate(
            SettingContext settingContext,
            out CustomerSetting customerSetting,
            out int errCode)
        {
            var criteria = GetCriteriaForUpdate(settingContext);
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid criteria");
                }

                var custInfo = new CustomerInfo();
                var commissionSetting = new CommissionSetting();
                IList<BettingLimitItem> bettingLimiList = null;
                IList<PositionTakingItem> positionTakingList = null;
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        if (settingContext.TargetRoleId == (int)Role.Member)
                        {
                            var userGroup = string.Empty;
                            custInfo = CustomerInfoDataReader.ReaderMemberInfo(reader, out userGroup);
                            commissionSetting.UserGroup = userGroup;
                        }
                        else
                        {
                            custInfo = CustomerInfoDataReader.ReaderAgentInfo(reader);
                        }

                        if (settingContext.FormId != FormType.EditIndirectMember)
                        {
                            if (reader.NextResult())
                            {
                                commissionSetting.CommissionItems = settingContext.TargetRoleId == (int)Role.Member
                                    ? CommissionDataReader.ReadMemberCommissionData(reader)
                                    : CommissionDataReader.ReadAgentCommissionData(reader);
                            }

                            if (reader.NextResult())
                            {
                                bettingLimiList = BettingLimitDataReader.ReadBetLimitData(reader, true);
                            }
                        }

                        if (reader.NextResult())
                        {
                            positionTakingList = settingContext.TargetRoleId == (int)Role.Member
                                ? PositionTakingDataReader.ReadMemberPositionTakingData(reader)
                                : PositionTakingDataReader.ReadAgentPositionTakingDataForUpdate(reader, settingContext.TargetRoleId);
                        }
                    }

                    var outputParams = objDb.GetOuputParams();
                    errCode = Converter.ToInt(outputParams["msgid"]);
                    customerSetting = new CustomerSetting
                    {
                        CustInfo = custInfo,
                        CommissionSetting = commissionSetting,
                        BettingLimitSetting = bettingLimiList,
                        PositionTakingSetting = positionTakingList
                    };
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Creates the insert customer criteria.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns criteria.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private ICriteria CreateInsertCustomerCriteria(InsertCustomerViewModel model)
        {
            var discountXml = XmlBuilder.Build(model.SettingContext, model.SportsBookSetting.CommissionSetting);
            var betLimitXml = XmlBuilder.Build(model.SportsBookSetting.BettingLimitSetting);
            var positionTakingXml = XmlBuilder.Build(model.SettingContext, model.SportsBookSetting.PositionTakingSetting);
            var discount3RDXml = string.Empty;
            var betLimit3RDXml = string.Empty;
            var positionTaking3RDXml = string.Empty;
            if (model.NumberGameSetting != null)
            {
                discount3RDXml = XmlBuilder.Build(model.SettingContext, model.NumberGameSetting.CommissionSetting);
                betLimit3RDXml = XmlBuilder.BuildForNumberGame(model.NumberGameSetting.BettingLimitSetting);
                positionTaking3RDXml = XmlBuilder.Build(model.SettingContext, model.NumberGameSetting.PositionTakingSetting);
            }

            var custInfo = model.SportsBookSetting.CustInfo;
            var baseCriteria = new InsertCustomerCriteria
            {
                UserName = custInfo.UserName,
                Ip = model.SettingContext.Ip,
                Creator = model.SettingContext.LoginUserId,
                Credit = custInfo.Credit,
                Discount = discountXml,
                BetLimit = betLimitXml,
                PositionTaking = positionTakingXml,
                Discount3RD = discount3RDXml,
                BetLimit3RD = betLimit3RDXml,
                PositionTaking3RD = positionTaking3RDXml
            };

            if (model.SettingContext.TargetRoleId == (int)Role.Member)
            {
                return new AgentInsertMemberCriteria(baseCriteria)
                {
                    UGroup = model.SportsBookSetting.CommissionSetting.UserGroup,
                    ParentId = model.SettingContext.CustId
                };
            }
            else
            {
                if (model.SettingContext.TargetRoleId == (int)Role.AgentLevel1)
                {
                    var passwordSalt = Cryptography.CreateSalt();
                    var hashPassword = Cryptography.CreateHash(model.SportsBookSetting.CustInfo.Password, passwordSalt);
                    return new LicenseeInsertAgentCriteria(baseCriteria)
                    {
                        AgentL0Id = model.SettingContext.CustId,
                        UserSalt = passwordSalt,
                        UserPwd = hashPassword,
                        Phone = custInfo.Phone,
                        Mobile = custInfo.Mobile,
                        FirstName = custInfo.FirstName,
                        LastName = custInfo.LastName,
                        Fax = custInfo.Fax,
                        Status = model.SportsBookSetting.CustInfo.Status
                    };
                }
                else
                {
                    var passwordSalt = Cryptography.CreateSalt();
                    var hashPassword = Cryptography.CreateHash(model.SportsBookSetting.CustInfo.Password, passwordSalt);
                    return new AgentInsertAgentCriteria(baseCriteria)
                    {
                        ParentId = model.SettingContext.CustId,
                        UserSalt = passwordSalt,
                        UserPwd = hashPassword,
                        Phone = custInfo.Phone,
                        Mobile = custInfo.Mobile,
                        FirstName = custInfo.FirstName,
                        LastName = custInfo.LastName,
                        Fax = custInfo.Fax,
                        Status = model.SportsBookSetting.CustInfo.Status
                    };
                }
            }
        }

        /// <summary>
        /// Get Insert Setting Criteria.
        /// </summary>
        /// <param name="settingContext">Setting Context.</param>
        /// <returns>Insert Criteria.</returns>
        private ICriteria GetCriteriaForInsert(SettingContext settingContext)
        {
            if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                return new LicenseeGetAgentSettingForInsertCriteria
                {
                    AgentL0Id = settingContext.CustId,
                };
            }

            return new AgentGetAgentSettingForInsertCriteria
            {
                AgentId = settingContext.CustId
            };
        }

        /// <summary>
        /// Initializes the criteria for getting SportsBook setting for update.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Criteria for getting SportsBook setting.</returns>
        private ICriteria GetCriteriaForUpdate(SettingContext settingContext)
        {
            ICriteria criteria = null;
            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                if (settingContext.FormId == FormType.EditIndirectMember)
                {
                    criteria = new AgentGetMemberSettingIndirectForUpdateCriteria
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
            else if (settingContext.TargetRoleId == (int)Role.Agent)
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
    }
}