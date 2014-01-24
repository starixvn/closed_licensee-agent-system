// --------------------------------------------------------------------------
// <copyright file="ResourceManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ResourceManager
    {
        public static string GetErrMsg(int errorCode)
        {
            if (errorCode != 0)
            {
                var key = string.Format("Msg_{0}", errorCode);
                return LangMsg.ResourceManager.GetString(key);
            }

            return string.Empty;
        }

        public static string GetErrMsg(ErrCode msgId)
        {
            return GetErrMsg((int)msgId);
        }
    }
}