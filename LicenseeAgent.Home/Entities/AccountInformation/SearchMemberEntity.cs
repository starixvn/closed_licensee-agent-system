namespace LicenseeAgent.Home.Entities.AccountInformation
{
    using System.Collections.Generic;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// SearchMemberEntity class.
    /// </summary>
    public class SearchMemberEntity
    {
        public CustomerEntity Customer { get; set; }

        public List<UpLineEntity> UpLines { get; set; }

        public List<DownLineEntity> DownLines { get; set; }
    }
}