// -----------------------------------------------------------------------
// <copyright file="FakeServices.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerList.Criteria;
    using LicenseeAgent.CustomerList.Entities;
    using LicenseeAgent.CustomerList.Services;
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
        /// Get customer service.
        /// </summary>
        /// <returns>Customer service.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static ICustomerService GetCustomerService()
        {
            var customerService = Substitute.For<ICustomerService>();

            var agents = new List<AgentEntity> 
            { 
                new AgentEntity { AgentId = 101, AgentLevel = 2, UserName = "AgentAAA1" },
                new AgentEntity { AgentId = 102, AgentLevel = 2, UserName = "AgentAAA2" },
                new AgentEntity { AgentId = 103, AgentLevel = 2, UserName = "AgentBBB1" },
                new AgentEntity { AgentId = 104, AgentLevel = 2, UserName = "AgentBBB2" },
                new AgentEntity { AgentId = 105, AgentLevel = 2, UserName = "AgentCCC" }
            };
            var members = new List<MemberEntity>
            {
                new MemberEntity { AgentId = 101, CustId = 100, Status = 1 },
                new MemberEntity { AgentId = 101, CustId = 101, Status = 1 }
            };

            var failureAgents = new List<AgentEntity> 
            { 
                new AgentEntity { AgentId = 201, AgentLevel = 2, UserName = "AgentAAA1" },
                new AgentEntity { AgentId = 202, AgentLevel = 2, UserName = "AgentAAA2" }
            };

            customerService.GetAgentList(Arg.Any<AgentGetByAgentCriteria>()).Returns(agents);
            customerService.SearchAgent(Arg.Any<SearchAgentCriteria>()).Returns(agents);
            customerService.GetMemberList(Arg.Any<MemberGetByAgentCriteria>())
                        .Returns(args => 
                        { 
                            var criteria = (MemberGetByAgentCriteria)args[0];
                            criteria.MsgId = Result.SuccessValue;

                            return members; 
                        });
            customerService.SearchMember(Arg.Any<SearchMemberCriteria>())
                        .Returns(args => 
                        { 
                            var criteria = (SearchMemberCriteria)args[0];
                            criteria.MsgId = Result.SuccessValue;

                            return members; 
                        });

            customerService.UpdateAgentAccStatus(Arg.Is<UpdateAgentStatusCriteria>(x => x.AgentId == 100)).Returns(agents);
            customerService.UpdateAgentAccStatus(Arg.Is<UpdateAgentStatusCriteria>(x => x.AgentId != 100)).Returns(failureAgents);
            customerService.When(x => x.CheckMemberUplineStatusForUpdate(Arg.Is<CheckMemberUpLineStatusForUpdateCriteria>(c => c.CustId != 1000)))
                            .Do(args =>
                            {
                                var criteria = (CheckMemberUpLineStatusForUpdateCriteria)args[0];
                                criteria.ErrCode = Result.FailureValue;
                            });
            customerService.GetAgentAutoComplete(Arg.Any<AgentAutoCompleteCriteria>()).Returns(agents);
            customerService.GetMemberAutoComplete(Arg.Any<MemberAutoCompleteCriteria>()).Returns(members);
            
            customerService.When(x => x.ResetPassword(Arg.Is<ResetPasswordCriteria>(c => c.AgentId != 100)))
                            .Do(args =>
                            {
                                var criteria = (ResetPasswordCriteria)args[0];
                                criteria.ErrCode = Result.FailureValue;
                            });
            customerService.GetAgentInfo(Arg.Any<AgentInfoCriteria>()).Returns(new AgentEntity { AgentId = 101, AgentLevel = 2, UserName = "AgentAAA1" });
            customerService.When(x => x.EditInfoAgent(Arg.Is<EditinfoAgentCriteria>(c => c.AgentId != 100)))
                            .Do(args =>
                            {
                                var criteria = (EditinfoAgentCriteria)args[0];
                                criteria.ErrCode = Result.FailureValue;
                            });

            return customerService;
        }

        /// <summary>
        /// Get IP information service.
        /// </summary>
        /// <returns>IP information service.</returns>
        public static IIpInformationService GetIpInformationService()
        {
            var ipInformationService = Substitute.For<IIpInformationService>();

            ipInformationService.GetIpInformation(Arg.Any<GetIpInformationCriteria>())
                            .Returns(args => 
                            {
                                return new List<IpEntity>
                                {
                                    new IpEntity { IpCity = "Ho Chi Minh City", CountryLong = "VietNam" }
                                };
                            });

            return ipInformationService;
        }

        /// <summary>
        /// Get member service.
        /// </summary>
        /// <returns>Member service.</returns>
        public static IMemberService GetMemberService()
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

        /// <summary>
        /// Get down line service.
        /// </summary>
        /// <returns>Down line service.</returns>
        public static IDownlLinePermissionService GetDownlineService()
        {
            var downlineService = Substitute.For<IDownlLinePermissionService>();

            downlineService.IsDownLineAgent(Arg.Is<int>(x => x == 100 || x == 200)).Returns(true);
            downlineService.IsDownLineMember(Arg.Is<int>(x => x == 1000 || x == 2000)).Returns(true);
            downlineService.IsDirectDownLineAgent(Arg.Is<int>(x => x == 100 || x == 200)).Returns(true);
            downlineService.IsDirectDownLineMember(Arg.Is<int>(x => x == 1000 || x == 2000)).Returns(true);

            return downlineService;
        }

        /// <summary>
        /// Get core service.
        /// </summary>
        /// <returns>Core service.</returns>
        public static ICoreService GetCoreService()
        {
            var coreService = Substitute.For<ICoreService>();

            coreService.GetProducts(Arg.Any<int>())
                .Returns(new List<ProductInfo> { new ProductInfo { ProductId = 1, ProductName = "Sportsbook" } });
            coreService.HasProductOffered(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

            return coreService;
        }
    }
}
