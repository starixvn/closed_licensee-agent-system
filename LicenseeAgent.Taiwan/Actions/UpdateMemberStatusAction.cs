// ----------------------------------------------------------------------
// <copyright file="UpdateMemberStatusAction.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Actions
{
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Criterias;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Utility;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateMemberStatusAction : LogAction
    {
        public string IBCCustId { get; set; }

        public string LASAgentId { get; set; }

        public bool IsClosed { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "UpdateMemberStatus";
            }
        }

        public override LogActionCriteria CreateCriteria()
        {
            var actionLog = ServiceExtension.CreateUpdateMemberStatusActionLog(IBCCustId, LASAgentId);
            var actionLogXml = ObjectSerializer<UpdateMemberStatusActionLog>.Serialize(actionLog);
            return new ChangeStatusMemberCriteria
            {
                LicenseeId = LicenseeId,
                UserId = UserId,
                LogXml = actionLogXml,
                Doing = GetDoingLogXml()
            };
        }
    }
}