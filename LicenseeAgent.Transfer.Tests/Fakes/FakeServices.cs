namespace LicenseeAgent.Transfer.Tests.Fakes
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Results;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Transfer.Criteria;
    using LicenseeAgent.Transfer.Entities;
    using LicenseeAgent.Transfer.Services;
    using LicenseeAgent.Web.Services;
    using NSubstitute;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class FakeServices
    {
        /// <summary>
        /// Gets transfer service.
        /// </summary>
        /// <value>
        /// Returns transfer service.
        /// </value>
        public static ITransferService TransferService
        {
            get
            {
                var transferService = Substitute.For<ITransferService>();

                transferService.GetAgentBalanceInfo(Arg.Any<GetAgentBalanceInfoCriteria>()).Returns(
                    new List<AgentBalanceInfo>
                {
                    new AgentBalanceInfo
                    {
                        AgentId  = 1,
                        UserName = "Agent1"
                    }
                });

                transferService.GetMemberBalanceInfo(Arg.Any<GetMemberBalanceInfoCriteria>()).Returns(
                    new List<MemberBalanceInfo>
                {
                    new MemberBalanceInfo
                    {
                        CustId  = 1,
                        UserName = "Member1"
                    }
                });

                return transferService;
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
                coreService.HasProductOffered(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
                return coreService;
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
                memberService.GetMemberInfoList<MemberBalanceInfo>(Arg.Any<List<MemberBalanceInfo>>(), Arg.Any<GetMemberInfoListAction>())
                    .Returns(arg =>
                        {
                            return new ResponseResult(1, "Fail.");
                        });

                return memberService;
            }
        }
    }
}