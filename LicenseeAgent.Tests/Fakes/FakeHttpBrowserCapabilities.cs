namespace LicenseeAgent.Tests.Fakes
{
    using System;
    using System.Web;

    /// <summary>
    /// FakeHttpBrowserCapabilities class.
    /// </summary>
    public class FakeHttpBrowserCapabilities : HttpBrowserCapabilitiesBase
    {
        public override string Browser
        {
            get
            {
                return "Chrome";
            }
        }

        public override string Version
        {
            get
            {
                return "35.0";
            }
        }

        public override bool IsMobileDevice
        {
            get
            {
                return false;
            }
        }

        public override string Platform
        {
            get
            {
                return Environment.OSVersion.Platform.ToString();
            }
        }
    }
}