namespace LicenseeAgent.AccountInformation.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using LicenseeAgent.AccountInformation.Criteria;
    using LicenseeAgent.AccountInformation.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Taiwan.Entities;

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
        /// <param name="criteria">Get member list criteria.</param>
        /// <returns>
        /// Search user result.
        /// </returns>
        public SearchMemberEntity GetMembers(ICriteria criteria)
        {
            var dict = GetEntites<CustomerEntity, UpLineEntity, DownLineEntity>(criteria);
            var customer = (dict[typeof(CustomerEntity).Name] as List<CustomerEntity>).FirstOrDefault();
            var uplines = dict[typeof(UpLineEntity).Name] as List<UpLineEntity>;
            var downlines = dict[typeof(DownLineEntity).Name] as List<DownLineEntity>;

            return new SearchMemberEntity
            {
                Customer = customer,
                UpLines = uplines,
                DownLines = downlines
            };
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
                        UsedCredit = criteria.UsedCredit,
                        YesterdayBalance = criteria.YesterdayBalance,
                        YesterdayCash = criteria.YesterdayCash
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

        /// <summary>
        /// Get agent balance.
        /// </summary>
        /// <param name="criteria">Agent balance criteria.</param>
        /// <returns>Agent balance entity.</returns>
        public BalanceEntity GetCashAgentBalance(CashAgentBalanceCriteria criteria)
        {
            Execute(criteria);

            return new BalanceEntity
            {
                CashBalance = criteria.Balance,
                Outstanding = criteria.Outstanding,
                TotalOutstanding = criteria.TotalOutstanding,
                Today = criteria.Today
            };
        }

        /// <summary>
        /// Get member balance.
        /// </summary>
        /// <param name="criteria">Member balance criteria.</param>
        /// <returns>Member balance entity.</returns>
        public BalanceEntity GetCashMemberBalance(CashMemberBalanceCriteria criteria)
        {
            Execute(criteria);

            return new BalanceEntity
            {
                PendingFund = criteria.PendingFund,
                AvailableFund = criteria.AvailableFund
            };
        }
    }
}