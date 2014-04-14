// --------------------------------------------------------------------------
// <copyright file="ResourceManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;

    public static class ResourceManager
    {
        /// <summary>
        /// Gets the error MSG.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <returns>Returns error message.</returns>
        public static string GetErrMsg(int errorCode)
        {
            var result = string.Empty;
            if (errorCode != 0)
            {
                var key = string.Format("Msg_{0}", errorCode);
                result = LangMsg.ResourceManager.GetString(key);
                if (string.IsNullOrEmpty(result))
                {
                    result = string.Format(Las.UndefinedErrorCode, errorCode);
                }
            }

            return result;
        }

        public static string GetErrMsg(ErrCode msgId)
        {
            return GetErrMsg((int)msgId);
        }
    }
}