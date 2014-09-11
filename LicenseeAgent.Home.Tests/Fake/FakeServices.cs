namespace LicenseeAgent.Home.Tests.Fake
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Home.Criteria.AccountInformation;
    using LicenseeAgent.Home.Entities.AccountInformation;
    using LicenseeAgent.Home.Services;
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
        /// Gets search user service.
        /// </summary>
        /// <value>
        /// Returns search user service.
        /// </value>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static ISearchUserService SearchUserService
        {
            get
            {
                var searchUserService = Substitute.For<ISearchUserService>();

                searchUserService.SuggestByAdmin(Arg.Any<AdminSuggestSearchMemberCriteria>()).Returns(
                    new List<SuggestionEntity>
                {
                    new SuggestionEntity
                    {
                        CustId  = 1,
                        UserName = "Agent1"
                    }
                });

                searchUserService.SuggestByAgent(Arg.Any<AgentSuggestSearchMemberCriteria>()).Returns(
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

                searchUserService.GetAgentCreditBalance(Arg.Any<AgentCreditBalanceCriteria>()).Returns(
                    new BalanceEntity
                    {
                    });

                searchUserService.GetMemberCreditBalance(Arg.Any<MemberCreditBalanceCriteria>()).Returns(
                    new BalanceEntity
                    {
                    });

                return searchUserService;
            }
        }

        /// <summary>
        /// Gets member service.
        /// </summary>
        /// <value>
        /// Returns member service.
        /// </value>
        public static IMemberService MemberService
        {
            get
            {
                return Substitute.For<IMemberService>();
            }
        }

        /// <summary>
        /// Gets down line permission service.
        /// </summary>
        /// <value>
        /// Returns Down line permission service.
        /// </value>
        public static IDownLinePermissionService DownLinePermissionService
        {
            get
            {
                return Substitute.For<IDownLinePermissionService>();
            }
        }

        /// <summary>
        /// Gets core service.
        /// </summary>
        /// <value>
        /// Returns core service.
        /// </value>
        public static ICoreService CoreService
        {
            get
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
}