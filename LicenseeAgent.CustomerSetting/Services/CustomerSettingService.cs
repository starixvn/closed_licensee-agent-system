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

        public void SwitchUserValidate(SwitchUserValidateCriteria criteria)
        {
            QueryService.Execute(criteria);
        }

        public void GetLegendAgentId(GetLegendAgentIdCriteria criteria)
        {
            QueryService.Execute(criteria);
        }

        public void GetLegendAgentIdHasDeposit(GetLegendAgentIdHasDepositCriteria criteria)
        {
            QueryService.Execute(criteria);
        }

        public CustomerInfo GetMemberInfo(GetMemberInfoCriteria criteria)
        {
            return QueryService.GetEntity<CustomerInfo>(criteria);
        }

        public AgentInfo GetAgentInfoByMember(GetAgentInfoByMemberCriteria criteria)
        {
            return QueryService.GetEntity<AgentInfo>(criteria);
        }
        public AgentInformation GetAgentInfo(GetAgentInfoCriteria criteria)
        {
            return QueryService.GetEntity<AgentInformation>(criteria);
        }

    }
}