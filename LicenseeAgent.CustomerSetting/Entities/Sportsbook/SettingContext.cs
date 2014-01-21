// --------------------------------------------------------------------------
// <copyright file="SettingContext.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities.Sportsbook
{
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Class name.
    /// </summary>
    public class SettingContext
    {
        public SettingContext()
        {
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

        public SettingContext(bool isLastLevel, bool isLevel0)
            : this()
        {
            LoginUserId = SessionManager.CurrentSession.LoginUserId;
            RoleId = isLevel0 ? (int)Role.AgentLevel0 : (int)Role.Agent;
            TargetRoleId = isLevel0
                ? (int)Role.AgentLevel1
                : isLastLevel ? (int)Role.Member : (int)Role.Agent;
            Language = SessionManager.CurrentSession.Language;
        }

        public int CustId { get; set; }

        public string UserName { get; set; }

        public string TargetCustIds { get; set; }

        public int TargetCustId { get; set; }

        public int TargetRoleId { get; set; }

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

        public FormType FormId { get; set; }

        public int FormSetting { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public string LangKey { get; set; }

        public bool IsSimpleMode { get; set; }

        public bool HasMinConstrain { get; set; }

        public bool CanEditComGroup { get; set; }

        public bool IsOnlyUpdatePT { get; set; }

        public int IndirectAgentId { get; set; }
    }
}