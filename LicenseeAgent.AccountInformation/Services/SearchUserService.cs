// ----------------------------------------------------------------------
// <copyright file="SearchUserService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Services
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.AccountInformation.Criteria;
    using LicenseeAgent.AccountInformation.Entities;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// SearchUserService class.
    /// </summary>
    public class SearchUserService : QueryService, ISearchUserService
    {
        /// <summary>
        /// Suggest member by Admin.
        /// </summary>
        /// <param name="criteria">Suggest Search member criteria.</param>
        /// <returns>List Suggestion.</returns>
        public IList<SuggestionEntity> SuggestAdmin(AdminSuggestSearchMemberCriteria criteria)
        {
            return GetEntites<SuggestionEntity>(criteria);
        }

        /// <summary>
        /// Suggest member by Agent.
        /// </summary>
        /// <param name="criteria">Suggest Search member criteria.</param>
        /// <returns>List Suggestion.</returns>
        public IList<SuggestionEntity> SuggestAgent(AgentSuggestSearchMemberCriteria criteria)
        {
            return GetEntites<SuggestionEntity>(criteria);
        }

        /// <summary>
        /// Get members.
        /// </summary>
        /// <typeparam name="T1">Type output 1.</typeparam>
        /// <typeparam name="T2">Type output 2.</typeparam>
        /// <typeparam name="T3">Type output 3.</typeparam>
        /// <param name="criteria">Get member list criteria.</param>
        /// <returns>Search user result.</returns>
        public IDictionary<string, object> GetMembers<T1, T2, T3>(ICriteria criteria)
        {
            return GetEntites<T1, T2, T3>(criteria);
        }

        /// <summary>
        /// Get down line list.
        /// </summary>
        /// <param name="criteria">Down line list criteria.</param>
        /// <returns>List down line.</returns>
        public IList<DownLineEntity> GetDirectDownlines(GetDirectDownLineCriteria criteria)
        {
            return GetEntites<DownLineEntity>(criteria);
        }

        /// <summary>
        /// Get agent balance.
        /// </summary>
        /// <param name="criteria">Agent balance criteria.</param>
        /// <returns>Agent balance entity.</returns>
        public BalanceEntity GetAgentBalance(AgentBalanceCriteria criteria)
        {
            Execute(criteria);

            return new BalanceEntity
                    {
                         GivenCredit = criteria.GivenCredit,
                         Outstanding = criteria.Outstanding,
                         TotalBalance = criteria.TotalBalance,
                         TotalCash = criteria.TotalCash,
                         TotalOutstanding = criteria.TotalOutstanding,
                         UsedCredit  = criteria.UsedCredit,
                         YesterdayBalance = criteria.YesterdayBalance,
                         YesterdayCash  = criteria.YesterdayCash
                    };
        }

        /// <summary>
        /// Get member balance.
        /// </summary>
        /// <param name="criteria">Member balance criteria.</param>
        /// <returns>Member balance entity.</returns>
        public BalanceEntity GetMemberBalance(MemberBalanceCriteria criteria)
        {
            Execute(criteria);

            return new BalanceEntity
            {
                BetCredit = criteria.BetCredit,
                GivenCredit = criteria.GivenCredit,
                Outstanding = criteria.Outstanding,
                TotalBalance = criteria.TotalBalance,
                YesterdayBalance = criteria.YesterdayBalance
            };
        }
    }
}