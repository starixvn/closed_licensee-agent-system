// ----------------------------------------------------------------------
// <copyright file="ServiceWrapper.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web;
    using Fanex.Logging;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Results;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal static class ServiceWrapper
    {
        /// <summary>
        /// Response data when call service.
        /// </summary>
        /// <typeparam name="T">Type response result.</typeparam>
        /// <param name="action">Action of service.</param>
        /// <returns>String response data.</returns>
        public static T GetResponseData<T>(IAction action) where T : class
        {
            var request = ServiceWrapper.CreateRequest(action.GetActionName());
            ServiceWrapper.SetParams(request, action);

            return ServiceWrapper.MakeRequest<T>(request);
        }

        /// <summary>
        /// Create request.
        /// </summary>
        /// <param name="actionName">Action name for request.</param>
        /// <returns>Request Object.</returns>
        private static HttpWebRequest CreateRequest(string actionName)
        {
            var address = new Uri(string.Format(ServiceSetting.UrlAction, AppSettings.TaiwanAPI, actionName));
            var request = WebRequest.Create(address) as HttpWebRequest;

            request.Method = ServiceSetting.Method;
            request.ContentType = ServiceSetting.ContentType;

            return request;
        }

        /// <summary>
        /// Set parameter for action request.
        /// </summary>
        /// <param name="request">Request object.</param>
        /// <param name="action">Action service.</param>
        private static void SetParams(HttpWebRequest request, IAction action)
        {
            var paramData = new StringBuilder();
            var properties = action.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(action, null);
                var attribute = property.GetCustomAttributes(typeof(ServiceAttribute), true).FirstOrDefault();

                if (!ServiceWrapper.IsParamRequest(attribute))
                {
                    continue;
                }

                if (propertyValue != null)
                {
                    paramData.Append(string.Format(
                                                    ServiceSetting.ParamFormat,
                                                    attribute != null ? ((ServiceAttribute)attribute).Name : property.Name,
                                                    HttpUtility.UrlEncode(propertyValue.ToString())));
                }
            }

            byte[] formData = UTF8Encoding.UTF8.GetBytes(paramData.ToString(0, paramData.Length - 1));
            request.ContentLength = formData.Length;

            using (Stream post = request.GetRequestStream())
            {
                post.Write(formData, 0, formData.Length);
            }
        }

        /// <summary>
        /// Make request service.
        /// </summary>
        /// <typeparam name="T">Type response result.</typeparam>
        /// <param name="request">Request object.</param>
        /// <returns>String response data after request.</returns>
        private static T MakeRequest<T>(HttpWebRequest request) where T : class
        {
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new InvalidOperationException(string.Format(ServiceSetting.HttpError, response.StatusCode, response.StatusDescription));
                    }

                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));

                    return jsonSerializer.ReadObject(response.GetResponseStream()) as T;
                }
            }
            catch (InvalidOperationException exception)
            {
                LogService.Error(exception.Message, exception);
            }

            return null;
        }

        private static bool IsParamRequest(object attribute)
        {
            if (attribute != null)
            {
                return ((ServiceAttribute)attribute).IsParamAction;
            }

            return true;
        }
    }
}
