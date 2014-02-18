// ----------------------------------------------------------------------
// <copyright file="SearchUserService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.AccountInformation.Criteria;
    using LicenseeAgent.AccountInformation.Entities;
    using LicenseeAgent.Core.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SearchUserService : QueryService, ISearchUserService
    {
        /// <summary>
        /// Suggest member by Admin.
        /// </summary>
        /// <param name="criteria">Suggest Search member criteria.</param>
        /// <returns>List Suggestion.</returns>
        public IEnumerable<SuggestionEntity> SuggestAdmin(AdminSuggestSearchMemberCriteria criteria)
        {
            return GetEntites<SuggestionEntity>(criteria);
        }

        /// <summary>
        /// Suggest member by Agent.
        /// </summary>
        /// <param name="criteria">Suggest Search member criteria.</param>
        /// <returns>List Suggestion.</returns>
        public IEnumerable<SuggestionEntity> SuggestAgent(AgentSuggestSearchMemberCriteria criteria)
        {
            return GetEntites<SuggestionEntity>(criteria);
        }

        /// <summary>
        /// Get members by Admin.
        /// </summary>
        /// <typeparam name="T1">Type output 1.</typeparam>
        /// <typeparam name="T2">Type output 2.</typeparam>
        /// <typeparam name="T3">Type output 3.</typeparam>
        /// <param name="criteria">Get member list criteria.</param>
        /// <returns>Search user result.</returns>
        public IDictionary<string, object> GetMembersAdmin<T1, T2, T3>(AdminGetMemberListCriteria criteria)
        {
            return GetEntites<T1, T2, T3>(criteria);
        }

        /// <summary>
        /// Get members by Agent.
        /// </summary>
        /// <typeparam name="T1">Type output 1.</typeparam>
        /// <typeparam name="T2">Type output 2.</typeparam>
        /// <typeparam name="T3">Type output 3.</typeparam>
        /// <param name="criteria">Get member list criteria.</param>
        /// <returns>Search user result.</returns>
        public IDictionary<string, object> GetMembersAgent<T1, T2, T3>(AgentGetMemberListCriteria criteria)
        {
            return GetEntites<T1, T2, T3>(criteria);
        }

        /// <summary>
        /// Get down line list.
        /// </summary>
        /// <param name="criteria">Down line list criteria.</param>
        /// <returns>List down line.</returns>
        public IEnumerable<DownlineEntity> GetDownlines(GetDownlineListCriteria criteria)
        {
            return GetEntites<DownlineEntity>(criteria);
        }

        /// <summary>
        /// Get agent balance.
        /// </summary>
        /// <param name="criteria">Agent balance criteria.</param>
        /// <returns>Agent balance entity.</returns>
        public AgentBalanceEntity GetAgentBalance(AgentBalanceCriteria criteria)
        {
            Execute(criteria);

            return new AgentBalanceEntity
                    {
                         AgentOutstanding = criteria.AgentOutstanding,
                         GivenCredit = criteria.GivenCredit,
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
        public MemberBalanceEntity GetAgentBalance(MemberBalanceCriteria criteria)
        {
            Execute(criteria);

            return new MemberBalanceEntity
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