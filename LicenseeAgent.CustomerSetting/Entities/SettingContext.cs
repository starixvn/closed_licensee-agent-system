// --------------------------------------------------------------------------
// <copyright file="SettingContext.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities
{
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.Web;

    /// <summary>
    /// Class name.
    /// </summary>
    public class SettingContext
    {
        public SettingContext()
        {
            FormId = FormType.Add;
            IsSimpleMode = true;
            Language = Constant.DefaultLanguage;
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

        public string UserName { get; set; }

        public string TargetCustIds { get; set; }

        public int TargetCustId { get; set; }

        public int TargetRoleId { get; set; }

        public int LoginUserId { get; set; }

        public int RoleId { get; set; }

        public FormType FormId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public bool IsSimpleMode { get; set; }
    }
}