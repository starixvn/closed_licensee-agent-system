namespace LicenseeAgent.ProductSetting.Criteria.NumberGame
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Agent Position Taking criteria.
    /// </summary>
    public class AgentGetMemberSettingForUpdateCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Las_AgentGetMemberNumberGameSettingForUpdate";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}