namespace LicenseeAgent.ProductSetting.Services.ProductSetting
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
    using LicenseeAgent.ProductSetting.Criteria.VirtualSports;
    using LicenseeAgent.ProductSetting.Entities;
    using LicenseeAgent.ProductSetting.Utils;
    using LicenseeAgent.ProductSetting.ViewModels;

    /// <summary>
    /// Customer repository.
    /// </summary>
    public class VirtualSportsSettingService : ThirdProductService, IVirtualSportsSettingService
    {
        public override ProductCode ProductId
        {
            get
            {
                return ProductCode.Product_5;
            }
        }

        public override string Name
        {
            get
            {
                return "virtualSportsSetting";
            }
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
        /// Creates the update customer criteria.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns criteria.</returns>
        public ICriteria CreateUpdateCustomerCriteria(UpdateVirtualSportsCustomerViewModel model)
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
        /// Initializes the criteria for getting Virtual Sports setting for update.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Criteria for getting Virtual Sports setting.</returns>
        public override ICriteria GetCriteriaForUpdateAgent(SettingContext settingContext)
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
        public override ICriteria GetCriteriaForUpdateMember(SettingContext settingContext)
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