namespace LicenseeAgent.Core.Tests
{
    using System.Net;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;
    using NUnit.Framework;

    /// <summary>
    /// Extension Test class.
    /// </summary>
    [TestFixture]
    public class AppHandlerTest
    {
        /// <summary>
        /// Get Http Error Message Test.
        /// </summary>
        [Test]
        public void GetHttpErrorMessage()
        {
            // ARRANGE.
            int forbidden = (int)HttpStatusCode.Forbidden;
            int notFound = (int)HttpStatusCode.NotFound;
            int ok = (int)HttpStatusCode.OK;
            int badRequest = (int)HttpStatusCode.BadRequest;
            
            // ACT.
            // ASSERT.
            Assert.AreEqual(Las.HttpError_403, AppHandler.GetHttpErrorMessage(forbidden), "@Text ABCD Should return Forbidden message");
            Assert.AreEqual(Las.HttpError_404, AppHandler.GetHttpErrorMessage(notFound), "@Text ABCD Should return Not Found message");
            Assert.AreEqual(Las.HttpError_Default, AppHandler.GetHttpErrorMessage(ok), "@Text ABCD Should return Default message");
            Assert.AreEqual(Las.HttpError_Default, AppHandler.GetHttpErrorMessage(badRequest), "@Text ABCD Should return Default message");
        }
    }
}