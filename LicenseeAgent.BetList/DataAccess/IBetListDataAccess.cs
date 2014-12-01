namespace LicenseeAgent.BetList.DataAccess
{
    using Entities;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using LicenseeAgent.BetList.Criteria;

    /// <summary>
    /// Interface Account.
    /// </summary>
    public interface IBetListDataAccess
    {
        BetListResult GetBetList(ICriteria criteria);

        SystemParlayData GetSystemParlayResult(SystemParlayDetailsCriteria criteria);
    }
}