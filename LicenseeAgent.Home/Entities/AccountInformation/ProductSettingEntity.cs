namespace LicenseeAgent.Home.Entities.AccountInformation
{
    /// <summary>
    /// ProductSettingEntity class.
    /// </summary>
    public class ProductSettingEntity
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public bool Disable { get; set; }

        public bool UplineDisable { get; set; }
    }
}