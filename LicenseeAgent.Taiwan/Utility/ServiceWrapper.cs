// ----------------------------------------------------------------------
// <copyright file="ServiceWrapper.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Utility
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Attributes;
    using LicenseeAgent.Taiwan.Results;

    /// <summary>
    /// ServiceWrapper class.
    /// </summary>
    internal static class ServiceWrapper
    {
        /// <summary>
        /// Response data when call service.
        /// </summary>
        /// <typeparam name="T">Type response result.</typeparam>
        /// <param name="action">Action of service.</param>
        /// <returns>
        /// String response data.
        /// </returns>
        public static T GetActionResponse<T>(BaseAction action) where T : ResponseResult
        {
            var webRequest = CreateActionRequest(action);
            using (var response = webRequest.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var errorMessage = string.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription);
                    throw new InvalidOperationException(errorMessage);
                }

                var jsonSerializer = new DataContractJsonSerializer(typeof(T));
                var responseResult = jsonSerializer.ReadObject(response.GetResponseStream()) as T;
                responseResult.ApiErrCode = responseResult.ErrCode;
                responseResult.ApiErrMsg = responseResult.ErrMsg;
                if (responseResult.ErrCode != Result.SuccessValue)
                {
                    responseResult.ErrCode = Result.ApiError;
                    responseResult.ErrMsg = ApiMsg.ResourceManager.GetString(string.Format("{0}_{1}", action.ActionName, responseResult.ApiErrCode)) ?? Las.HttpError_Default;
                }

                return responseResult;
            }
        }

        /// <summary>
        /// Create request.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>
        /// Request Object.
        /// </returns>
        private static WebRequest CreateActionRequest(BaseAction action)
        {
            var actionUrl = new Uri(string.Format("{0}/{1}", AppSettings.TaiwanAPI, action.ActionName));
            var webRequest = WebRequest.Create(actionUrl);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";

            var paramData = new StringBuilder();
            var properties = action.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(action, null);
                var attribute = property.GetCustomAttributes(typeof(ServiceAttribute), true).FirstOrDefault();
                if (!ServiceExtension.IsParamRequest(attribute))
                {
                    continue;
                }

                if (propertyValue != null)
                {
                    var param = string.Format(
                        "{0}={1}&",
                        attribute != null ? ((ServiceAttribute)attribute).Name : property.Name,
                        HttpUtility.UrlEncode(propertyValue.ToString()));
                    paramData.Append(param);
                }
            }

            byte[] formData = Encoding.UTF8.GetBytes(paramData.ToString(0, paramData.Length - 1));
            webRequest.ContentLength = formData.Length;
            using (var requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
            }

            return webRequest;
        }
    }
}