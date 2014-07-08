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
        /// Get transfer service.
        /// </summary>
        /// <returns>Transfer service.</returns>
        public static ITransferService GetTransferService()
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

        /// <summary>
        /// Get down line permission service.
        /// </summary>
        /// <returns>Down line permission service.</returns>
        public static IDownLinePermissionService GetDownLinePermissionService()
        {
            return Substitute.For<IDownLinePermissionService>();
        }

        /// <summary>
        /// Get member service.
        /// </summary>
        /// <returns>Member service.</returns>
        public static IMemberService GetMemberService()
        {
            var memberService = Substitute.For<IMemberService>();
            memberService.GetMemberInformations<MemberBalanceInfo>(Arg.Any<List<MemberBalanceInfo>>(), Arg.Any<GetMemberInfoListAction>())
                .Returns(arg =>
                    {
                        return new ResponseResult(1, "Fail.");
                    });

            return memberService;
        }
    }
}