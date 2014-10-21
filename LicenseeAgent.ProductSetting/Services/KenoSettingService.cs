namespace LicenseeAgent.ProductSetting.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Exceptions;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.ProductSetting.Criteria.Keno;
    using LicenseeAgent.ProductSetting.Entities;
    using LicenseeAgent.ProductSetting.ViewModels;

    /// <summary>
    /// KenoSettingService class.
    /// </summary>
    public class KenoSettingService : ThirdProductService, IKenoSettingService
    {
        public override ProductCode ProductId
        {
            get
            {
                return ProductCode.Product_8;
            }
        }

        public override string Name
        {
            get
            {
                return "kenoSetting";
            }
        }

        /// <summary>
        /// Insert customer setting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Status of insert command.
        /// </returns>
        public IList<UpdateResult> UpdateCustomer(UpdateKenoCustomerViewModel model)
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
        public ICriteria CreateUpdateCustomerCriteria(UpdateKenoCustomerViewModel model)
        {
            var betLimit3RDXml = string.Empty;
            var positionTaking3RDXml = string.Empty;
            positionTaking3RDXml = XmlBuilder.Build(model.SettingContext, model.KenoSetting.PositionTakingSetting);

            if (model.SettingContext.FormId != FormType.EditIndirectMember)
            {
                betLimit3RDXml = XmlBuilder.BuildForKeno(model.KenoSetting.BettingLimitSetting);
            }

            if (model.SettingContext.TargetRoleId == (int)Role.Member)
            {
                return CreateUpdateMemberCriteria(model, positionTaking3RDXml, betLimit3RDXml);
            }
            else
            {
                if (model.SettingContext.TargetRoleId == (int)Role.AgentLevel1)
                {
                    return new LicenseeEditAgentGameMainCriteria()
                    {
                        AgentId = model.SettingContext.TargetCustId,
                        ProductId = model.SettingContext.ProductId,
                        BetLimit3RD = betLimit3RDXml,
                        PositionTaking3RD = positionTaking3RDXml,
                        Updater = model.SettingContext.LoginUserId,
                        Ip = model.SettingContext.Ip
                    };
                }
                else
                {
                    return new AgentEditAgentGameMainCriteria()
                    {
                        AgentId = model.SettingContext.TargetCustId,
                        ProductId = model.SettingContext.ProductId,
                        BetLimit3RD = betLimit3RDXml,
                        PositionTaking3RD = positionTaking3RDXml,
                        Updater = model.SettingContext.LoginUserId,
                        Ip = model.SettingContext.Ip
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
                criteria = new AgentGetAgentSettingGameForUpdateCriteria
                {
                    AgentId = settingContext.TargetCustId,
                    ProductId = settingContext.ProductId
                };
            }
            else if (settingContext.TargetRoleId == (int)Role.AgentLevel1)
            {
                criteria = new LicenseeGetAgentGameSettingForUpdateCriteria
                {
                    AgentId = settingContext.TargetCustId,
                    ProductId = settingContext.ProductId
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
                    criteria = new AgentGetMemIndirectSettingGameForUpdateCriteria
                    {
                        AgentId = settingContext.CustId,
                        CustId = settingContext.TargetCustId,
                        ProductId = settingContext.ProductId
                    };
                }
                else
                {
                    criteria = new AgentGetMemberSettingGameForUpdateCriteria
                    {
                        CustId = settingContext.TargetCustId,
                        ProductId = settingContext.ProductId
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
        private ICriteria CreateUpdateMemberCriteria(UpdateKenoCustomerViewModel model, string positionTaking, string betLimit)
        {
            if (model.SettingContext.FormId == FormType.EditIndirectMember)
            {
                return new AgentEditMemIndirectGameMainCriteria
                {
                    CustId = model.SettingContext.TargetCustId,
                    AgentId = model.SettingContext.CustId,
                    ProductId = model.SettingContext.ProductId,
                    PositionTaking3RD = positionTaking,
                    Updater = model.SettingContext.LoginUserId,
                    Ip = model.SettingContext.Ip
                };
            }
            else
            {
                return new AgentEditMemGameMainCriteria
                {
                    CustId = model.SettingContext.TargetCustId,
                    ProductId = model.SettingContext.ProductId,
                    PositionTaking3RD = positionTaking,
                    BetLimit3RD = betLimit,
                    Updater = model.SettingContext.LoginUserId,
                    Ip = model.SettingContext.Ip
                };
            }
        }
    }
}
