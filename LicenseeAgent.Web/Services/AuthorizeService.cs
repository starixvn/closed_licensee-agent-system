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
        public void CheckDownlineBelong(CheckDownlineBelongCriteria criteria)
        {
            if (criteria.CustId == criteria.TargetCustId)
            {
                criteria.IsBelongTo = true;
                criteria.IsDirectDownLine = true;
                return;
            }

            Execute(criteria);
        }

        /// <summary>
        /// Determines whether [is down-line belong] [the specified customer identifier].
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="targetCustId">The target customer identifier.</param>
        /// <param name="downlineRole">The down-line role.</param>
        /// <returns>Returns true if is down-line.</returns>
        public bool IsDownlineBelong(int custId, int targetCustId, DownlineRole downlineRole)
        {
            if (custId == targetCustId)
            {
                return true;
            }

            var criteria = new CheckDownlineBelongCriteria
            {
                CustId = custId,
                TargetCustId = targetCustId,
                Type = (int)downlineRole
            };

            CheckDownlineBelong(criteria);
            return criteria.IsBelongTo;
        }

        public bool IsDownlineBelong(int targetCustId, DownlineRole downlineRole)
        {
            var custId = SessionManager.CurrentSession.AgentInfo.AgentId;
            return IsDownlineBelong(custId, targetCustId, downlineRole);
        }

        /// <summary>
        /// Validates the direct down-line.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns boolean.</returns>
        public bool ValidateDirectDownline(CheckDownlineBelongCriteria criteria)
        {
            if (SessionManager.CurrentSession.AgentInfo.IsLevel0)
            {
                return true;
            }

            if (criteria.Type == (int)DownlineRole.Agent)
            {
                if (!criteria.IsDirectDownLine)
                {
                    return false;
                }
            }
            else
            {
                if (!SessionManager.CurrentSession.AgentInfo.IsLastLevel)
                {
                    return false;
                }
            }

            return true;
        }
    }
}