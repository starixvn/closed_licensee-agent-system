// --------------------------------------------------------------------------
// <copyright file="SportsbookSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
//   <change who="Ricky.Nguyen" date="2013.3.25">Update PT New Rule</change>
//   <change who="Ricky.Nguyen" date="2013.4.12">Update New Rules</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using Fanex.Common;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Criteria.Sportsbook;
    using LicenseeAgent.CustomerSetting.Criteria.Sportsbook.Agent;
    using LicenseeAgent.CustomerSetting.Criteria.Sportsbook.Licensee;
    using LicenseeAgent.CustomerSetting.Criteria.Sportsbook.Member;
    using LicenseeAgent.CustomerSetting.Criterias;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Entities.Sportsbook;
    using LicenseeAgent.CustomerSetting.Repositories;
    using LicenseeAgent.CustomerSetting.Services;
    using LicenseeAgent.CustomerSetting.ViewModels;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Mvc;
    using Newtonsoft.Json;

    /// <summary>
    /// Customer Service.
    /// </summary>
    public class SportsbookSettingService : CustomerSettingServiceBase, ISportsbookSettingService
    {
        private readonly ISportsbookSettingRepository _customerSettingRepository;

        public SportsbookSettingService(ISportsbookSettingRepository customerSettingRepository)
        {
            _customerSettingRepository = customerSettingRepository;
        }

        public void CheckExistUserName(CheckExistUserNameCriteria criteria)
        {
            Execute(criteria);
        }

        #region Add Customer Setting
        /// <summary>
        /// Get Agent setting for insert new.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <returns>Sports book customer setting.</returns>
        public SportsbookCustomer GetSportsbookSettingForInsert(SettingContext settingContext)
        {
            CustomerInfo custInfo = null;
            CommissionSetting commissionSetting = null;
            IList<BettingLimit> bettingLimits = null;
            Dictionary<string, PositionTaking> positionTakingData = null;
            ICriteria getAgentSettingCriteria = null;
            if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                getAgentSettingCriteria = new LicenseeGetAgentSettingForInsertCriteria
                {
                    AgentL0Id = settingContext.CustId,
                };
            }
            else
            {
                getAgentSettingCriteria = new AgentGetAgentSettingForInsertCriteria
                {
                    AgentId = settingContext.CustId
                };
            }

            var msgId = 0;
            var maxLicenseePt = 0;
            _customerSettingRepository.GetSettingForInsert(
                getAgentSettingCriteria,
                settingContext,
                out custInfo,
                out commissionSetting,
                out bettingLimits,
                out positionTakingData,
                out msgId,
                out maxLicenseePt);

            if (msgId == 0)
            {
                return new SportsbookCustomer
                {
                    CustInfo = custInfo,
                    CommissionSetting = commissionSetting,
                    BettingLimits = bettingLimits,
                    PositionTakingSetting = BuildPositionTakingGroups(settingContext, positionTakingData, maxLicenseePt)
                };
            }

            return new SportsbookCustomer
            {
                Result = new Result(msgId, ResourceManager.GetErrMsg(msgId))
            };
        }

        /// <summary>
        /// Get Agent setting for updating.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <returns>
        /// Sports book customer setting.
        /// </returns>
        public SportsbookCustomer GetSportsbookSettingForUpdate(SettingContext settingContext)
        {
            CustomerInfo custInfo = null;
            CommissionSetting commissionSetting = null;
            IList<BettingLimit> bettingLimits = null;
            Dictionary<string, PositionTaking> positionTakingData = null;
            ICriteria getAgentSettingCriteria = this.InitializeCriteriaForGettingSportsbookSettingForUpdate(settingContext);

            var msgId = 0;
            _customerSettingRepository.GetSettingForUpdate(
                getAgentSettingCriteria,
                settingContext,
                out custInfo,
                out commissionSetting,
                out bettingLimits,
                out positionTakingData,
                out msgId);

            if (msgId == 0)
            {
                return new SportsbookCustomer
                {
                    CustInfo = custInfo,
                    CommissionSetting = commissionSetting,
                    BettingLimits = bettingLimits,
                    PositionTakingSetting = BuildPositionTakingGroupsForUpdate(settingContext, positionTakingData)
                };
            }

            return new SportsbookCustomer
            {
                Result = new Result(msgId, ResourceManager.GetErrMsg(msgId))
            };
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
            //// Build XML setting for insert new customer.
            var discountXmlStringData = BuildCommissionSettingXML(model.SettingContext, model.UpdateDataSportsbookCustomer.CommissionSetting);
            var betLimitXmlStringData = BuildBetLimitSettingXML(model.SettingContext, model.UpdateDataSportsbookCustomer.BettingLimits);
            var positionTakingXmlStringData = BuildPositionTakingSettingXML(
                model.SettingContext, model.UpdateDataSportsbookCustomer.PositionTakingSetting);
            var discountXML = discountXmlStringData != string.Empty
                ? string.Format(Constant.FORMAT_XML_ROOT, discountXmlStringData)
                : string.Empty;
            var betLimitXML = betLimitXmlStringData != string.Empty
                ? string.Format(Constant.FORMAT_XML_ROOT, betLimitXmlStringData)
                : string.Empty;
            var positionTakingXML = positionTakingXmlStringData != string.Empty
                ? string.Format(Constant.FORMAT_XML_ROOT, positionTakingXmlStringData)
                : string.Empty;

            try
            {
                return InsertCustomerData(
                    model,
                    discountXML,
                    betLimitXML,
                    positionTakingXML);
            }
            catch (Exception ex)
            {
                var errorMessage = BuildLogErrorDetail(
                    ex.Message,
                    model.SettingContext.TargetRoleId != (int)Role.Member,
                    model.SettingContext.CustId,
                    model.SettingContext.FormId == FormType.EditMultiple ? model.SettingContext.TargetCustIds : model.SettingContext.TargetCustId.ToString(),
                    model.SettingContext.LoginUserId,
                    discountXML,
                    betLimitXML,
                    positionTakingXML);
                HandleException(errorMessage, ex);
            }

            return new InsertResult
            {
                ErrCode = Constant.ERROR_CODE_DEFAULT
            };
        }

        /// <summary>
        /// Insert customer setting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Status of insert command.
        /// </returns>
        public InsertResult UpdateCustomer(UpdateCustomerViewModel model)
        {
            // Build XML setting for update customer.
            var discountXmlStringData = this.BuildCommissionSettingXML(model.SettingContext, model.UpdateDataSportsbookCustomer.CommissionSetting);
            var betLimitXmlStringData = this.BuildBetLimitSettingXML(model.SettingContext, model.UpdateDataSportsbookCustomer.BettingLimits);
            var positionTakingXmlStringData = this.BuildPositionTakingSettingXML(
                model.SettingContext, model.UpdateDataSportsbookCustomer.PositionTakingSetting);
            var discountXML = discountXmlStringData != string.Empty
                ? string.Format(Constant.FORMAT_XML_ROOT, discountXmlStringData)
                : string.Empty;
            var betLimitXML = betLimitXmlStringData != string.Empty
                ? string.Format(Constant.FORMAT_XML_ROOT, betLimitXmlStringData)
                : string.Empty;
            var positionTakingXML = positionTakingXmlStringData != string.Empty
                ? string.Format(Constant.FORMAT_XML_ROOT, positionTakingXmlStringData)
                : string.Empty;

            try
            {
                return UpdateCustomerData(
                    model,
                    discountXML,
                    betLimitXML,
                    positionTakingXML);
            }
            catch (Exception ex)
            {
                var errorMessage = BuildLogErrorDetail(
                    ex.Message,
                    model.SettingContext.TargetRoleId != (int)Role.Member,
                    model.SettingContext.CustId,
                    model.SettingContext.FormId == FormType.EditMultiple ? model.SettingContext.TargetCustIds : model.SettingContext.TargetCustId.ToString(),
                    model.SettingContext.LoginUserId,
                    discountXML,
                    betLimitXML,
                    positionTakingXML);
                HandleException(errorMessage, ex);
            }

            return new InsertResult
            {
                ErrCode = Constant.ERROR_CODE_DEFAULT
            };
        }

        /// <summary>
        /// Insert customer setting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Status of insert command.
        /// </returns>
        public InsertResult UpdateIndirectCustomer(UpdateCustomerViewModel model)
        {
            // Build XML setting for update customer.
            var positionTakingXmlStringData = this.BuildPositionTakingSettingXML(
                model.SettingContext, model.UpdateDataSportsbookCustomer.PositionTakingSetting);
            var positionTakingXML = positionTakingXmlStringData != string.Empty
                ? string.Format(Constant.FORMAT_XML_ROOT, positionTakingXmlStringData)
                : string.Empty;

            try
            {
                return UpdateIndirectCustomerData(model, positionTakingXML);
            }
            catch (Exception ex)
            {
                var errorMessage = BuildLogErrorDetail(
                    ex.Message,
                    model.SettingContext.TargetRoleId != (int)Role.Member,
                    model.SettingContext.CustId,
                    model.SettingContext.FormId == FormType.EditMultiple ? model.SettingContext.TargetCustIds : model.SettingContext.TargetCustId.ToString(),
                    model.SettingContext.LoginUserId,
                    string.Empty,
                    string.Empty,
                    positionTakingXML);
                HandleException(errorMessage, ex);
            }

            return new InsertResult
            {
                ErrCode = Constant.ERROR_CODE_DEFAULT
            };
        }

        /// <summary>
        /// Format JSON return to client.
        /// </summary>
        /// <param name="settingContext">Current Setting Context.</param>
        /// <param name="sportsBookCustomer">The sports book customer.</param>
        /// <returns>
        /// JS object string.
        /// </returns>
        public string FormatJavascriptReturn(
            SettingContext settingContext,
            SportsbookCustomer sportsBookCustomer)
        {
            var result = sportsBookCustomer != null ? sportsBookCustomer.Result : null;
            var custInfo = sportsBookCustomer != null ? sportsBookCustomer.CustInfo : null;
            var commissionSetting = sportsBookCustomer != null ? sportsBookCustomer.CommissionSetting : null;
            var betLimits = sportsBookCustomer != null ? sportsBookCustomer.BettingLimits : null;
            var positionTakingSetting = sportsBookCustomer != null ? sportsBookCustomer.PositionTakingSetting : null;

            JsonSerializerSettings jsonSetting = GetJsonSetting();
            var settingScript = new StringBuilder();
            if (result != null && result.ErrCode != 0)
            {
                settingScript.Append(string.Format("var result = {0}; \n", JsonConvert.SerializeObject(result, jsonSetting)));
            }
            else
            {
                if (settingContext != null)
                {
                    settingScript.Append(string.Format("var settingContext= {0}; \n", JsonConvert.SerializeObject(settingContext, jsonSetting)));
                }

                if (custInfo != null)
                {
                    settingScript.Append(string.Format("var custInfo= {0}; \n", JsonConvert.SerializeObject(custInfo, jsonSetting)));
                }

                if (commissionSetting != null)
                {
                    settingScript.Append(string.Format("var commissionSetting= {0}; \n", JsonConvert.SerializeObject(commissionSetting, jsonSetting)));
                }

                if (betLimits != null)
                {
                    settingScript.Append(string.Format("var betLimits= {0}; \n", JsonConvert.SerializeObject(betLimits, jsonSetting)));
                }

                if (positionTakingSetting != null)
                {
                    settingScript.Append(string.Format("var positionTakingSetting= {0}; \n", JsonConvert.SerializeObject(positionTakingSetting, jsonSetting)));
                }
            }

            return settingScript.ToString();
        }

        /// <summary>
        /// Insert customer to database.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="discountXML">Discount XML data.</param>
        /// <param name="betLimitXML">Bet limit XML data.</param>
        /// <param name="positionTakingXML">Position taking setting XML data.</param>
        /// <returns>
        /// Insert status and customer id.
        /// </returns>
        private InsertResult InsertCustomerData(
            InsertCustomerViewModel model,
            string discountXML,
            string betLimitXML,
            string positionTakingXML)
        {
            var criteria = CreateInsertCustomerCriteria(model, discountXML, betLimitXML, positionTakingXML);
            return _customerSettingRepository.InsertCustomer(criteria);
        }

        /// <summary>
        /// Insert customer to database.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="discountXML">Discount XML data.</param>
        /// <param name="betLimitXML">Bet limit XML data.</param>
        /// <param name="positionTakingXML">Position taking setting XML data.</param>
        /// <returns>
        /// Insert status and customer id.
        /// </returns>
        private InsertResult UpdateCustomerData(
            UpdateCustomerViewModel model,
            string discountXML,
            string betLimitXML,
            string positionTakingXML)
        {
            var criteria = this.CreateUpdateCustomerCriteria(model, discountXML, betLimitXML, positionTakingXML);
            return _customerSettingRepository.UpdateCustomer(criteria);
        }

        /// <summary>
        /// Edit customer to database.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="positionTakingXML">Position taking setting XML data.</param>
        /// <returns>
        /// Insert status and customer id.
        /// </returns>
        private InsertResult UpdateIndirectCustomerData(
            UpdateCustomerViewModel model,
            string positionTakingXML)
        {
            var criteria = this.CreateUpdateCustomerCriteria(model, string.Empty, string.Empty, positionTakingXML);
            return _customerSettingRepository.UpdateIndirectCustomer(criteria);
        }

        /// <summary>
        /// Creates the insert customer criteria.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="discountXML">The discount XML.</param>
        /// <param name="betLimitXML">The bet limit XML.</param>
        /// <param name="positionTakingXML">The position taking XML.</param>
        /// <returns>Returns a criteria.</returns>
        private ICriteria CreateInsertCustomerCriteria(
            InsertCustomerViewModel model,
            string discountXML,
            string betLimitXML,
            string positionTakingXML)
        {
            var custInfo = model.UpdateDataSportsbookCustomer.CustInfo;
            var criteriaBase = new InsertCustomerCriteria
            {
                UserName = custInfo.UserName,
                Ip = model.SettingContext.Ip,
                Creator = model.SettingContext.LoginUserId,
                Credit = custInfo.Credit,
                Status = custInfo.Status,
                Discount = discountXML,
                BetLimit = betLimitXML,
                PositionTaking = positionTakingXML
            };

            if (model.SettingContext.TargetRoleId == (int)Role.Member)
            {
                return CreateAgentInsertMemberCriteria(criteriaBase, model);
            }
            else
            {
                if (model.SettingContext.TargetRoleId == (int)Role.AgentLevel1)
                {
                    return CreateLicenseeInsertAgentCriteria(criteriaBase, model);
                }
                else
                {
                    return CreateAgentInsertAgentCriteria(criteriaBase, model);
                }
            }
        }

        /// <summary>
        /// Creates the insert customer criteria.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="discountXML">The discount XML.</param>
        /// <param name="betLimitXML">The bet limit XML.</param>
        /// <param name="positionTakingXML">The position taking XML.</param>
        /// <returns>Returns a criteria.</returns>
        private ICriteria CreateUpdateCustomerCriteria(
            UpdateCustomerViewModel model,
            string discountXML,
            string betLimitXML,
            string positionTakingXML)
        {
            var custInfo = model.UpdateDataSportsbookCustomer.CustInfo;
            var criteriaBase = new UpdateCustomerCriteria
            {
                Discount = discountXML,
                BetLimit = betLimitXML,
                PositionTaking = positionTakingXML,
                Ip = model.SettingContext.Ip,
                Updater = model.SettingContext.LoginUserId
            };

            if (model.SettingContext.TargetRoleId == (int)Role.Member)
            {
                if (model.SettingContext.IsOnlyUpdatePT)
                {
                    return this.CreateAgentUpdateIndirectMemberCriteria(criteriaBase, model);
                }
                else
                {
                    return this.CreateAgentUpdateMemberCriteria(criteriaBase, model);
                }
            }
            else
            {
                if (model.SettingContext.TargetRoleId == (int)Role.AgentLevel1)
                {
                    return this.CreateLicenseeUpdateAgentCriteria(criteriaBase, model);
                }
                else
                {
                    return this.CreateAgentUpdateAgentCriteria(criteriaBase, model);
                }
            }
        }

        private ICriteria CreateAgentInsertMemberCriteria(
            InsertCustomerCriteria criteriaBase,
            InsertCustomerViewModel model)
        {
            var criteria = new AgentInsertMemberSettingCriteria(criteriaBase)
            {
                UGroup = model.UpdateDataSportsbookCustomer.CommissionSetting.UserGroup,
                ParentId = model.SettingContext.CustId
            };

            return criteria;
        }

        /// <summary>
        /// Creates the agent update member criteria.
        /// </summary>
        /// <param name="criteriaBase">The criteria base.</param>
        /// <param name="model">The model.</param>
        /// <returns>The criteria for agent update member.</returns>
        private ICriteria CreateAgentUpdateMemberCriteria(
            UpdateCustomerCriteria criteriaBase,
            UpdateCustomerViewModel model)
        {
            var criteria = new AgentUpdateMemberSettingCriteria(criteriaBase)
            {
                CustId = model.SettingContext.TargetCustId,
                UGroup = model.UpdateDataSportsbookCustomer.CommissionSetting.UserGroup,
            };

            return criteria;
        }

        /// <summary>
        /// Creates the agent update member criteria.
        /// </summary>
        /// <param name="criteriaBase">The criteria base.</param>
        /// <param name="model">The model.</param>
        /// <returns>The criteria for agent update member.</returns>
        private ICriteria CreateAgentUpdateIndirectMemberCriteria(
            UpdateCustomerCriteria criteriaBase,
            UpdateCustomerViewModel model)
        {
            var criteria = new AgentEditMemIndirectCriteria()
            {
                CustId = model.SettingContext.TargetCustId,
                AgentId = model.SettingContext.CustId,
                Updater = model.SettingContext.LoginUserId,
                PositionTaking = criteriaBase.PositionTaking,
                Ip = criteriaBase.Ip
            };

            return criteria;
        }

        /// <summary>
        /// Creates the licensee insert agent criteria.
        /// </summary>
        /// <param name="criteriaBase">The criteria base.</param>
        /// <param name="model">The model.</param>
        /// <returns>Returns a criteria.</returns>
        private ICriteria CreateLicenseeInsertAgentCriteria(
            InsertCustomerCriteria criteriaBase,
            InsertCustomerViewModel model)
        {
            var passwordSalt = Cryptography.CreateSalt();
            var hashPassword = Cryptography.CreateHash(model.UpdateDataSportsbookCustomer.CustInfo.Password, passwordSalt);
            var criteria = new LicenseeInsertAgentSettingCriteria(criteriaBase)
            {
                AgentL0Id = model.SettingContext.CustId,
                UserSalt = passwordSalt,
                UserPwd = hashPassword
            };

            return criteria;
        }

        /// <summary>
        /// Creates the licensee insert agent criteria.
        /// </summary>
        /// <param name="criteriaBase">The criteria base.</param>
        /// <param name="model">The model.</param>
        /// <returns>Returns a criteria.</returns>
        private ICriteria CreateLicenseeUpdateAgentCriteria(
            UpdateCustomerCriteria criteriaBase,
            UpdateCustomerViewModel model)
        {
            var criteria = new LicenseeUpdateAgentSettingCriteria(criteriaBase)
            {
                AgentId = model.SettingContext.TargetCustId
            };

            return criteria;
        }

        /// <summary>
        /// Creates the agent insert agent criteria.
        /// </summary>
        /// <param name="criteriaBase">The criteria base.</param>
        /// <param name="model">The model.</param>
        /// <returns>Returns a criteria.</returns>
        private ICriteria CreateAgentInsertAgentCriteria(
            InsertCustomerCriteria criteriaBase,
            InsertCustomerViewModel model)
        {
            var passwordSalt = Cryptography.CreateSalt();
            var hashPassword = Cryptography.CreateHash(model.UpdateDataSportsbookCustomer.CustInfo.Password, passwordSalt);
            var criteria = new AgentInsertAgentSettingCriteria(criteriaBase)
            {
                ParentId = model.SettingContext.CustId,
                UserSalt = passwordSalt,
                UserPwd = hashPassword
            };

            return criteria;
        }

        /// <summary>
        /// Creates the agent insert agent criteria.
        /// </summary>
        /// <param name="criteriaBase">The criteria base.</param>
        /// <param name="model">The model.</param>
        /// <returns>Returns a criteria.</returns>
        private ICriteria CreateAgentUpdateAgentCriteria(
            UpdateCustomerCriteria criteriaBase,
            UpdateCustomerViewModel model)
        {
            var criteria = new AgentUpdateAgentSettingCriteria(criteriaBase)
            {
                AgentId = model.SettingContext.TargetCustId
            };

            return criteria;
        }

        /// <summary>
        /// Initializes the criteria for getting SportsBook setting for update.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Criteria for getting SportsBook setting.</returns>
        private ICriteria InitializeCriteriaForGettingSportsbookSettingForUpdate(SettingContext settingContext)
        {
            ICriteria getAgentSettingCriteria = null;

            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                if (settingContext.IsOnlyUpdatePT)
                {
                    getAgentSettingCriteria = new AgentSelMemIndirectForUpdateCriteria
                    {
                        CustId = settingContext.TargetCustId,
                        AgentId = settingContext.CustId
                    };
                }
                else
                {
                    getAgentSettingCriteria = new AgentGetMemberSettingForUpdateCriteria
                    {
                        CustId = settingContext.TargetCustId
                    };
                }
            }
            else if (settingContext.TargetRoleId == (int)Role.Agent)
            {
                getAgentSettingCriteria = new AgentGetAgentSettingForUpdateCriteria
                {
                    AgentId = settingContext.TargetCustId
                };
            }
            else if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                getAgentSettingCriteria = new LicenseeGetAgentSettingForUpdateCriteria
                {
                    AgentId = settingContext.TargetCustId,
                };
            }

            return getAgentSettingCriteria;
        }

        /// <summary>
        /// Build position taking group from raw data.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingData">Position taking raw data.</param>
        /// <param name="maxLicenseePt">Get max PT incase  insert new agent level 1.</param>
        /// <returns>List position taking group.</returns>
        private IList<PositionTakingGroup> BuildPositionTakingGroups(
            SettingContext settingContext,
            Dictionary<string, PositionTaking> positionTakingData,
            int maxLicenseePt = 0)
        {
            IList<PositionTakingGroup> positionTakingSetting = new List<PositionTakingGroup>();
            XDocument positionTakingDoc = SettingConfigurations.PositionTakingGridConfig;
            XElement root = positionTakingDoc.Root;

            foreach (XElement sport in root.Elements())
            {
                var betTypeIds = sport.Attribute("betTypeIds").Value;
                if (!string.IsNullOrEmpty(betTypeIds))
                {
                    PositionTakingGroup positionTakingGroup = new PositionTakingGroup
                    {
                        SportId = Converter.ToInt(sport.Attribute("id").Value),
                        IsLive = Converter.ToBool(sport.Attribute("isLive").Value),
                        PositionTakings = new List<PositionTaking>()
                    };

                    var betTypeArray = betTypeIds.Split(',');
                    foreach (string betTypeId in betTypeArray)
                    {
                        GetPositionTakingData(settingContext, positionTakingData, maxLicenseePt, positionTakingGroup, betTypeId);
                    }

                    positionTakingSetting.Add(positionTakingGroup);
                }
            }

            return positionTakingSetting;
        }

        /// <summary>
        /// Build position taking group from raw data.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingData">Position taking raw data.</param>
        /// <returns>
        /// List position taking group.
        /// </returns>
        private IList<PositionTakingGroup> BuildPositionTakingGroupsForUpdate(
            SettingContext settingContext,
            Dictionary<string, PositionTaking> positionTakingData)
        {
            IList<PositionTakingGroup> positionTakingSetting = new List<PositionTakingGroup>();
            XDocument positionTakingDoc = SettingConfigurations.PositionTakingGridConfig;
            XElement root = positionTakingDoc.Root;

            foreach (XElement sport in root.Elements())
            {
                var betTypeIds = sport.Attribute("betTypeIds").Value;
                if (!string.IsNullOrEmpty(betTypeIds))
                {
                    PositionTakingGroup positionTakingGroup = new PositionTakingGroup
                    {
                        SportId = Converter.ToInt(sport.Attribute("id").Value),
                        IsLive = Converter.ToBool(sport.Attribute("isLive").Value),
                        PositionTakings = new List<PositionTaking>()
                    };

                    var betTypeArray = betTypeIds.Split(',');
                    foreach (string betTypeId in betTypeArray)
                    {
                        GetPositionTakingDataForUpdate(settingContext, positionTakingData, positionTakingGroup, betTypeId);
                    }

                    positionTakingSetting.Add(positionTakingGroup);
                }
            }

            return positionTakingSetting;
        }

        /// <summary>
        /// Log Error with more details.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        /// <param name="isEditAgent">Current form is edit agent or edit member.</param>
        /// <param name="actorId">Current agent actor id.</param>
        /// <param name="custId">Current customer is being edited.</param>
        /// <param name="adminId">Current admin id.</param>
        /// <param name="commissionXml">Commission XML data.</param>
        /// <param name="betSettingXml">Betting limit XML data.</param>
        /// <param name="positionTakingSettingXml">Position taking setting data.</param>
        /// <returns>Message details string.</returns>
        private string BuildLogErrorDetail(
            string errorMessage,
            bool isEditAgent,
            int actorId,
            string custId,
            int adminId,
            string commissionXml,
            string betSettingXml,
            string positionTakingSettingXml)
        {
            try
            {
                string errorString = "Message: {0} \nForm: {1} \nActor Id: {2} \nCust Id(s): {3} \nAdmin Id: {4} \n"
                    + "CommissionSetting: {5} \n"
                    + "Betting Limit: {6} \n"
                    + "Share Setting: {7} \n"
                    + "Apply Preset On Member: {8} \n";

                return string.Format(
                    errorString,
                    errorMessage,
                    isEditAgent ? "Edit Agent" : "Edit Member(s)",
                    actorId,
                    custId,
                    adminId,
                    commissionXml,
                    betSettingXml,
                    positionTakingSettingXml);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return errorMessage;
            }
        }

        /// <summary>
        /// Build XML String for Updating.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="commissionSetting">Commission setting data.</param>
        /// <returns>String data for update to database.</returns>
        private string BuildCommissionSettingXML(SettingContext settingContext, CommissionSetting commissionSetting)
        {
            string xmlStringData = string.Empty;
            string tempString = string.Empty;
            if (commissionSetting != null
                && commissionSetting.CommissionItems != null
                && commissionSetting.CommissionItems.Any())
            {
                if (settingContext.TargetRoleId != (int)Role.Member)
                {
                    foreach (CommissionItem comItem in commissionSetting.CommissionItems)
                    {
                        tempString = string.Format(
                            Constant.FORMAT_AGENT_COMMISSION_SETTING_XML,
                            comItem.SportId,
                            comItem.BetTypeId,
                            comItem.DiscountA / Constant.STEP_MUL_DIV,
                            comItem.DiscountB / Constant.STEP_MUL_DIV,
                            comItem.DiscountC / Constant.STEP_MUL_DIV,
                            comItem.DiscountD / Constant.STEP_MUL_DIV);

                        xmlStringData += tempString;
                    }
                }
                else
                {
                    foreach (CommissionItem comItem in commissionSetting.CommissionItems)
                    {
                        tempString = string.Format(
                            Constant.FORMAT_MEMBER_COMMISSION_SETTING_XML,
                            comItem.SportId,
                            comItem.BetTypeId,
                            comItem.Discount / Constant.STEP_MUL_DIV);

                        xmlStringData += tempString;
                    }
                }
            }

            return xmlStringData;
        }

        /// <summary>
        /// Build XML String for Updating.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="bettingLimits">Betting limits data.</param>
        /// <returns>String data to update database.</returns>
        private string BuildBetLimitSettingXML(SettingContext settingContext, IList<BettingLimit> bettingLimits)
        {
            string xmlStringData = string.Empty;
            string tempString = string.Empty;
            if (bettingLimits != null && bettingLimits.Any())
            {
                foreach (BettingLimit betLimit in bettingLimits)
                {
                    xmlStringData += string.Format(
                        Constant.FORMAT_BETTING_LIMIT_XML,
                        betLimit.SportId,
                        betLimit.BetTypeId,
                        betLimit.MinBet,
                        betLimit.MaxBet,
                        betLimit.MaxPerMatch);
                }
            }

            return xmlStringData;
        }

        /// <summary>
        /// Build XML String for Updating.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingSetting">Position taking data.</param>
        /// <returns>String data to update database.</returns>
        private string BuildPositionTakingSettingXML(SettingContext settingContext, IList<PositionTaking> positionTakingSetting)
        {
            string xmlStringData = string.Empty;

            if (positionTakingSetting == null || !positionTakingSetting.Any())
            { 
                return xmlStringData; 
            }

            foreach (PositionTaking positionTaking in positionTakingSetting)
            {
                if (positionTaking != null)
                {
                    xmlStringData += this.BuildXMLForOnePositionTakingSetting(settingContext, positionTaking);
                }
            }

            return xmlStringData;
        }

        /// <summary>
        /// Builds the XML for one position taking setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="positionTaking">The position taking.</param>
        /// <returns>The xml string for one position taking item.</returns>
        private string BuildXMLForOnePositionTakingSetting(SettingContext settingContext, PositionTaking positionTaking)
        {
            string xmlResult = string.Empty;

            if (settingContext.TargetRoleId != (int)Role.Member)
            {
                if (settingContext.TargetRoleId == (int)Role.Agent && settingContext.FormId == FormType.Edit)
                {
                    xmlResult = string.Format(
                            Constant.FORMAT_AGENT_EDIT_AGENT_POSITION_TAKING_XML,
                            positionTaking.SportId,
                            positionTaking.BetTypeId,
                            positionTaking.IsLive ? 1 : 0,
                            (int)(positionTaking.Preset * Constant.STEP_MUL_DIV),
                            (int)(positionTaking.Max * Constant.STEP_MUL_DIV),
                            (int)(positionTaking.Min * Constant.STEP_MUL_DIV),
                            positionTaking.ApplyAll ? 1 : 0);
                }
                else
                {
                    xmlResult = string.Format(
                            Constant.FORMAT_AGENT_POSITION_TAKING_XML,
                            positionTaking.SportId,
                            positionTaking.BetTypeId,
                            positionTaking.IsLive ? 1 : 0,
                            (int)(positionTaking.Preset * Constant.STEP_MUL_DIV),
                            (int)(positionTaking.Max * Constant.STEP_MUL_DIV),
                            (int)(positionTaking.Min * Constant.STEP_MUL_DIV));
                }
            }
            else
            {
                xmlResult = string.Format(
                        Constant.FORMAT_MEMBER_POSITION_TAKING_XML,
                        positionTaking.SportId,
                        positionTaking.BetTypeId,
                        positionTaking.IsLive ? 1 : 0,
                        (int)(positionTaking.Value * Constant.STEP_MUL_DIV));
            }

            return xmlResult;
        }

        /// <summary>
        /// Get Position taking item.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingData">Position taking to save data.</param>
        /// <param name="maxLicenseePt">Max PT in case insert agent level 1.</param>
        /// <param name="positionTakingGroup">Position taking group.</param>
        /// <param name="betTypeId">Bet Type Id.</param>
        private void GetPositionTakingData(
            SettingContext settingContext,
            Dictionary<string, PositionTaking> positionTakingData,
            int maxLicenseePt,
            PositionTakingGroup positionTakingGroup,
            string betTypeId)
        {
            if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                positionTakingGroup.PositionTakings.Add(new PositionTaking
                {
                    SportId = positionTakingGroup.SportId,
                    IsLive = positionTakingGroup.IsLive,
                    BetTypeId = Converter.ToInt(betTypeId),
                    Upline = new PositionTaking
                    {
                        Max = maxLicenseePt
                    },
                    AllDownLine = new PositionTaking()
                });
            }
            else
            {
                string positionTakingKey = string.Format(
                    "{0}_{1}_{2}",
                    positionTakingGroup.SportId,
                    positionTakingGroup.IsLive,
                    betTypeId);

                if (positionTakingData.ContainsKey(positionTakingKey))
                {
                    positionTakingGroup.PositionTakings.Add(positionTakingData[positionTakingKey]);
                }
            }
        }

        /// <summary>
        /// Get Position taking item.
        /// </summary>
        /// <param name="settingContext">Current setting context.</param>
        /// <param name="positionTakingData">Position taking to save data.</param>
        /// <param name="positionTakingGroup">Position taking group.</param>
        /// <param name="betTypeId">Bet Type Id.</param>
        private void GetPositionTakingDataForUpdate(
            SettingContext settingContext,
            Dictionary<string, PositionTaking> positionTakingData,
            PositionTakingGroup positionTakingGroup,
            string betTypeId)
        {
            string positionTakingKey = string.Format(
                "{0}_{1}_{2}",
                positionTakingGroup.SportId,
                positionTakingGroup.IsLive,
                betTypeId);

            if (positionTakingData.ContainsKey(positionTakingKey))
            {
                positionTakingGroup.PositionTakings.Add(positionTakingData[positionTakingKey]);
            }
        }
        #endregion Add Customer Setting
    }
}