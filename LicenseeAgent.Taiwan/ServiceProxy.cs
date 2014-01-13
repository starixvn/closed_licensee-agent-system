// ----------------------------------------------------------------------
// <copyright file="ServiceProxy.cs" company="Nexcel Solutions Vietnam">
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
    using System.Text;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ServiceProxy
    {
        public static string HttpPost(string url, string[] paramName, string[] paramVal)
        {
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";


            Uri address = new Uri("http://api.search.yahoo.com/ContentAnalysisService/V1/termExtraction");

            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            StringBuilder data = new StringBuilder();
            for (int i = 0; i < paramName.Length; i++)
            {
                data.Append(paramName[i]);
                data.Append("=");
                data.Append(HttpUtility.UrlEncode(paramVal[i]));
                data.Append("&");
            }

            byte[] formData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            request.ContentLength = formData.Length;

            using (Stream post = request.GetRequestStream())
            {
                post.Write(formData, 0, formData.Length);
            }

            string result = string.Empty;

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
