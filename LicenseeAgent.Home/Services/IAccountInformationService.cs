namespace LicenseeAgent.Home.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using LicenseeAgent.Home.Criteria.AccountInformation;
    using LicenseeAgent.Home.Entities.AccountInformation;

    public interface IAccountInformationService
    {
        /// <summary>
        /// Suggest member by Admin.
        /// </summary>
        /// <param name="criteria">Suggest Search member criteria.</param>
        /// <returns>List Suggestion.</returns>
        IList<SuggestionEntity> SuggestByAdmin(AdminSuggestSearchMemberCriteria criteria);

        /// <summary>
        /// Suggest member by Agent.
        /// </summary>
        /// <param name="criteria">Suggest Search member criteria.</param>
        /// <returns>List Suggestion.</returns>
        IList<SuggestionEntity> SuggestByAgent(AgentSuggestSearchMemberCriteria criteria);

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
        BalanceEntity GetAgentCreditBalance(AgentCreditBalanceCriteria criteria);

        /// <summary>
        /// Get member balance.
        /// </summary>
        /// <param name="criteria">Member balance criteria.</param>
        /// <returns>Member balance entity.</returns>
        BalanceEntity GetMemberCreditBalance(MemberCreditBalanceCriteria criteria);

        /// <summary>
        /// Get agent balance.
        /// </summary>
        /// <param name="criteria">Agent balance criteria.</param>
        /// <returns>Agent balance entity.</returns>
        BalanceEntity GetAgentCashBalance(AgentCashBalanceCriteria criteria);

        /// <summary>
        /// Get member balance.
        /// </summary>
        /// <param name="criteria">Member balance criteria.</param>
        /// <returns>Member balance entity.</returns>
        BalanceEntity GetMemberCashBalance(MemberCashBalanceCriteria criteria);

        IList<ProductSettingEntity> GetAgentProductSetting(GetAgentProductSettingCriteria criteria);

        IList<ProductSettingEntity> GetMemberProductSetting(GetMemberProductSettingCriteria criteria);
    }
}