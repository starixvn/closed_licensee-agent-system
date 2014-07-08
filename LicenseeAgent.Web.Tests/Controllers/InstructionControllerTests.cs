namespace LicenseeAgent.Web.Tests.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Tests.Fakes;
    using LicenseeAgent.Web.Controllers;
    using NUnit.Framework;

    /// <summary>
    /// InstructionControllerTests class.
    /// </summary>
    [TestFixture]
    public class InstructionControllerTests
    {
        private InstructionController _controller;

        /// <summary>
        /// Initialize when begin testing.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            FakeSession.InitHttpContext();
            _controller = CreateInstructionController();
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
        /// Index action.
        /// </summary>
        [Test]
        public void IndexAction_Should_ReturnJsonResult()
        {
            // Arrange
            var key = "Commission";
            var language = "en-US";
            var viewName = string.Format("~/Views/Instruction/{0}/{1}.cshtml", language, key);
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Index(key) as ViewResult;

            // Assert
            Assert.AreEqual(viewName, result.ViewName);
        }

        /// <summary>
        /// Create Instruction Controller.
        /// </summary>
        /// <returns> Instruction controller.</returns>
        private InstructionController CreateInstructionController()
        {
            return new InstructionController();
        }
    }
}