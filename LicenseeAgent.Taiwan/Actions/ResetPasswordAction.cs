// ----------------------------------------------------------------------
// <copyright file="ResetPasswordAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    using System.Xml.Serialization;
    using LicenseeAgent.Taiwan.Attributes;
    using LicenseeAgent.Taiwan.Criteria;

    /// <summary>
    /// ResetPasswordAction class.
    /// </summary>
    public class ResetPasswordAction : LogAction
    {
        public int IBCCustId { get; set; }

        [XmlIgnore]
        public string Password { get; set; }

        [Service]
        [XmlElement(ElementName = "Password")]
        public string EncryptedPassword { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "ResetPassword";
            }
        }

        public override LogActionCriteria CreateCriteria()
        {
            return new ResetPasswordMemberCriteria(this)
            {
                LicenseeId = LicenseeId,
                UserId = UserId,
                CustId = IBCCustId,
                Doing = GetDoingLogXml()
            };
        }
    }
}