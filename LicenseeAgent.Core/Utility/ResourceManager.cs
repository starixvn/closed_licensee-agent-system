// --------------------------------------------------------------------------
// <copyright file="ResourceManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// ResourceManager class.
    /// </summary>
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
            if (errorCode > 0)
            {
                var key = string.Format("Msg_{0}", errorCode);
                result = LangMsg.ResourceManager.GetString(key);
                if (string.IsNullOrEmpty(result))
                {
                    result = string.Format(Las.UndefinedErrorCode, errorCode);
                }
            }
            else
            {
                switch (errorCode)
                {
                    case Result.FailureValue:
                    case Result.InvalidModel:
                    case Result.ApiError:
                        result = Las.HttpError_Default;
                        break;

                    case Result.Unauthorized:
                        result = Las.UnauthorizedMsg;
                        break;

                    case Result.UnderMaintenance:
                        result = Las.UnderMaintenance;
                        break;

                    case Result.AccessDenied:
                        result = Las.AccessDeniedMsg;
                        break;

                    case Result.NoCurrencyPermission:
                        result = Las.AlertNoCurrencyPermission;
                        break;
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