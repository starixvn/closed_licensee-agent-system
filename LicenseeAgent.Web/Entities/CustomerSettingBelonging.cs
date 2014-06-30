namespace LicenseeAgent.Web.Entities
{
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// Customer settings belonging object.
    /// </summary>
    public class CustomerSettingBelonging
    {
        public FormType FormId { get; set; }

        public int CustId { get; set; }

        public int RoleId { get; set; }

        public int TargetCustId { get; set; }

        public int TargetRoleId { get; set; }
    }
}