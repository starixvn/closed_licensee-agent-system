namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Report.Criteria.AgentWinLoss;
    using LicenseeAgent.Report.Entities.AgentWinLoss;

    /// <summary>
    /// AgentWinLossService class.
    /// </summary>
    public class AgentWinLossService : QueryService, IAgentWinLossService
    {
        public IList<AgentWinLossEntity> GetAgentList(ICriteria criteria)
        {
            return GetEntites<AgentWinLossEntity>(criteria);
        }

        public IList<AgentWinLossEntity> GetMemberList(ICriteria criteria)
        {
            return GetEntites<AgentWinLossEntity>(criteria);
        }

        /// <summary>
        /// Gets the member sum.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns sum entity.</returns>
        public AgentWinLossEntity GetMemberSum(WinLossMemberViewCriteria criteria)
        {
            var list = GetEntites<AgentWinLossEntity>(criteria);
            if (list.Count == 0)
            {
                return null;
            }

            long betCount = 0;
            decimal turnover = 0, winlost = 0, playerComm = 0, memberTotal = 0, awinLost = 0, acomm = 0, atotal = 0;
            decimal downlineStake = 0, downlineTotal = 0, uplineStake = 0, uplineTotal = 0;
            foreach (var item in list)
            {
                betCount += item.BetCount;
                turnover += item.Turnover;
                winlost += item.Winlost;
                playerComm += item.PlayerComm;
                memberTotal += item.MemberTotal;
                awinLost += item.AWinLost;
                acomm += item.AComm;
                atotal += item.ATotal;
                downlineStake += item.DownlineStake;
                downlineTotal += item.DownlineTotal;
                uplineStake += item.UplineStake;
                uplineTotal += item.UplineTotal;
            }

            return new AgentWinLossEntity
            {
                AgentId = 0,
                Username = "Direct Member",
                BetCount = betCount,
                Turnover = turnover,
                Winlost = winlost,
                PlayerComm = playerComm,
                MemberTotal = memberTotal,
                AWinLost = awinLost,
                AComm = acomm,
                ATotal = atotal,
                DownlineStake = downlineStake,
                DownlineTotal = downlineTotal,
                UplineStake = uplineStake,
                UplineTotal = uplineTotal
            };
        }
    }
}