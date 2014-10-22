namespace LicenseeAgent.Core.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// SelectBalanceUpDayCriteria class.
    /// </summary>
    public class SelectBalanceUpDayCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "LAS_SelBalanceUpDay";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}