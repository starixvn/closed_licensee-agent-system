// -----------------------------------------------------------------------
// <copyright file="AccountInformationControllersTests.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.AccountInformation.Controllers;
    using LicenseeAgent.AccountInformation.Criteria;
    using LicenseeAgent.AccountInformation.Entities;
    using LicenseeAgent.AccountInformation.Tests.Fake;
    using LicenseeAgent.AccountInformation.ViewModels;
    using LicenseeAgent.AccountInformation.ViewResults;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Tests;
    using LicenseeAgent.Tests.Fakes;
    using NUnit.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestFixture]
    public class AccountInformationControllersTests
    {
        private AccountInformationController _controller;
 
        /// <summary>
        /// Initialize when begin testing.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            FakeSession.InitHttpContext();
            _controller = CreateAccountInformationController();
        }

        /// <summary>
        /// Release resource.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
            _controller = null;
        }

        /// <summary>
        /// Test when load page.
        /// </summary>
        [Test]
        public void IndexAction_Get_ReturnView()
        {
            // Arrange
            var viewName = string.Empty;

            // Act
            var result = _controller.Index() as ViewResult;

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
            var message = string.Format("<div class='alert alert-error' style='display: block;'>{0}</div>", Las.NoInfo);

            // Act
            var validationResults = TestExtensions.ValidateModel(viewModel);
            
            foreach (var validationResult in validationResults)
            {
                _controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            var result = _controller.Search(viewModel) as ContentResult;

            // Assert
            Assert.AreEqual(message, result.Content);
        }

        /// <summary>
        /// Test when call Search Admin if agent level 0.
        /// </summary>
        [Test]
        public void SearchAction_IsAdmin_SearchAdmin()
        {
            // Arrange
            var viewModel = new SearchUserViewModel();
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Search(viewModel) as ViewResult;

            // Assert
            Assert.IsInstanceOf<SearchUserResultViewModel>(result.ViewData.Model);
        }

        /// <summary>
        /// Test when call Search Agent.
        /// </summary>
        [Test]
        public void SearchAction_IsAgent_SearchAgent()
        {
            // Arrange
            var viewModel = new SearchUserViewModel();
            FakeSession.SetAgentLevel();

            // Act
            var result = _controller.Search(viewModel) as ViewResult;

            // Assert
            Assert.IsInstanceOf<SearchUserResultViewModel>(result.ViewData.Model);
        }

        /// <summary>
        /// Suggest action.
        /// </summary>
        [Test]
        public void SuggestAction_AdminLevel_SuggestForAdminLevel()
        {
            // Arrange
            var viewModel = new SuggestionViewModel();
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Suggest(viewModel) as JsonResult;

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Suggest action.
        /// </summary>
        [Test]
        public void SuggestAction_AgentLevel_SuggestForAgentLevel()
        {
            // Arrange
            var viewModel = new SuggestionViewModel();
            FakeSession.SetAgentLevel();

            // Act
            var result = _controller.Suggest(viewModel) as JsonResult;

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Suggest action.
        /// </summary>
        [Test]
        public void SuggestAction_AdminLevel_ReturnArray()
        {
            // Arrange
            var viewModel = new SuggestionViewModel();
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Suggest(viewModel) as JsonResult;

            // Assert
            var usernames = ((dynamic)result.Data).List;
            Assert.IsInstanceOf<string[]>(usernames);
        }

        /// <summary>
        /// Suggest action.
        /// </summary>
        [Test]
        public void SuggestAction_AgentLevel_ReturnArray()
        {
            // Arrange
            var viewModel = new SuggestionViewModel();
            FakeSession.SetAgentLevel();

            // Act
            var result = _controller.Suggest(viewModel) as JsonResult;

            // Assert
            var usernames = ((dynamic)result.Data).List;
            Assert.IsInstanceOf<string[]>(usernames);
        }

        /// <summary>
        /// Test when suggest agent.
        /// </summary>
        [Test]
        public void SuggestAgent_Should_ReturnJson()
        {
            // Arrange
            var viewModel = new SuggestionViewModel();
            FakeSession.SetAgentLevel();

            // Act
            var result = _controller.SuggestAgent(viewModel);

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
            var viewModel = new SuggestionViewModel();
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.SuggestAdmin(viewModel);

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
            var downlineViewMode = new DownLineViewModel();

            // Act
            var result = _controller.GetDownlines(downlineViewMode);

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
            var downlineViewMode = new DownLineViewModel();

            // Act
            var result = _controller.GetDownlines(downlineViewMode);

            // Assert
            var downlines = (List<DownLineEntity>)((dynamic)result.Data).List;
            Assert.IsTrue(downlines.Count > 0);
        }

        /// <summary>
        /// Get full information.
        /// </summary>
        [Test]
        public void GetFullInformation_IsMember_UpdateMemberInformation()
        {
            // Arrange
            var customer = new CustomerEntity { CustId = 1, UserName = "Member" };
            var agentId = 1;
            var isLastLevel = true;

            FakeSession.SetAdminLevel();
            _controller.ViewBag.PageContext = new PageContext(agentId, isLastLevel) { IsMember = true };

            // Act
            var result = _controller.GetFullInformation(customer);

            // Assert
            Assert.IsNotNullOrEmpty(result.LastName);
            Assert.IsNotNullOrEmpty(result.FirstName);
            Assert.IsNotNullOrEmpty(result.Phone);
            Assert.IsNotNullOrEmpty(result.Mobile);
            Assert.IsNotNullOrEmpty(result.Fax);
        }

        /// <summary>
        /// Get full information.
        /// </summary>
        [Test]
        public void GetFullInformation_IsAgent_NoUpdateInformation()
        {
            // Arrange
            var customer = new CustomerEntity { CustId = 1, UserName = "Agent" };
            var agentId = 1;
            var isLastLevel = true;

            FakeSession.SetAdminLevel();
            _controller.ViewBag.PageContext = new PageContext(agentId, isLastLevel) { IsMember = false };

            // Act
            var result = _controller.GetFullInformation(customer);

            // Assert
            Assert.IsNullOrEmpty(result.LastName);
            Assert.IsNullOrEmpty(result.FirstName);
            Assert.IsNullOrEmpty(result.Phone);
            Assert.IsNullOrEmpty(result.Mobile);
            Assert.IsNullOrEmpty(result.Fax);
        }

        /// <summary>
        /// Set Admin view.
        /// </summary>
        [Test]
        public void SetAdminView_MsgIdError_SearchUserViewResult()
        {
            // Arrange
            var criteria = new AdminGetMemberListCriteria
            {
                MsgId = 1,
                AgentId = 10,
                CurrencyId = 22,
                LicenseeId = 27,
                AgentCurrency = 22,
                AgentCurrencyName = "UUS"
            };
            var customer = new CustomerEntity { CustId = 100 };

            // Act
            _controller.SetAdminView(criteria, customer);

            // Assert
            Assert.IsInstanceOf<SearchUserViewResult>(_controller.ViewBag.Result);
        }

        /// <summary>
        /// Set Admin View.
        /// </summary>
        [Test]
        public void SetAdminView_CustomerIsNull_SearchUserViewResult()
        {
            // Arrange
            var criteria = new AdminGetMemberListCriteria
            {
                MsgId = 0,
                AgentId = 10,
                CurrencyId = 22,
                LicenseeId = 27,
                AgentCurrency = 22,
                AgentCurrencyName = "UUS"
            };

            // Act
            _controller.SetAdminView(criteria, null);

            // Assert
            Assert.IsInstanceOf<SearchUserViewResult>(_controller.ViewBag.Result);
        }

        /// <summary>
        /// Set agent view.
        /// </summary>
        [Test]
        public void SetAgentView_MsgIdError_SearchUserViewResult()
        {
            // Arrange
            var criteria = new AgentGetMemberListCriteria
            {
                MsgId = 1
            };
            var customer = new CustomerEntity { CustId = 100 };

            // Act
            _controller.SetAgentView(criteria, customer);

            // Assert
            Assert.IsInstanceOf<SearchUserViewResult>(_controller.ViewBag.Result);
        }

        /// <summary>
        /// Set agent view.
        /// </summary>
        [Test]
        public void SetAgentView_CustomerIsNull_SearchUserViewResult()
        {
            // Arrange
            var criteria = new AgentGetMemberListCriteria
            {
                MsgId = 0
            };

            // Act
            _controller.SetAgentView(criteria, null);

            // Assert
            Assert.IsInstanceOf<SearchUserViewResult>(_controller.ViewBag.Result);
        }
        
        /// <summary>
        /// Create Account information Controller.
        /// </summary>
        /// <returns> Account information controller.</returns>
        private AccountInformationController CreateAccountInformationController()
        {
            var searchUserService = FakeServices.GetSearchUserService();
            var memberService = FakeServices.GetMemberService();
            var downlineService = FakeServices.GetDownlLinePermissionService();
            var coreService = FakeServices.GetCoreService();

            return new AccountInformationController(searchUserService, memberService, downlineService, coreService);
        }
    }
}
