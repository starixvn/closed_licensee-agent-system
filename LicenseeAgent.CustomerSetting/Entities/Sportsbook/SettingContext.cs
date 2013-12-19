// --------------------------------------------------------------------------
// <copyright file="SettingContext.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.1">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.12">Update Simple Mode</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Entities.Sportsbook
{
    using System.Diagnostics.CodeAnalysis;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.Web;
    using Newtonsoft.Json;

    /// <summary>
    /// Class name.
    /// </summary>
    public class SettingContext
    {
        public SettingContext()
        {
            var deviceDetection = new DeviceDetection();
            IP = deviceDetection.ClientIp;
            FormId = FormType.Add;
            IsSimpleMode = true;
            Language = Constant.DEFAULT_LANGUAGE;
        }

        public SettingContext(ICurrentSession currentSession)
            : this()
        {
            LoginUserId = currentSession.LoginUserId;
            CustId = currentSession.AgentInfo.AgentId;
            RoleId = currentSession.AgentInfo.IsLevel0 ? (int)Role.AgentLevel0 : (int)Role.Agent;
            TargetRoleId = currentSession.AgentInfo.IsLevel0
                ? (int)Role.AgentLevel1
                : currentSession.AgentInfo.IsLastLevel ? (int)Role.Member : (int)Role.Agent;
            Language = currentSession.Language;
        }

        public int CustId { get; set; }

        public int AgentLevel { get; set; }

        public string UserName { get; set; }

        public int TargetCustId { get; set; }

        public string TargetCustIds { get; set; }

        /// <summary>
        /// Gets or sets the Admin ID.
        /// </summary>
        /// <value>
        /// The creator identifier.
        /// </value>
        [JsonIgnore]
        public int LoginUserId { get; set; }

        public int SubAccId { get; set; }

        public int RoleId { get; set; }

        public string SiteName { get; set; }

        public int TargetRoleId { get; set; }

        public FormType FormId { get; set; }

        public int FormSetting { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public string LangKey { get; set; }

        public bool IsSimpleMode { get; set; }

        public bool HasMinConstrain { get; set; }

        public bool CanEditComGroup { get; set; }

        public bool IsUpdateMember { get; set; }

        public bool IsChangePositionTaking { get; set; }
    }
}