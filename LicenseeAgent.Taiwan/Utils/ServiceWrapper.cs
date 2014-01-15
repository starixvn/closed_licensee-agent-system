// ----------------------------------------------------------------------
// <copyright file="ServiceWrapper.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Web;
    using Fanex.Logging;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Taiwan.Actions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class ServiceWrapper
    {
        /// <summary>
        /// Response data when call service.
        /// </summary>
        /// <param name="action">Action of service.</param>
        /// <returns>String response data.</returns>
        public static ResponseResult GetResponseData(IAction action)
        {
            var request = ServiceWrapper.CreateRequest(action.GetActionName());

            ServiceWrapper.SetParams(request, action);

            return ServiceWrapper.MakeRequest(request);
        }

        /// <summary>
        /// Create request.
        /// </summary>
        /// <param name="actionName">Action name for request.</param>
        /// <returns>Request Object.</returns>
        private static HttpWebRequest CreateRequest(string actionName)
        {
            Uri address = new Uri(string.Format(ServiceSetting.UrlAction, AppSettings.TaiwanAPI, actionName));

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
            StringBuilder paramData = new StringBuilder();

            var properties = action.GetType().GetProperties();

            foreach (var property in properties)
            {
                paramData.Append(string.Format(ServiceSetting.ParamFormat, property.Name, HttpUtility.UrlEncode(property.GetValue(action, null).ToString())));
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
        /// <param name="request">Request object.</param>
        /// <returns>String response data after request.</returns>
        private static ResponseResult MakeRequest(HttpWebRequest request)
        {
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(string.Format(ServiceSetting.HttpError, response.StatusCode, response.StatusDescription));
                    }

                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ResponseResult));

                    return jsonSerializer.ReadObject(response.GetResponseStream()) as ResponseResult;
                }
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
            }

            return null;
        }
    }
}
