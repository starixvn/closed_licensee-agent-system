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
        /// <param name="criteria">Get member list criteria.</param>
        /// <returns>
        /// Search user result.
        /// </returns>
        SearchMemberEntity GetMembers(ICriteria criteria);

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