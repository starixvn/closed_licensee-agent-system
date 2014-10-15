namespace LicenseeAgent.Home.Entities.AccountInformation
{
    using LicenseeAgent.Core.Configurations;

    /// <summary>
    /// UpLineEntity class.
    /// </summary>
    public class UpLineEntity
    {
        public int UplineCustId { get; set; }

        public string UserName { get; set; }

        public int Level { get; set; }

        public string ShortUserName
        {
            get
            {
                if (UserName.Length > Constants.SmallStringLength)
                {
                    return string.Format("{0}...", UserName.Substring(0, Constants.SmallStringLength));
                }

                return UserName;
            }
        }

        public string MediumUserName
        {
            get
            {
                if (UserName.Length > Constants.MediumStringLength)
                {
                    return string.Format("{0}...", UserName.Substring(0, Constants.MediumStringLength));
                }

                return UserName;
            }
        }
    }
}