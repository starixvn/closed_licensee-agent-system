namespace LicenseeAgent.AccountInformation.Tests.Fake
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data.Repository;
    using LicenseeAgent.AccountInformation.Criteria;
    using LicenseeAgent.AccountInformation.Entities;
    using LicenseeAgent.AccountInformation.Services;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Services;
    using NSubstitute;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class FakeServices
    {
        /// <summary>
        /// Get search user service.
        /// </summary>
        /// <returns>Search user service.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Verified")]
        public static ISearchUserService GetSearchUserService()
        {
            var searchUserService = Substitute.For<ISearchUserService>();

            searchUserService.SuggestAdmin(Arg.Any<AdminSuggestSearchMemberCriteria>()).Returns(
                new List<SuggestionEntity>
                {
                    new SuggestionEntity
                    {
                        CustId  = 1,
                        UserName = "Agent1"
                    }
                });

            searchUserService.SuggestAgent(Arg.Any<AgentSuggestSearchMemberCriteria>()).Returns(
                new List<SuggestionEntity>
                {
                    new SuggestionEntity
                    {
                        CustId  = 2,
                        UserName = "Agent2"
                    }
                });

            searchUserService.GetMembers(Arg.Any<ICriteria>()).Returns(
                    x =>
                    {
                        var customer = new CustomerEntity
                        {
                            UserName = "Customer name"
                        };

                        var upLines = new List<UpLineEntity>()
                        {
                            new UpLineEntity
                            {
                                UserName = "upline"
                            }
                        };
                        var downLines = new List<DownLineEntity>()
                        {
                            new DownLineEntity
                            {
                                UserName = "downline"
                            }
                        };

                        return new SearchMemberEntity
                        {
                            Customer = customer,
                            UpLines = upLines,
                            DownLines = downLines
                        };
                    });

            searchUserService.GetDirectDownlines(Arg.Any<GetDirectDownLineCriteria>()).Returns(
                new List<DownLineEntity>
                {
                    new DownLineEntity
                    {
                        DownlineCustId = 1,
                        Level = 1,
                        UserName = "Downline"
                    }
                });

            searchUserService.GetAgentBalance(Arg.Any<AgentBalanceCriteria>()).Returns(
                new BalanceEntity
                {
                });

            searchUserService.GetMemberBalance(Arg.Any<MemberBalanceCriteria>()).Returns(
                new BalanceEntity
                {
                });

            return searchUserService;
        }

        /// <summary>
        /// Get member service.
        /// </summary>
        /// <returns>Member service.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Verified")]
        public static IMemberService GetMemberService()
        {
            return Substitute.For<IMemberService>();
        }

        /// <summary>
        /// Get down line permission service.
        /// </summary>
        /// <returns>Down line permission service.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Verified")]
        public static IDownLinePermissionService GetDownLinePermissionService()
        {
            return Substitute.For<IDownLinePermissionService>();
        }

        /// <summary>
        /// Get core service.
        /// </summary>
        /// <returns>Core service.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Verified")]
        public static ICoreService GetCoreService()
        {
            var coreService = Substitute.For<ICoreService>();

            coreService.GetProducts(Arg.Any<int>()).Returns(
                    new List<ProductInfo>
                    {
                    });

            return coreService;
        }
    }
}