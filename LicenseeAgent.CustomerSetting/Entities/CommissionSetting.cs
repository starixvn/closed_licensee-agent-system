namespace LicenseeAgent.CustomerSetting.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// CommissionSetting name.
    /// </summary>
    public class CommissionSetting
    {
        public CommissionSetting()
        {
            CommissionItems = new List<CommissionItem>();
        }

        public IList<CommissionItem> CommissionItems { get; set; }

        public string UserGroup { get; set; }
    }
}