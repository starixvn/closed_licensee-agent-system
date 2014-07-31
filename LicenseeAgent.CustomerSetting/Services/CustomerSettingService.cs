namespace LicenseeAgent.CustomerSetting.Services
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
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerSettingService : QueryService, ICustomerSettingService
    {
        /// <summary>
        /// Check user name.
        /// </summary>
        /// <param name="criteria">Check exist username criteria.</param>
        public void CheckExistUserName(CheckExistUserNameCriteria criteria)
        {
            Execute(criteria);
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
        public IList<UpdateResult> UpdateCustomer(UpdateCustomerViewModel model)
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
        public IList<UpdateResult> UpdateIndirectCustomer(UpdateCustomerViewModel model)
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

        public void MemberIsFirstDeposit(MemberIsFirstDepositCriteria criteria)
        {
            QueryService.Execute(criteria);
        }

        public void MemberReArrangement(ReArrangementMemberCriteria criteria)
        {
            QueryService.Execute(criteria);
        }

        public void SwitchUserFromNormalToLegend(SwitchUserFromNormalToLegendCriteria criteria)
        {
            QueryService.Execute(criteria);
        }

        public void GetLegendAgentId(GetLegendAgentIdCriteria criteria)
        {
            QueryService.Execute(criteria);
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
        /// Creates the update customer criteria.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns criteria.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private ICriteria CreateUpdateCustomerCriteria(UpdateCustomerViewModel model)
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
    }
}