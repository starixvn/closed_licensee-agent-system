// --------------------------------------------------------------------------
// <copyright file="AuthorizeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthorizeService : QueryService, IAuthorizeService
    {
        public bool IsDirectDownline(CheckDownlineBelongCriteria criteria)
        {
            var agentId = SessionManager.CurrentSession.AgentInfo.AgentId;
            if (agentId == criteria.TargetCustId)
            {
                return true;
            }

            criteria.CustId = agentId;
            Execute(criteria);

            return criteria.Type == (int)DownlineRole.Agent
                ? criteria.IsDirectDownLine
                : criteria.IsBelongTo && SessionManager.CurrentSession.AgentInfo.IsLastLevel;
        }

        public bool IsDownline(CheckDownlineBelongCriteria criteria)
        {
            var agentId = SessionManager.CurrentSession.AgentInfo.AgentId;
            if (agentId == criteria.TargetCustId)
            {
                return true;
            }

            criteria.CustId = agentId;
            Execute(criteria);

            return criteria.IsBelongTo;
        }
    }
}