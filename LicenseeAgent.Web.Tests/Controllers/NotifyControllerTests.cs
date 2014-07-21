namespace LicenseeAgent.Web.Tests.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Tests.Fakes;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Tests.Fakes;
    using NUnit.Framework;

    /// <summary>
    /// NotifyControllerTests class.
    /// </summary>
    [TestFixture]
    public class NotifyControllerTests
    {
        private NotifyController _controller;

        /// <summary>
        /// Initialize when begin testing.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            FakeSession.InitHttpContext();
            _controller = CreateNotifyController();
        }

        /// <summary>
        /// Release resource.
        /// </summary>
        [TearDown]
        public void Dispose()
        {
            _controller.Dispose();
            _controller = null;
        }

        /// <summary>
        /// Under maintenance action.
        /// </summary>
        [Test]
        public void UnderMaintenanceAction_Should_ReturnJsonResult()
        {
            // Arrange

            // Act
            var result = _controller.UnderMaintenance();

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Create Notify Controller.
        /// </summary>
        /// <returns> Notify controller.</returns>
        private NotifyController CreateNotifyController()
        {
            var underMaintenanceService = FakeServices.UnderMaintenanceService;
            var controller = new NotifyController(underMaintenanceService);

            return controller;
        }
    }
}