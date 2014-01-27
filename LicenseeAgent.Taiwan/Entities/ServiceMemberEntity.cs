// ----------------------------------------------------------------------
// <copyright file="ServiceMemberEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Entities
{
    using System.Runtime.Serialization;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [DataContract]
    public class ServiceMemberEntity
    {
        [DataMember(Name = "IBCCustID")]
        public int IBCCustId { get; set; }

        [DataMember(Name = "FirstName")]
        [Service(DefaultValue = "---")]
        public string FirstName { get; set; }

        [DataMember(Name = "LastName")]
        [Service(DefaultValue = "---")]
        public string LastName { get; set; }

        [DataMember(Name = "Phone")]
        [Service(DefaultValue = "---")]
        public string Phone { get; set; }

        [DataMember(Name = "MobilePhone")]
        [Service(DefaultValue = "---")]
        public string MobilePhone { get; set; }

        [DataMember(Name = "Fax")]
        [Service(DefaultValue = "---")]
        public string Fax { get; set; }

        [DataMember(Name = "IsClosed")]
        public bool IsClosed { get; set; }

        public byte Status 
        {
            get
            {
                return IsClosed ? (byte)AccountStatus.Closed : (byte)AccountStatus.Open;
            }
        }
    }
}
