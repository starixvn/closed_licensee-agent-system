namespace LicenseeAgent.Web.Entities
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;

    /// <summary>
    /// ErrorPageInfo class.
    /// </summary>
    public class ErrorPageInfo
    {
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Verifed")]
        public ErrorPageInfo(
            HttpStatusCode statusCode,
            int errCode,
            string errMsg,
            string errPageUrl)
        {
            StatusCode = statusCode;
            ErrCode = errCode;
            ErrMsg = errMsg;
            ErrPageUrl = errPageUrl;
        }

        public HttpStatusCode StatusCode { get; private set; }

        public int ErrCode { get; private set; }

        public string ErrMsg { get; private set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Verifed")]
        public string ErrPageUrl { get; private set; }
    }
}