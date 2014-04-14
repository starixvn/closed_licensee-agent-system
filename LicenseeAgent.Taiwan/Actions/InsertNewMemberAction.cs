// ----------------------------------------------------------------------
// <copyright file="InsertNewMemberAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    using System.Xml.Serialization;
    using LicenseeAgent.Taiwan.Attributes;
    using LicenseeAgent.Taiwan.Criterias;

    public class InsertNewMemberAction : LogAction
    {
        public string UserName { get; set; }

        [XmlIgnore]
        public string Password { get; set; }

        [Service]
        [XmlElement(ElementName = "Password")]
        public string EncryptedPassword { get; set; }

        public int IBCCustId { get; set; }

        public string IBCUserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string OtherPhone { get; set; }

        public string Fax { get; set; }

        public int Currency { get; set; }

        public int LASAgentId { get; set; }

        public bool IsClosed { get; set; }

        [Service(IsParamAction = false)]
        public override string ActionName
        {
            get
            {
                return "InsertNewMember";
            }
        }

        public override LogActionCriteria CreateCriteria()
        {
            return new AddNewMemberCriteria(this)
            {
                LicenseeId = LicenseeId,
                UserId = UserId,
                CustId = IBCCustId,
                Doing = GetDoingLogXml()
            };
        }
    }
}