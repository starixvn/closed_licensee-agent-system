// ----------------------------------------------------------------------
// <copyright file="CustomerSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Criteria.SportsBook;
    using LicenseeAgent.CustomerSetting.Criteria.SportsBook.Agent;
    using LicenseeAgent.CustomerSetting.Criteria.SportsBook.Licensee;
    using LicenseeAgent.CustomerSetting.Criteria.SportsBook.Member;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerSettingService : QueryService, ICustomerSettingService
    {
        private readonly Services.SportsBook.ISettingService _sportsBookService = new Services.SportsBook.SettingService();

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
            // Build XML setting for insert new customer.
            var discountXmlStringData = XmlBuilder.Build(model.SettingContext, model.SportsBookSetting.CommissionSetting);
            var betLimitXmlStringData = XmlBuilder.Build(model.SportsBookSetting.BettingLimitSetting);
            var positionTakingXmlStringData = XmlBuilder.Build(
                model.SettingContext, model.SportsBookSetting.PositionTakingSetting);
            var discountXml = !string.IsNullOrEmpty(discountXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, discountXmlStringData)
                : string.Empty;
            var betLimitXml = !string.IsNullOrEmpty(betLimitXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, betLimitXmlStringData)
                : string.Empty;
            var positionTakingXml = !string.IsNullOrEmpty(positionTakingXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, positionTakingXmlStringData)
                : string.Empty;

            var criteria = CreateInsertCustomerCriteria(model, discountXml, betLimitXml, positionTakingXml);
            return _sportsBookService.InsertCustomer(criteria);
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
            var discountXmlStringData = XmlBuilder.Build(model.SettingContext, model.SportsBookSetting.CommissionSetting);
            var betLimitXmlStringData = XmlBuilder.Build(model.SportsBookSetting.BettingLimitSetting);
            var positionTakingXmlStringData = XmlBuilder.Build(
                model.SettingContext, model.SportsBookSetting.PositionTakingSetting);
            var discountXml = !string.IsNullOrEmpty(discountXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, discountXmlStringData)
                : string.Empty;
            var betLimitXml = !string.IsNullOrEmpty(betLimitXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, betLimitXmlStringData)
                : string.Empty;
            var positionTakingXml = !string.IsNullOrEmpty(positionTakingXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, positionTakingXmlStringData)
                : string.Empty;

            var criteria = CreateUpdateCustomerCriteria(model, discountXml, betLimitXml, positionTakingXml);
            return _sportsBookService.UpdateCustomer(criteria);
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
            var positionTakingXmlStringData = XmlBuilder.Build(
                model.SettingContext, model.SportsBookSetting.PositionTakingSetting);
            var positionTakingXml = !string.IsNullOrEmpty(positionTakingXmlStringData)
                ? string.Format(Constant.FORMAT_XML_ROOT, positionTakingXmlStringData)
                : string.Empty;

            var criteria = CreateUpdateCustomerCriteria(model, string.Empty, string.Empty, positionTakingXml);
            return _sportsBookService.UpdateIndirectCustomer(criteria);
        }

        /// <summary>
        /// Creates the insert customer criteria.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="discountXml">The discount XML.</param>
        /// <param name="betLimitXml">The bet limit XML.</param>
        /// <param name="positionTakingXml">The position taking XML.</param>
        /// <returns>Returns a criteria.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static ICriteria CreateInsertCustomerCriteria(
            InsertCustomerViewModel model,
            string discountXml,
            string betLimitXml,
            string positionTakingXml)
        {
            var custInfo = model.SportsBookSetting.CustInfo;
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
                return new AgentInsertMemberSettingCriteria(criteriaBase)
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
                    return new LicenseeInsertAgentSettingCriteria(criteriaBase)
                    {
                        AgentL0Id = model.SettingContext.CustId,
                        UserSalt = passwordSalt,
                        UserPwd = hashPassword,
                        Phone = custInfo.Phone,
                        Mobile = custInfo.Mobile,
                        FirstName = custInfo.FirstName,
                        LastName = custInfo.LastName,
                        Fax = custInfo.Fax,
                        Status = (byte)model.SportsBookSetting.CustInfo.Status
                    };
                }
                else
                {
                    var passwordSalt = Cryptography.CreateSalt();
                    var hashPassword = Cryptography.CreateHash(model.SportsBookSetting.CustInfo.Password, passwordSalt);
                    return new AgentInsertAgentSettingCriteria(criteriaBase)
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
                if (model.SettingContext.FormId == FormType.EditIndirectMember)
                {
                    return new AgentEditMemberIndirectCriteria
                    {
                        Updater = model.SettingContext.LoginUserId,
                        AgentId = model.SettingContext.CustId,
                        CustId = model.SettingContext.TargetCustId,
                        PositionTaking = criteriaBase.PositionTaking,
                        Ip = criteriaBase.Ip
                    };
                }
                else
                {
                    return new AgentUpdateMemberSettingCriteria(criteriaBase)
                    {
                        CustId = model.SettingContext.TargetCustId
                    };
                }
            }
            else
            {
                if (model.SettingContext.TargetRoleId == (int)Role.AgentLevel1)
                {
                    return new LicenseeUpdateAgentSettingCriteria(criteriaBase)
                    {
                        AgentId = model.SettingContext.TargetCustId
                    };
                }
                else
                {
                    return new AgentUpdateAgentSettingCriteria(criteriaBase)
                    {
                        AgentId = model.SettingContext.TargetCustId
                    };
                }
            }
        }
    }
}