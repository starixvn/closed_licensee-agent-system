namespace LicenseeAgent.ProductSetting.Entities
{
    using Fanex.Data.Repository;

    /// <summary>
    /// BettingLimitItem name.
    /// </summary>
    public class BettingLimitItem
    {
        [SpParam(SqlParamName = "SportType")]
        public int SportId { get; set; }

        [SpParam(SqlParamName = "BetType")]
        public int BetTypeId { get; set; }

        public int GameTypeId { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MinMaxBet { get; set; }

        public decimal MaxMinBet { get; set; }

        public decimal MinMaxPerMatch { get; set; }

        public decimal MaxPerBall { get; set; }

        public decimal MinMaxPerBall { get; set; }
    }
}