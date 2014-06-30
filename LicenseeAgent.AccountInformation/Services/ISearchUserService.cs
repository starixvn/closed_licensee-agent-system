namespace LicenseeAgent.AccountInformation.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using LicenseeAgent.AccountInformation.Criteria;
    using LicenseeAgent.AccountInformation.Entities;

    public interface ISearchUserService
    {
        /// <summary>
        /// Suggest member by Admin.
        /// </summary>
        /// <param name="criteria">Suggest Search member criteria.</param>
        /// <returns>List Suggestion.</returns>
        IList<SuggestionEntity> SuggestAdmin(AdminSuggestSearchMemberCriteria criteria);

        /// <summary>
        /// Suggest member by Agent.
        /// </summary>
        /// <param name="criteria">Suggest Search member criteria.</param>
        /// <returns>List Suggestion.</returns>
        IList<SuggestionEntity> SuggestAgent(AgentSuggestSearchMemberCriteria criteria);

        /// <summary>
        /// Get members.
        /// </summary>
        /// <typeparam name="T1">Type output 1.</typeparam>
        /// <typeparam name="T2">Type output 2.</typeparam>
        /// <typeparam name="T3">Type output 3.</typeparam>
        /// <param name="criteria">Get member list criteria.</param>
        /// <returns>Search user result.</returns>
        IDictionary<string, object> GetMembers<T1, T2, T3>(ICriteria criteria);

        /// <summary>
        /// Get down line list.
        /// </summary>
        /// <param name="criteria">Down line list criteria.</param>
        /// <returns>List down line.</returns>
        IList<DownLineEntity> GetDirectDownlines(GetDirectDownLineCriteria criteria);

        /// <summary>
        /// Get agent balance.
        /// </summary>
        /// <param name="criteria">Agent balance criteria.</param>
        /// <returns>Agent balance entity.</returns>
        BalanceEntity GetAgentBalance(AgentBalanceCriteria criteria);

        /// <summary>
        /// Get member balance.
        /// </summary>
        /// <param name="criteria">Member balance criteria.</param>
        /// <returns>Member balance entity.</returns>
        BalanceEntity GetMemberBalance(MemberBalanceCriteria criteria);
    }
}