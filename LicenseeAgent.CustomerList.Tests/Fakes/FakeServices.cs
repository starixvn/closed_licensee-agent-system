namespace LicenseeAgent.CustomerList.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerList.Criteria.Agent;
    using LicenseeAgent.CustomerList.Criteria.Member;
    using LicenseeAgent.CustomerList.Entities.Agent;
    using LicenseeAgent.CustomerList.Entities.Member;
    using LicenseeAgent.CustomerList.Services;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Results;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Criteria.Common;
    using LicenseeAgent.Web.Entities.Common;
    using LicenseeAgent.Web.Services;
    using NSubstitute;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class FakeServices
    {
        /// <summary>
        /// Gets customer service.
        /// </summary>
        /// <value>
        /// Returns customer service.
        /// </value>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static IAgentListService AgentListService
        {
            get
            {
                var agentService = Substitute.For<IAgentListService>();

                var agents = new List<AgentEntity>
                {
                    new AgentEntity { AgentId = 101, AgentLevel = 2, UserName = "AgentAAA1" },
                    new AgentEntity { AgentId = 102, AgentLevel = 2, UserName = "AgentAAA2" },
                    new AgentEntity { AgentId = 103, AgentLevel = 2, UserName = "AgentBBB1" },
                    new AgentEntity { AgentId = 104, AgentLevel = 2, UserName = "AgentBBB2" },
                    new AgentEntity { AgentId = 105, AgentLevel = 2, UserName = "AgentCCC" }
                };

                var failureAgents = new List<AgentEntity>
                {
                    new AgentEntity { AgentId = 201, AgentLevel = 2, UserName = "AgentAAA1" },
                    new AgentEntity { AgentId = 202, AgentLevel = 2, UserName = "AgentAAA2" }
                };

                agentService.GetAgentList(Arg.Any<AgentGetByAgentCriteria>()).Returns(agents);
                agentService.SearchAgent(Arg.Any<SearchAgentCriteria>()).Returns(agents);
                agentService.UpdateAgentAccStatus(Arg.Is<UpdateAgentStatusCriteria>(x => x.AgentId == 100)).Returns(agents);
                agentService.UpdateAgentAccStatus(Arg.Is<UpdateAgentStatusCriteria>(x => x.AgentId != 100)).Returns(failureAgents);
                agentService.GetAgentAutoComplete(Arg.Any<AgentAutoCompleteCriteria>()).Returns(agents);
                agentService.When(x => x.ResetPassword(Arg.Is<ResetPasswordAgentCriteria>(c => c.AgentId != 100)))
                                .Do(args =>
                                {
                                    var criteria = (ResetPasswordAgentCriteria)args[0];
                                    criteria.ErrCode = Result.FailureValue;
                                });
                agentService.GetAgentInfo(Arg.Any<AgentInfoCriteria>()).Returns(new AgentEntity { AgentId = 101, AgentLevel = 2, UserName = "AgentAAA1" });
                agentService.When(x => x.EditInfo(Arg.Is<EditinfoAgentCriteria>(c => c.AgentId != 100)))
                                .Do(args =>
                                {
                                    var criteria = (EditinfoAgentCriteria)args[0];
                                    criteria.ErrCode = Result.FailureValue;
                                });

                return agentService;
            }
        }

        public static IMemberListService MemberListService
        {
            get
            {
                var memberListService = Substitute.For<IMemberListService>();

                var members = new List<MemberEntity>
                {
                    new MemberEntity { AgentId = 101, CustId = 100, Status = 1 },
                    new MemberEntity { AgentId = 101, CustId = 101, Status = 1 }
                };

                memberListService.GetMemberList(Arg.Any<MemberGetByAgentCriteria>())
                    .Returns(args =>
                    {
                        var criteria = (MemberGetByAgentCriteria)args[0];
                        criteria.MsgId = Result.SuccessValue;

                        return members;
                    });
                memberListService.SearchMember(Arg.Any<SearchMemberCriteria>())
                    .Returns(args =>
                    {
                        var criteria = (SearchMemberCriteria)args[0];
                        criteria.MsgId = Result.SuccessValue;

                        return members;
                    });

                memberListService.When(x => x.CheckMemberUplineStatusForUpdate(Arg.Is<CheckMemberUpLineStatusForUpdateCriteria>(c => c.CustId != 1000)))
                    .Do(args =>
                    {
                        var criteria = (CheckMemberUpLineStatusForUpdateCriteria)args[0];
                        criteria.ErrCode = Result.FailureValue;
                    });
                memberListService.GetMemberAutoComplete(Arg.Any<MemberAutoCompleteCriteria>()).Returns(members);

                return memberListService;
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
                var memberService = Substitute.For<IMemberService>();
                memberService.GetMemberInformations(Arg.Any<IEnumerable<MemberEntity>>(), Arg.Is<GetMemberInfoListAction>(x => x.LASAgentId == 100))
                                .Returns(new ResponseResult(0, "Success"));
                memberService.GetMemberInformations(Arg.Any<IEnumerable<MemberEntity>>(), Arg.Is<GetMemberInfoListAction>(x => x.LASAgentId != 100))
                                .Returns(new ResponseResult(1, "Fail"));
                memberService.UpdateMemberStatus(Arg.Is<UpdateMemberStatusAction>(x => x.LASAgentId.IndexOf("101") >= 0))
                                .Returns(new ResponseResult(0, "Success"));
                memberService.UpdateMemberStatus(Arg.Is<UpdateMemberStatusAction>(x => x.LASAgentId.IndexOf("101") < 0))
                                .Returns(new ResponseResult(1, "Fail"));
                memberService.ResetPassword(Arg.Is<ResetPasswordAction>(x => x.IBCCustId == 1000))
                                .Returns(new ResponseResult(0, "Success"));
                memberService.ResetPassword(Arg.Is<ResetPasswordAction>(x => x.IBCCustId != 1000))
                                .Returns(new ResponseResult(1, "Fail"));
                memberService.UpdateMemberInfo(Arg.Is<UpdateMemberInfoAction>(x => x.IBCCustId == 1000))
                                .Returns(new ResponseResult(0, "Success"));
                memberService.UpdateMemberInfo(Arg.Is<UpdateMemberInfoAction>(x => x.IBCCustId != 1000))
                                .Returns(new ResponseResult(1, "Fail"));

                return memberService;
            }
        }

        /// <summary>
        /// Gets down line service.
        /// </summary>
        /// <value>
        /// Returns Down line service.
        /// </value>
        public static IDownLinePermissionService DownLineService
        {
            get
            {
                var downlineService = Substitute.For<IDownLinePermissionService>();

                downlineService.IsDownLineAgent(Arg.Is<int>(x => x == 100 || x == 200)).Returns(true);
                downlineService.IsDownLineMember(Arg.Is<int>(x => x == 1000 || x == 2000)).Returns(true);
                downlineService.IsDirectDownLineAgent(Arg.Is<int>(x => x == 100 || x == 200)).Returns(true);
                downlineService.IsDirectDownLineMember(Arg.Is<int>(x => x == 1000 || x == 2000)).Returns(true);

                return downlineService;
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

                coreService.GetProducts(Arg.Any<int>())
                    .Returns(new List<ProductInfo> { new ProductInfo { ProductId = 1, ProductName = "Sportsbook" } });
                coreService.HasProductOffered(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

                return coreService;
            }
        }
    }
}