// -----------------------------------------------------------------------
// <copyright file="FakeServices.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Tests.Fake
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.AccountInformation.Criteria;
    using LicenseeAgent.AccountInformation.Entities;
    using LicenseeAgent.AccountInformation.Services;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Results;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Services;
    using NSubstitute;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FakeServices
    {
        /// <summary>
        /// Get search user service.
        /// </summary>
        /// <returns>Search user service.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static ISearchUserService GetSearchUserService()
        {
            var searchUserService = Substitute.For<ISearchUserService>();

            searchUserService.SuggestAdmin(Arg.Any<AdminSuggestSearchMemberCriteria>())
                .Returns(new List<SuggestionEntity> 
                        { 
                            new SuggestionEntity
                            { 
                                CustId  = 1, 
                                UserName = "Agent1"
                            },
                            new SuggestionEntity
                            { 
                                CustId  = 1, 
                                UserName = "Agent1"
                            }
                        });

            searchUserService.SuggestAgent(Arg.Any<AgentSuggestSearchMemberCriteria>())
                        .Returns(new List<SuggestionEntity> 
                                    { 
                                        new SuggestionEntity
                                        { 
                                            CustId  = 2, 
                                            UserName = "Agent2"
                                        }
                                    });

            searchUserService.GetMembers<CustomerEntity, UpLineEntity, DownLineEntity>(Arg.Any<ICriteria>())
                    .Returns(args => 
                        { 
                            var result = new Dictionary<string, object>();
                            result.Add(
                                    "CustomerEntity", 
                                    new List<CustomerEntity>()
                                    {
                                        new CustomerEntity { UserName = "Customer name" }
                                    });
                            result.Add(
                                    "UpLineEntity", 
                                    new List<UpLineEntity>()
                                    {
                                        new UpLineEntity { UserName = "upline" }
                                    });
                            result.Add(
                                    "DownLineEntity", 
                                    new List<DownLineEntity>()
                                    {
                                        new DownLineEntity { UserName = "downline" }
                                    });

                            return result;
                        });

            searchUserService.GetDirectDownlines(Arg.Any<GetDirectDownLineCriteria>())
                        .Returns(new List<DownLineEntity> 
                            { 
                                new DownLineEntity
                                { 
                                    DownlineCustId = 1, 
                                    Level = 1, 
                                    UserName = "Downline"
                                }
                            });

            searchUserService.GetAgentBalance(Arg.Any<AgentBalanceCriteria>())
                    .Returns(new BalanceEntity());

            searchUserService.GetMemberBalance(Arg.Any<MemberBalanceCriteria>())
                    .Returns(new BalanceEntity());

            return searchUserService;
        }

        /// <summary>
        /// Get member service.
        /// </summary>
        /// <returns>Member service.</returns>
        public static IMemberService GetMemberService()
        {
            var memberService = Substitute.For<IMemberService>();
            memberService.GetMemberInformation<CustomerEntity>(Arg.Any<CustomerEntity>(), Arg.Any<GetMemberInfoAction>())
                    .Returns(args =>
                        {
                            var customer = args[0] as CustomerEntity;

                            customer.FirstName = "First";
                            customer.LastName = "Last";
                            customer.Phone = "123456";
                            customer.Mobile = "123456";
                            customer.Fax = "123456789";

                            return new ResponseResult(1, "Fail.");
                        });

            return memberService;
        }

        /// <summary>
        /// Get down line permission service.
        /// </summary>
        /// <returns>Down line permission service.</returns>
        public static IDownlLinePermissionService GetDownlLinePermissionService()
        {
            return Substitute.For<IDownlLinePermissionService>();
        }

        /// <summary>
        /// Get core service.
        /// </summary>
        /// <returns>Core service.</returns>
        public static ICoreService GetCoreService()
        {
            var coreService = Substitute.For<ICoreService>();

            coreService.HasProductOffered(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

            return coreService;
        }
    }
}
