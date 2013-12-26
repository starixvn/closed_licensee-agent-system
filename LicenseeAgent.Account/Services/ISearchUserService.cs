// ----------------------------------------------------------------------
// <copyright file="ISearchUserService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Account.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Entities;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISearchUserService
    {
        /// <summary>
        /// Suggest member by Admin.
        /// </summary>
        /// <param name="criteria">Suggest Search member criteria.</param>
        /// <returns>List Suggestion.</returns>
        IEnumerable<SuggestionEntity> SuggestAdmin(AdminSuggestSearchMemberCriteria criteria);
        /// <summary>
        /// Suggest member by Agent.
        /// </summary>
        /// <param name="criteria">Suggest Search member criteria.</param>
        /// <returns>List Suggestion.</returns>
        IEnumerable<SuggestionEntity> SuggestAgent(AgentSuggestSearchMemberCriteria criteria);

        /// <summary>
        /// Get members by Admin.
        /// </summary>
        /// <typeparam name="T1">Type output 1.</typeparam>
        /// <typeparam name="T2">Type output 2.</typeparam>
        /// <typeparam name="T3">Type output 3.</typeparam>
        /// <param name="criteria">Get member list criteria.</param>
        /// <returns>Search user result.</returns>
        IDictionary<string, object> GetMembersAdmin<T1, T2, T3>(AdminGetMemberListCriteria criteria);

        /// <summary>
        /// Get members by Agent.
        /// </summary>
        /// <typeparam name="T1">Type output 1.</typeparam>
        /// <typeparam name="T2">Type output 2.</typeparam>
        /// <typeparam name="T3">Type output 3.</typeparam>
        /// <param name="criteria">Get member list criteria.</param>
        /// <returns>Search user result.</returns>
        IDictionary<string, object> GetMembersAgent<T1, T2, T3>(AgentGetMemberListCriteria criteria);

        /// <summary>
        /// Get down line list.
        /// </summary>
        /// <param name="criteria">Down line list criteria.</param>
        /// <returns>List down line.</returns>
        IEnumerable<DownlineEntity> GetDownlines(GetDownlineListCriteria criteria);
    }
}