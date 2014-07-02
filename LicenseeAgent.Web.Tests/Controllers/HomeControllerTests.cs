namespace LicenseeAgent.Web.Tests.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Tests;
    using LicenseeAgent.Tests.Fakes;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.ViewModels;
    using NSubstitute;
    using NUnit.Framework;

    /// <summary>
    /// HomeControllerTests class.
    /// </summary>
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controller;

        /// <summary>
        /// Initialize when begin testing.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            FakeSession.InitHttpContext();
            _controller = CreateHomeController();
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
        /// Test Index Action.
        /// </summary>
        [Test]
        public void IndexAction_AgentChangePassword_ReturnHomeViewModel()
        {
            // Arrange
            FakeSession.SetAgentLevel();

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            TestExtensions.ShouldBe<HomeViewModel>(result.Model);
        }

        /// <summary>
        /// Test Index Action.
        /// </summary>
        [Test]
        public void IndexAction_Admin_ReturnHomeViewModel()
        {
            // Arrange
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            TestExtensions.ShouldBe<HomeViewModel>(result.Model);
        }

        /// <summary>
        /// Test Help Action.
        /// </summary>
        [Test]
        public void HelpAction_Should_ReturnViewName()
        {
            // Arrange
            var viewName = "Help";

            // Act
            var result = _controller.Help() as ViewResult;

            // Assert
            TestExtensions.ShouldEqual(viewName, result.ViewName);
        }

        /// <summary>
        /// Test Example Action.
        /// </summary>
        [Test]
        public void ExampleAction_Should_ReturnViewName()
        {
            // Arrange
            var viewName = "Example";

            // Act
            var result = _controller.Help() as ViewResult;

            // Assert
            TestExtensions.ShouldEqual(viewName, result.ViewName);
        }

        /// <summary>
        /// Create Home Controller.
        /// </summary>
        /// <returns> Home controller.</returns>
        private HomeController CreateHomeController()
        {
            var httpContext = Substitute.For<HttpContextBase>();
            var httpRequest = Substitute.For<HttpRequestBase>();
            var httpResponse = Substitute.For<HttpResponseBase>();
            var httpCookies = new HttpCookieCollection { new HttpCookie(Constants.AppCookieTopMenu, string.Empty) };
            var controllerContext = Substitute.For<ControllerContext>();
            var controller = new HomeController();

            httpRequest.Cookies.Returns(httpCookies);
            httpResponse.Cookies.Returns(httpCookies);
            httpContext.Request.Returns(httpRequest);
            httpContext.Response.Returns(httpResponse);

            controllerContext.HttpContext = httpContext;
            controllerContext.RouteData = new RouteData();
            controllerContext.Controller = controller;

            controller.ControllerContext = controllerContext;

            return controller;
        }
    }
}
