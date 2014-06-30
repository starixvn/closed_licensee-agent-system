// -----------------------------------------------------------------------
// <copyright file="AccountInformationControllersTests.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Tests.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using LicenseeAgent.AccountInformation.Controllers;
    using LicenseeAgent.AccountInformation.Entities;
    using LicenseeAgent.AccountInformation.Tests.Fake;
    using LicenseeAgent.AccountInformation.ViewModels;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;
    using NUnit.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AccountInformationControllersTests
    {
        /// <summary>
        /// Initialize when begin testing.
        /// </summary>
        [SetUp]
        public void Init()
        {
            var httpRequest = new HttpRequest(string.Empty, "http://localhost/", string.Empty);
            var httpResponce = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(httpRequest, httpResponce);
            var sessionContainer = new HttpSessionStateContainer(
                                                "id",
                                               new SessionStateItemCollection(),
                                               new HttpStaticObjectsCollection(),
                                               10,
                                               true,
                                               HttpCookieMode.AutoDetect,
                                               SessionStateMode.InProc,
                                               false);
            httpContext.Items["AspSession"] = typeof(HttpSessionState)
                                                .GetConstructor(
                                                                    BindingFlags.NonPublic | BindingFlags.Instance,
                                                                    null,
                                                                    CallingConventions.Standard,
                                                                    new[] { typeof(HttpSessionStateContainer) },
                                                                    null)
                                                .Invoke(new object[] { sessionContainer });
            HttpContext.Current = httpContext;

            SessionManager.CurrentSession = new CurrentSession(
                                                new AgentInfo
                                                {
                                                    AgentLevel = 1,
                                                    CurrencyId = 20,
                                                    CurrencyName = "UUS",
                                                    LicenseeId = 1,
                                                    LicenseeName = "yq355",
                                                    NumberOfLevel = 3
                                                },
                                                0,
                                                "en-US");
        }

        /// <summary>
        /// Test when load page.
        /// </summary>
        [Test]
        public void IndexAction_Get_ReturnView()
        {
            // Arrange
            var controller = CreateAccountInformationController();
            var viewName = string.Empty;

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual(viewName, result.ViewName);
        }

        /// <summary>
        /// Test when input length of user name invalid.
        /// </summary>
        /// <param name="userName"> User name for search.</param>
        [Test]
        [TestCase("agentaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaabbbbbbbbb")]
        [TestCase("agent")]
        public void SearchAction_UserNameInvalidLength_ReturnInvalid(string userName)
        {
            // Arrange
            var viewModel = new SearchUserViewModel { UserName = userName };
            var controller = CreateAccountInformationController();
            var message = string.Format("<div class='alert alert-error' style='display: block;'>{0}</div>", Las.NoInfo);

            // Act
            var validationResults = ValidateModel(viewModel);

            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            var result = controller.Search(viewModel) as ContentResult;

            // Assert
            Assert.AreEqual(message, result.Content);
        }

        /// <summary>
        /// Test when call Search Admin if agent level 0.
        /// </summary>
        [Test]
        public void SearchAction_IsLevel0_CallSearchAdmin()
        {
            // Arrange
            var viewModel = new SearchUserViewModel();
            var controller = CreateAccountInformationController();
            SessionManager.CurrentSession = new CurrentSession(
                                                new AgentInfo
                                                {
                                                    AgentLevel = 1,
                                                    CurrencyId = 20,
                                                    CurrencyName = "UUS",
                                                    LicenseeId = 1,
                                                    LicenseeName = "yq355",
                                                    NumberOfLevel = 3
                                                },
                                                0,
                                                "en-US");

            // Act
            var result = controller.Search(viewModel) as ViewResult;

            // Assert
            Assert.IsInstanceOf<SearchUserResultViewModel>(result.ViewData.Model);
        }

        /// <summary>
        /// Test when call Search Agent.
        /// </summary>
        [Test]
        public void SearchAction_IsAgent_CallSearchAgent()
        {
            // Arrange
            var viewModel = new SearchUserViewModel();
            var controller = CreateAccountInformationController();
            SessionManager.CurrentSession = new CurrentSession(
                                                new AgentInfo
                                                {
                                                    AgentLevel = 2,
                                                    CurrencyId = 20,
                                                    CurrencyName = "UUS",
                                                    LicenseeId = 1,
                                                    LicenseeName = "yq355",
                                                    NumberOfLevel = 3
                                                },
                                                0,
                                                "en-US");

            // Act
            var result = controller.Search(viewModel) as ViewResult;

            // Assert
            Assert.IsInstanceOf<SearchUserResultViewModel>(result.ViewData.Model);
        }

        /// <summary>
        /// Test when suggest agent.
        /// </summary>
        [Test]
        public void SuggestAgent_Should_ReturnJson()
        {
            // Arrange
            var controller = CreateAccountInformationController();
            var viewModel = new SuggestionViewModel();

            // Act
            var result = controller.SuggestAgent(viewModel);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Test when suggest admin.
        /// </summary>
        [Test]
        public void SuggestAdmin_Should_ReturnJson()
        {
            // Arrange
            var controller = CreateAccountInformationController();
            var viewModel = new SuggestionViewModel();

            // Act
            var result = controller.SuggestAdmin(viewModel);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Test when get down line.
        /// </summary>
        [Test]
        public void GetDownlinesAction_Should_ReturnJson()
        {
            // Arrange
            var controller = CreateAccountInformationController();
            var downlineViewMode = new DownLineViewModel();

            // Act
            var result = controller.GetDownlines(downlineViewMode);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Test when get down line.
        /// </summary>
        [Test]
        public void GetDownlinesAction_Should_ReturnDownlines()
        {
            // Arrange
            var controller = CreateAccountInformationController();
            var downlineViewMode = new DownLineViewModel();

            // Act
            var result = controller.GetDownlines(downlineViewMode);

            // Assert
            var downlines = (List<DownLineEntity>)((dynamic)result.Data).List;
            Assert.IsTrue(downlines.Count > 0);
        }

        /// <summary>
        /// Create Account information Controller.
        /// </summary>
        /// <returns> Account information controller.</returns>
        private AccountInformationController CreateAccountInformationController()
        {
            var searchUserService = FakeServices.GetSearchUserService();
            var memberService = FakeServices.GetMemberService();
            var coreService = FakeServices.GetCoreService();

            return new AccountInformationController(searchUserService, memberService, coreService);
        }

        /// <summary>
        /// Validate Model.
        /// </summary>
        /// <param name="model"> Model input.</param>
        /// <returns> List validation result.</returns>
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);

            Validator.TryValidateObject(model, context, validationResults, true);

            return validationResults;
        }
    }
}