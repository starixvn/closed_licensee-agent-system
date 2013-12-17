// --------------------------------------------------------------------------
// <copyright file="ResourceManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Core.Utils
{
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ResourceManager
    {
        public static string GetErrorMsg(int msgId)
        {
            if (msgId != 0)
            {
                var key = string.Format("Msg_{0}", msgId);
                return LangMsg.ResourceManager.GetString(key);
            }

            return string.Empty;
        }

        public static string GetErrorMsg(MsgId msgId)
        {
            return GetErrorMsg((int)msgId);
        }
    }
}