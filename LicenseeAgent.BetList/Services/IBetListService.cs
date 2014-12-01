namespace LicenseeAgent.BetList.Services
{
    using System.Collections.Generic;
    using Entities;
    using Fanex.BetList.Core.Entities;
    using Fanex.Result.Core.DTO;
    using LicenseeAgent.BetList.Criteria;

    public interface IBetListService
    {
        BetListResult GetByMember(SelectByMemberCriteria criteria);

        BetListResult GetTotalBets(TotalBetsCriteria criteria);

        BetListResult GetOutstanding(MemberOutstandingCriteria criteria);

        BetListResult GetOutright(OutrightCriteria criteria);

        BetListResult GetMixParlayDetails(MixParlayDetailsCriteria criteria);

        SystemParlayData GetSystemParlayResult(SystemParlayDetailsCriteria criteria);

        IList<NumberGame> GetNumberGameResults(int matchId);

        IList<Racing> GetVirtualRacingResults(int matchId, string raceNo);

        IList<Sportsbook> GetSportsBookResults(int matchId);

        IList<TicketDetail> GetDetails(TicketDetailsCriteria criteria);

        BetListResult GetParlay(TotalBetsParlayCriteria criteria);

        IList<Parlay> GetParlayResults(ResultsParlayCriteria criteria);
    }
}