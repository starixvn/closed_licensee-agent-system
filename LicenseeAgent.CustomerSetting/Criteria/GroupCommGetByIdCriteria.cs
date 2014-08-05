namespace LicenseeAgent.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Group Commission Get By ID Criteria class.
    /// </summary>
    public class GroupCommGetByIdCriteria : CriteriaBase
    {
        public int GroupCommId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GroupCommGetById";
        }

        public override bool IsValid()
        {
            return GroupCommId > 0;
        } 
    }
}
