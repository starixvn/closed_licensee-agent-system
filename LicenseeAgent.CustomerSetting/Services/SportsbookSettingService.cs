// --------------------------------------------------------------------------
// <copyright file="SportsBookSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Criteria.SportsBook;
    using LicenseeAgent.CustomerSetting.Criteria.SportsBook.Agent;
    using LicenseeAgent.CustomerSetting.Criteria.SportsBook.Licensee;
    using LicenseeAgent.CustomerSetting.Criteria.SportsBook.Member;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Repositories;
    using LicenseeAgent.CustomerSetting.ViewModels;

    /// <summary>
    /// Customer Service.
    /// </summary>
    public class SportsBookSettingService : QueryService, ISportsBookSettingService
    {
        private readonly ISportsBookSettingRepository _customerSettingRepository;

        public SportsBookSettingService(ISportsBookSettingRepository customerSettingRepository)
        {
            _customerSettingRepository = customerSettingRepository;
        }

        #region Add Customer Setting

        /// <summary>
        /// Check user name.
        /// </summary>
        /// <param name="criteria">Check exist username criteria.</param>
        public void CheckExistUserName(CheckExistUserNameCriteria criteria)
        {
            Execute(criteria);
        }

        /// <summary>
        /// Get Agent setting for insert new.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>
        /// Sports book customer setting.
        /// </returns>
        public CustomerSetting GetSportsBookSettingForInsert(SettingContext settingContext, out int errCode)
        {
            CustomerInfo custInfo;
            CommissionSetting commissionSetting;
            IList<BettingLimit> bettingLimits;
            Dictionary<string, PositionTaking> positionTakingData;

            ICriteria getAgentSettingCriteria = GetInsertSettingCriteria(settingContext);

            errCode = Result.FailureValue;
            var maxLicenseePt = 0;
            _customerSettingRepository.GetSettingForInsert(
                getAgentSettingCriteria,
                settingContext,
                out custInfo,
                out commissionSetting,
                out bettingLimits,
                out positionTakingData,
                out errCode,
                out maxLicenseePt);

            if (errCode == Result.SuccessValue)
            {
                return new CustomerSetting
                {
                    CustInfo = custInfo,
                    CommissionSetting = commissionSetting,
                    BettingLimits = bettingLimits,
                    PositionTakingSetting = ServiceUtils.BuildPositionTakingGroups((int)Product.Product_0, settingContext, positionTakingData, maxLicenseePt)
                };
            }

            return null;
        }

        /// <summary>
        /// Get Agent setting for updating.
        /// </summary>
        /// <param name="settingContext">Current setting Context.</param>
        /// <returns>
        /// Sports book customer setting.
        /// </returns>
        public CustomerSetting GetSportsBookSettingForUpdate(SettingContext settingContext)
        {
            CustomerInfo custInfo;
            CommissionSetting commissionSetting;
            IList<BettingLimit> bettingLimits;
            Dictionary<string, PositionTaking> positionTakingData;
            ICriteria getAgentSettingCriteria = InitializeCriteriaForGettingSportsBookSettingForUpdate(settingContext);

            int msgId;
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
                return new CustomerSetting
                {
                    CustInfo = custInfo,
                    CommissionSetting = commissionSetting,
                    BettingLimits = bettingLimits,
                    PositionTakingSetting = ServiceUtils.BuildPositionTakingGroupsForUpdate((int)Product.Product_0, positionTakingData)
                };
            }

            return new CustomerSetting
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
            var discountXmlStringData = ServiceUtils.BuildCommissionSettingXml(model.SettingContext, model.UpdateDataSportsBookCustomer.CommissionSetting);
            var betLimitXmlStringData = ServiceUtils.BuildBetLimitSettingXml(model.UpdateDataSportsBookCustomer.BettingLimits);
            var positionTakingXmlStringData = ServiceUtils.BuildPositionTakingSettingXml(
                model.SettingContext, model.UpdateDataSportsBookCustomer.PositionTakingSetting);
            var discountXml = !string.IsNullOrEmpty(discountXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, discountXmlStringData)
                : string.Empty;
            var betLimitXml = !string.IsNullOrEmpty(betLimitXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, betLimitXmlStringData)
                : string.Empty;
            var positionTakingXml = !string.IsNullOrEmpty(positionTakingXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, positionTakingXmlStringData)
                : string.Empty;

            return InsertCustomerData(model, discountXml, betLimitXml, positionTakingXml);
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
            var discountXmlStringData = ServiceUtils.BuildCommissionSettingXml(model.SettingContext, model.UpdateDataSportsBookCustomer.CommissionSetting);
            var betLimitXmlStringData = ServiceUtils.BuildBetLimitSettingXml(model.UpdateDataSportsBookCustomer.BettingLimits);
            var positionTakingXmlStringData = ServiceUtils.BuildPositionTakingSettingXml(
                model.SettingContext, model.UpdateDataSportsBookCustomer.PositionTakingSetting);
            var discountXml = !string.IsNullOrEmpty(discountXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, discountXmlStringData)
                : string.Empty;
            var betLimitXml = !string.IsNullOrEmpty(betLimitXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, betLimitXmlStringData)
                : string.Empty;
            var positionTakingXml = !string.IsNullOrEmpty(positionTakingXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, positionTakingXmlStringData)
                : string.Empty;

            return UpdateCustomerData(model, discountXml, betLimitXml, positionTakingXml);
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
            var positionTakingXmlStringData = ServiceUtils.BuildPositionTakingSettingXml(
                model.SettingContext, model.UpdateDataSportsBookCustomer.PositionTakingSetting);
            var positionTakingXml = !string.IsNullOrEmpty(positionTakingXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, positionTakingXmlStringData)
                : string.Empty;

            return UpdateIndirectCustomerData(model, positionTakingXml);
        }

        /// <summary>
        /// Get Insert Setting Criteria.
        /// </summary>
        /// <param name="settingContext">Setting Context.</param>
        /// <returns>Insert Criteria.</returns>
        private static ICriteria GetInsertSettingCriteria(SettingContext settingContext)
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
        /// Creates the insert customer criteria.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="discountXml">The discount XML.</param>
        /// <param name="betLimitXml">The bet limit XML.</param>
        /// <param name="positionTakingXml">The position taking XML.</param>
        /// <returns>Returns a criteria.</returns>
        private static ICriteria CreateInsertCustomerCriteria(
            InsertCustomerViewModel model,
            string discountXml,
            string betLimitXml,
            string positionTakingXml)
        {
            var custInfo = model.UpdateDataSportsBookCustomer.CustInfo;
            var criteriaBase = new InsertCustomerCriteria
            {
                UserName = custInfo.UserName,
                Ip = model.SettingContext.Ip,
                Creator = model.SettingContext.LoginUserId,
                Credit = custInfo.Credit,
                Discount = discountXml,
                BetLimit = betLimitXml,
                PositionTaking = positionTakingXml
            };

            if (model.SettingContext.TargetRoleId == (int)Role.Member)
            {
                return CreateAgentInsertMemberCriteria(criteriaBase, model);
            }

            return model.SettingContext.TargetRoleId == (int)Role.AgentLevel1
                ? CreateLicenseeInsertAgentCriteria(criteriaBase, model)
                : CreateAgentInsertAgentCriteria(criteriaBase, model);
        }

        /// <summary>
        /// Creates the insert customer criteria.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="discountXml">The discount XML.</param>
        /// <param name="betLimitXml">The bet limit XML.</param>
        /// <param name="positionTakingXml">The position taking XML.</param>
        /// <returns>Returns a criteria.</returns>
        private static ICriteria CreateUpdateCustomerCriteria(
            UpdateCustomerViewModel model,
            string discountXml,
            string betLimitXml,
            string positionTakingXml)
        {
            var criteriaBase = new UpdateCustomerCriteria
            {
                Discount = discountXml,
                BetLimit = betLimitXml,
                PositionTaking = positionTakingXml,
                Ip = model.SettingContext.Ip,
                Updater = model.SettingContext.LoginUserId
            };

            if (model.SettingContext.TargetRoleId == (int)Role.Member)
            {
                return model.SettingContext.FormId == FormType.EditIndirectMember
                    ? CreateAgentUpdateIndirectMemberCriteria(criteriaBase, model)
                    : CreateAgentUpdateMemberCriteria(criteriaBase, model);
            }

            return model.SettingContext.TargetRoleId == (int)Role.AgentLevel1
                ? CreateLicenseeUpdateAgentCriteria(criteriaBase, model)
                : CreateAgentUpdateAgentCriteria(criteriaBase, model);
        }

        private static ICriteria CreateAgentInsertMemberCriteria(
            InsertCustomerCriteria criteriaBase,
            InsertCustomerViewModel model)
        {
            var criteria = new AgentInsertMemberSettingCriteria(criteriaBase)
            {
                UGroup = model.UpdateDataSportsBookCustomer.CommissionSetting.UserGroup,
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
        private static ICriteria CreateAgentUpdateMemberCriteria(
            UpdateCustomerCriteria criteriaBase,
            UpdateCustomerViewModel model)
        {
            var criteria = new AgentUpdateMemberSettingCriteria(criteriaBase)
            {
                CustId = model.SettingContext.TargetCustId
            };

            return criteria;
        }

        /// <summary>
        /// Creates the agent update member criteria.
        /// </summary>
        /// <param name="criteriaBase">The criteria base.</param>
        /// <param name="model">The model.</param>
        /// <returns>The criteria for agent update member.</returns>
        private static ICriteria CreateAgentUpdateIndirectMemberCriteria(
            UpdateCustomerCriteria criteriaBase,
            UpdateCustomerViewModel model)
        {
            var criteria = new AgentEditMemberIndirectCriteria()
            {
                Updater = model.SettingContext.LoginUserId,
                AgentId = model.SettingContext.CustId,
                CustId = model.SettingContext.TargetCustId,
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
        private static ICriteria CreateLicenseeInsertAgentCriteria(
            InsertCustomerCriteria criteriaBase,
            InsertCustomerViewModel model)
        {
            var passwordSalt = Cryptography.CreateSalt();
            var hashPassword = Cryptography.CreateHash(model.UpdateDataSportsBookCustomer.CustInfo.Password, passwordSalt);
            var custInfo = model.UpdateDataSportsBookCustomer.CustInfo;

            var criteria = new LicenseeInsertAgentSettingCriteria(criteriaBase)
            {
                AgentL0Id = model.SettingContext.CustId,
                UserSalt = passwordSalt,
                UserPwd = hashPassword,
                Phone = custInfo.Phone,
                Mobile = custInfo.Mobile,
                FirstName = custInfo.FirstName,
                LastName = custInfo.LastName,
                Fax = custInfo.Fax,
                Status = (byte)model.UpdateDataSportsBookCustomer.CustInfo.Status
            };

            return criteria;
        }

        /// <summary>
        /// Creates the licensee insert agent criteria.
        /// </summary>
        /// <param name="criteriaBase">The criteria base.</param>
        /// <param name="model">The model.</param>
        /// <returns>Returns a criteria.</returns>
        private static ICriteria CreateLicenseeUpdateAgentCriteria(
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
        private static ICriteria CreateAgentInsertAgentCriteria(
            InsertCustomerCriteria criteriaBase,
            InsertCustomerViewModel model)
        {
            var passwordSalt = Cryptography.CreateSalt();
            var hashPassword = Cryptography.CreateHash(model.UpdateDataSportsBookCustomer.CustInfo.Password, passwordSalt);
            var custInfo = model.UpdateDataSportsBookCustomer.CustInfo;

            var criteria = new AgentInsertAgentSettingCriteria(criteriaBase)
            {
                ParentId = model.SettingContext.CustId,
                UserSalt = passwordSalt,
                UserPwd = hashPassword,
                Phone = custInfo.Phone,
                Mobile = custInfo.Mobile,
                FirstName = custInfo.FirstName,
                LastName = custInfo.LastName,
                Fax = custInfo.Fax,
                Status = model.UpdateDataSportsBookCustomer.CustInfo.Status
            };

            return criteria;
        }

        /// <summary>
        /// Creates the agent insert agent criteria.
        /// </summary>
        /// <param name="criteriaBase">The criteria base.</param>
        /// <param name="model">The model.</param>
        /// <returns>Returns a criteria.</returns>
        private static ICriteria CreateAgentUpdateAgentCriteria(
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
        private static ICriteria InitializeCriteriaForGettingSportsBookSettingForUpdate(SettingContext settingContext)
        {
            ICriteria getAgentSettingCriteria = null;

            if (settingContext.TargetRoleId == (int)Role.Member)
            {
                if (settingContext.FormId == FormType.EditIndirectMember)
                {
                    getAgentSettingCriteria = new AgentSelectMemberIndirectForUpdateCriteria
                    {
                        AgentId = settingContext.CustId,
                        CustId = settingContext.TargetCustId
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
        /// Insert customer to database.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="discountXml">Discount XML data.</param>
        /// <param name="betLimitXml">Bet limit XML data.</param>
        /// <param name="positionTakingXml">Position taking setting XML data.</param>
        /// <returns>
        /// Insert status and customer id.
        /// </returns>
        private InsertResult InsertCustomerData(
            InsertCustomerViewModel model,
            string discountXml,
            string betLimitXml,
            string positionTakingXml)
        {
            var criteria = CreateInsertCustomerCriteria(model, discountXml, betLimitXml, positionTakingXml);
            return _customerSettingRepository.InsertCustomer(criteria);
        }

        /// <summary>
        /// Insert customer to database.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="discountXml">Discount XML data.</param>
        /// <param name="betLimitXml">Bet limit XML data.</param>
        /// <param name="positionTakingXml">Position taking setting XML data.</param>
        /// <returns>
        /// Insert status and customer id.
        /// </returns>
        private InsertResult UpdateCustomerData(
            UpdateCustomerViewModel model,
            string discountXml,
            string betLimitXml,
            string positionTakingXml)
        {
            var criteria = CreateUpdateCustomerCriteria(model, discountXml, betLimitXml, positionTakingXml);
            return _customerSettingRepository.UpdateCustomer(criteria);
        }

        /// <summary>
        /// Edit customer to database.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="positionTakingXml">Position taking setting XML data.</param>
        /// <returns>
        /// Insert status and customer id.
        /// </returns>
        private InsertResult UpdateIndirectCustomerData(
            UpdateCustomerViewModel model,
            string positionTakingXml)
        {
            var criteria = CreateUpdateCustomerCriteria(model, string.Empty, string.Empty, positionTakingXml);
            return _customerSettingRepository.UpdateIndirectCustomer(criteria);
        }

        #endregion Add Customer Setting
    }
}