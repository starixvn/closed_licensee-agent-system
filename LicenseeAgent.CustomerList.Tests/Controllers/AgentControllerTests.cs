namespace LicenseeAgent.CustomerList.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerList.Controllers;
    using LicenseeAgent.CustomerList.Entities.Agent;
    using LicenseeAgent.CustomerList.Tests.Fakes;
    using LicenseeAgent.CustomerList.ViewModels;
    using LicenseeAgent.CustomerList.ViewModels.Agent;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Tests.Fakes;
    using LicenseeAgent.Tests.Helpers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.ViewModels;
    using NUnit.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestFixture]
    public class AgentControllerTests : IDisposable
    {
        #region Variables

        private AgentController _controller;

        #endregion Variables

        #region Breadcrumb

        /// <summary>
        /// Test when call Index action.
        /// </summary>
        [Test]
        public void IndexAction_AgentInformationViewModel_ReturnViewResult()
        {
            // Arrange
            var viewModel = new AgentBreadcrumbViewModel { AgentId = 1, AgentName = "Agent", AgentLevel = 1 };
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Index(viewModel) as ViewResult;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        /// <summary>
        /// Test when call Index action.
        /// </summary>
        [Test]
        public void IndexAction_AgentInformationViewModel_ReturnBreadcrumb()
        {
            // Arrange
            var viewModel = new AgentBreadcrumbViewModel { AgentId = 1, AgentName = "Agent", AgentLevel = 1 };
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Index(viewModel) as ViewResult;

            // Assert
            Assert.IsInstanceOf<AgentBreadcrumbViewModel>(result.Model);
        }

        /// <summary>
        /// Test when call Index action.
        /// </summary>
        [Test]
        public void IndexAction_AgentBreadcrumbViewModel_ReturnRedirectResultWhenAccessDenied()
        {
            // Arrange
            var viewModel = new AgentBreadcrumbViewModel { AgentId = 99, AgentLevel = 1 };

            // Act
            var result = _controller.Index(viewModel);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        /// <summary>
        /// Test when call Index action.
        /// </summary>
        [Test]
        public void IndexAction_AgentBreadcrumbViewModel_ReturnContentResult()
        {
            // Arrange
            var breadCrumb = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { AgentId = 1, AgentLevel = 1, AgentName = "Agent1" },
                new BreadcrumbItem { AgentId = 2, AgentLevel = 2, AgentName = "Agent2" }
            };

            var viewModel = new AgentBreadcrumbViewModel
            {
                AgentId = 100,
                AgentLevel = 1,
                Breadcrumb = new Breadcrumb(breadCrumb)
            };

            // Act
            var result = _controller.Index(viewModel);

            // Assert
            Assert.IsInstanceOf<ContentResult>(result);
        }

        /// <summary>
        /// Test when call Index action.
        /// </summary>
        [Test]
        public void IndexAction_AgentBreadcrumbViewModel_ReturnViewResult()
        {
            // Arrange
            var breadCrumb = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { AgentId = 1, AgentLevel = 1, AgentName  = "Agent1" },
                new BreadcrumbItem { AgentId = 2, AgentLevel = 2, AgentName  = "Agent2" }
            };

            var viewModel = new AgentBreadcrumbViewModel
            {
                AgentId = 100,
                AgentLevel = 3,
                Breadcrumb = new Breadcrumb(breadCrumb)
            };

            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Index(viewModel);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        /// <summary>
        /// Test when call Index action.
        /// </summary>
        [Test]
        public void IndexAction_AgentBreadcrumbViewModel_ReturnBreadcrumb()
        {
            // Arrange
            var breadCrumb = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { AgentId = 1, AgentLevel = 1, AgentName  = "Agent1" },
                new BreadcrumbItem { AgentId = 2, AgentLevel = 2, AgentName  = "Agent2" }
            };

            var viewModel = new AgentBreadcrumbViewModel
            {
                AgentId = 100,
                AgentLevel = 3,
                Breadcrumb = new Breadcrumb(breadCrumb)
            };

            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Index(viewModel) as ViewResult;

            // Assert
            Assert.IsInstanceOf<AgentBreadcrumbViewModel>(result.Model);
        }

        #endregion Breadcrumb

        #region Index

        /// <summary>
        /// Test when call Get Customer List action.
        /// </summary>
        [Test]
        public void GetCustomerListAction_CustomerListViewModel_ReturnJsonResultWhenAccessDenied()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 99 };

            // Act
            var result = _controller.Index(viewModel);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Test when call Get Customer List action.
        /// </summary>
        [Test]
        public void GetCustomerListAction_CustomerListViewModel_ReturnFailureWhenAccessDenied()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 99 };
            var errorMessage = Las.AccessDeniedMsg;

            // Act
            var result = _controller.Index(viewModel);
            var error = result.Data;
            var type = error.GetType();
            var errCodePropertyInfo = type.GetProperty("ErrCode");
            var errMsgPropertyInfo = type.GetProperty("ErrMsg");

            var errCode = errCodePropertyInfo.GetValue(error, null);
            var errMsg = errMsgPropertyInfo.GetValue(error, null);

            // Assert
            Assert.AreEqual(Result.FailureValue, (int)errCode);
            Assert.AreEqual(errorMessage, errMsg.ToString());
        }

        /// <summary>
        /// Test when call Get Customer List action.
        /// </summary>
        [Test]
        public void GetCustomerListAction_CustomerListViewModel_IsLastLevel()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 100 };
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Index(viewModel);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Test when call Get Customer List action.
        /// </summary>
        [Test]
        public void GetCustomerListAction_CustomerListViewModel_DontLastLevel()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 100 };
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Index(viewModel);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Get agent return JSON result.
        /// </summary>
        [Test]
        public void GetAgents_CustomerListViewModel_ReturnJsonResult()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 100 };

            // Act
            var result = _controller.GetAgents(viewModel);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Get agent list.
        /// </summary>
        [Test]
        public void GetAgents_CustomerListViewModel_ReturnAgentEntities()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 100 };

            // Act
            var result = _controller.GetAgents(viewModel);
            var data = (dynamic)result.Data;
            var agents = (IList<AgentEntity>)data.List;

            // Assert
            Assert.AreEqual(5, agents.Count);
        }

        /// <summary>
        /// Search agent return JSON result.
        /// </summary>
        [Test]
        public void SearchAgents_CustomerListViewModel_ReturnJsonResult()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 100 };

            // Act
            var result = _controller.SearchAgents(viewModel);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Search agent return JSON result.
        /// </summary>
        [Test]
        public void SearchAgents_CustomerListViewModel_ReturnAgentEntities()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 100 };

            // Act
            var result = _controller.SearchAgents(viewModel);
            var data = (dynamic)result.Data;
            var agents = (IList<AgentEntity>)data.List;

            // Assert
            Assert.AreEqual(5, agents.Count);
        }

        #endregion Index

        #region Change status

        /// <summary>
        /// Change agent status.
        /// </summary>
        [Test]
        public void ChangeAgentStatusAction_AgentStatusViewModel_ReturnFailureWhenAccessDenied()
        {
            // Arrange
            var viewModel = new AgentStatusViewModel { AgentId = 99 };
            var errorMessage = Las.AccessDeniedMsg;

            // Act
            var result = _controller.ChangeStatus(viewModel);
            var error = result.Data;
            var type = error.GetType();
            var errCodePropertyInfo = type.GetProperty("ErrCode");
            var errMsgPropertyInfo = type.GetProperty("ErrMsg");

            var errCode = errCodePropertyInfo.GetValue(error, null);
            var errMsg = errMsgPropertyInfo.GetValue(error, null);

            // Assert
            Assert.AreEqual(Result.FailureValue, (int)errCode);
            Assert.AreEqual(errorMessage, errMsg.ToString());
        }

        /// <summary>
        /// Change agent status.
        /// </summary>
        [Test]
        public void ChangeAgentStatusAction_AgentStatusIsClose_ReturnJsonResult()
        {
            // Arrange
            var viewModel = new AgentStatusViewModel { AgentId = 100, Status = (int)AccountStatus.Closed };
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.ChangeStatus(viewModel);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Change agent status.
        /// </summary>
        [Test]
        public void ChangeAgentStatusAction_AgentStatusIsClose_ReturnFailure()
        {
            // Arrange
            var viewModel = new AgentStatusViewModel { AgentId = 200, Status = (int)AccountStatus.Closed };
            var message = "Success";
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.ChangeStatus(viewModel);
            var error = (dynamic)result.Data;

            // Assert
            Assert.AreNotEqual(Result.SuccessValue, (int)error.ErrCode);
            Assert.AreNotEqual(message, (string)error.ErrMsg);
        }

        #endregion Change status

        #region Auto complete

        /// <summary>
        /// Customer search auto complete.
        /// </summary>
        [Test]
        public void CustomerSearchAutoCompleteAction_CustomerSearchAutoCompleteViewModel_ReturnFailureWhenAccessDenied()
        {
            // Arrange
            var viewModel = new CustomerSearchAutoCompleteViewModel { AgentId = 99 };
            var errorMessage = Las.AccessDeniedMsg;
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.AutoComplete(viewModel);
            var error = result.Data;
            var type = error.GetType();
            var errCodePropertyInfo = type.GetProperty("ErrCode");
            var errMsgPropertyInfo = type.GetProperty("ErrMsg");

            var errCode = errCodePropertyInfo.GetValue(error, null);
            var errMsg = errMsgPropertyInfo.GetValue(error, null);

            // Assert
            Assert.AreEqual(Result.FailureValue, (int)errCode);
            Assert.AreEqual(errorMessage, errMsg.ToString());
        }

        /// <summary>
        /// Customer search auto complete.
        /// </summary>
        [Test]
        public void CustomerSearchAutoCompleteAction_CustomerSearchAutoCompleteViewModel_ReturnJsonResult()
        {
            // Arrange
            var viewModel = new CustomerSearchAutoCompleteViewModel { AgentId = 100 };
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.AutoComplete(viewModel);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Search agent auto complete.
        /// </summary>
        [Test]
        public void SearchAgentAutoComplete_CustomerSearchAutoCompleteViewModel_ReturnArray()
        {
            // Arrange
            var viewModel = new CustomerSearchAutoCompleteViewModel { AgentId = 100 };
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.AutoComplete(viewModel);
            var data = (dynamic)result.Data;

            // Assert
            Assert.IsInstanceOf<string[]>(data.List);
        }

        /// <summary>
        /// Search member auto complete.
        /// </summary>
        [Test]
        public void SearchMemberAutoComplete_CustomerSearchAutoCompleteViewModel_ReturnArray()
        {
            // Arrange
            var viewModel = new CustomerSearchAutoCompleteViewModel { AgentId = 100 };
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.AutoComplete(viewModel);
            var data = (dynamic)result.Data;

            // Assert
            Assert.IsInstanceOf<string[]>(data.List);
        }

        #endregion Auto complete

        #region Reset password

        /// <summary>
        /// Reset password agent.
        /// </summary>
        [Test]
        public void ResetPasswordAgentAction_AgentInformation_ReturnRedirectResultWhenAccessDenied()
        {
            // Arrange
            var agentId = 99;

            // Act
            var result = _controller.ResetPassword(agentId);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        /// <summary>
        /// Reset password agent.
        /// </summary>
        [Test]
        public void ResetPasswordAgentAction_AgentInformation_ReturnViewName()
        {
            // Arrange
            var agentId = 100;
            var viewName = "~/Views/CustomerList/ResetPassword.cshtml";
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.ResetPassword(agentId) as ViewResult;

            // Assert
            Assert.AreEqual(viewName, result.ViewName);
        }

        /// <summary>
        /// Reset password agent.
        /// </summary>
        [Test]
        public void ResetPasswordAgentAction_AgentInformation_ReturnViewModel()
        {
            // Arrange
            var agentId = 100;
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.ResetPassword(agentId) as ViewResult;
            var model = result.Model;

            // Assert
            Assert.IsInstanceOf<ResetPasswordViewModel>(model);
        }

        /// <summary>
        /// Reset password agent.
        /// </summary>
        /// <param name="password">Password want reset.</param>
        /// <param name="confirmPassword">Confirm password want reset.</param>
        [Test]
        [TestCase("123aaAA", "123aaAA")]
        [TestCase("1234aaAAaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "1234aaAAaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        [TestCase("memberaaa", "memberaaa")]
        [TestCase("1234aaAA", "1234aaBB")]
        public void ResetPasswordAgentAction_ResetPasswordViewModelInvalid_ReturnInvalid(string password, string confirmPassword)
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel { Password = password, ConfirmPassword = confirmPassword };

            // Act
            var validationResults = TestHelper.ValidateModel(viewModel);

            // Assert
            Assert.IsTrue(validationResults.Count > 0);
        }

        /// <summary>
        /// Reset password agent.
        /// </summary>
        /// <param name="password">Password want reset.</param>
        /// <param name="confirmPassword">Confirm password want reset.</param>
        [Test]
        [TestCase("1234aaAA", "1234aaAA")]
        public void ResetPasswordAgentAction_ResetPasswordViewModel_ReturnRedirectResultWhenAccessDenied(string password, string confirmPassword)
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel { Password = password, ConfirmPassword = confirmPassword, CustId = 99 };

            // Act
            var result = _controller.ResetPassword(viewModel);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        /// <summary>
        /// Reset password agent.
        /// </summary>
        /// <param name="password">Password want reset.</param>
        /// <param name="confirmPassword">Confirm password want reset.</param>
        [Test]
        [TestCase("1234aaAA", "1234aaAA")]
        public void ResetPasswordAgentAction_ResetPasswordViewModel_ReturnErrorWhenServiceError(string password, string confirmPassword)
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel { Password = password, ConfirmPassword = confirmPassword, CustId = 200 };
            FakeSession.SetAdminLevel();

            // Act
            _controller.ResetPassword(viewModel);

            // Assert
            Assert.IsTrue(_controller.ModelState.Count > 0);
        }

        /// <summary>
        /// Reset password agent.
        /// </summary>
        /// <param name="password">Password want reset.</param>
        /// <param name="confirmPassword">Confirm password want reset.</param>
        [Test]
        [TestCase("1234aaAA", "1234aaAA")]
        public void ResetPasswordAgentAction_ResetPasswordViewModel_ReturnSuccess(string password, string confirmPassword)
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel { Password = password, ConfirmPassword = confirmPassword, CustId = 100 };
            var message = Las.UpdateSuccessfully;
            FakeSession.SetAdminLevel();

            // Act
            _controller.ResetPassword(viewModel);

            // Assert
            Assert.AreEqual(message, _controller.ViewBag.Result);
        }

        #endregion Reset password

        #region Edit info

        /// <summary>
        /// Edit information agent.
        /// </summary>
        [Test]
        public void EditInfoAgentAction_AgentId_ReturnRedirectResultWhenAccessDenied()
        {
            // Arrange
            var agentId = 99;
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.EditInfo(agentId);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        /// <summary>
        /// Edit information agent.
        /// </summary>
        [Test]
        public void EditInfoAgentAction_AgentId_ReturnViewName()
        {
            // Arrange
            var agentId = 100;
            var viewName = "~/Views/CustomerList/EditInfo.cshtml";
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.EditInfo(agentId) as ViewResult;

            // Assert
            Assert.AreEqual(viewName, result.ViewName);
        }

        /// <summary>
        /// Edit information agent.
        /// </summary>
        [Test]
        public void EditInfoAgentAction_AgentId_ReturnEditInfoViewModel()
        {
            // Arrange
            var agentId = 100;
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.EditInfo(agentId) as ViewResult;

            // Assert
            Assert.IsInstanceOf<EditInfoViewModel>(result.Model);
        }

        /// <summary>
        /// Edit information agent.
        /// </summary>
        /// <param name="phone">Phone of member.</param>
        /// <param name="mobile">Mobile of member.</param>
        /// <param name="fax">Fax of member.</param>
        [Test]
        [TestCase("123456a", "123456", "123456")]
        [TestCase("123456", "123456a", "123456")]
        [TestCase("123456", "123456", "123456a")]
        public void EditInfoAgentAction_EditInfoViewModelIsInvalid_ReturnInvalid(string phone, string mobile, string fax)
        {
            // Arrange
            var viewModel = new EditInfoViewModel { CustId = 100, UserName = "agent", Phone = phone, Mobile = mobile, Fax = fax };

            // Act
            var validationResults = TestHelper.ValidateModel(viewModel);

            // Assert
            Assert.IsTrue(validationResults.Count > 0);
        }

        /// <summary>
        /// Edit information agent.
        /// </summary>
        /// <param name="phone">Phone of member.</param>
        /// <param name="mobile">Mobile of member.</param>
        /// <param name="fax">Fax of member.</param>
        [Test]
        [TestCase("123456", "123456", "123456")]
        public void EditInfoAgentAction_EditInfoViewModel_ReturnRedirectResultWhenAccessDenied(string phone, string mobile, string fax)
        {
            // Arrange
            var viewModel = new EditInfoViewModel { CustId = 99, UserName = "agent", Phone = phone, Mobile = mobile, Fax = fax };
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.EditInfo(viewModel);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        /// <summary>
        /// Edit information agent.
        /// </summary>
        /// <param name="phone">Phone of member.</param>
        /// <param name="mobile">Mobile of member.</param>
        /// <param name="fax">Fax of member.</param>
        [Test]
        [TestCase("123456", "123456", "123456")]
        public void EditInfoAgentAction_EditInfoViewModel_ReturnErrorWhenServiceError(string phone, string mobile, string fax)
        {
            // Arrange
            var viewModel = new EditInfoViewModel { CustId = 200, UserName = "agent", Phone = phone, Mobile = mobile, Fax = fax };
            FakeSession.SetAdminLevel();

            // Act
            _controller.EditInfo(viewModel);

            // Assert
            Assert.IsTrue(_controller.ModelState.Count > 0);
        }

        /// <summary>
        /// Edit information agent.
        /// </summary>
        /// <param name="phone">Phone of member.</param>
        /// <param name="mobile">Mobile of member.</param>
        /// <param name="fax">Fax of member.</param>
        [Test]
        [TestCase("123456", "123456", "123456")]
        public void EditInfoAgentAction_EditInfoViewModel_ReturnSuccess(string phone, string mobile, string fax)
        {
            // Arrange
            var viewModel = new EditInfoViewModel { CustId = 100, UserName = "agent", Phone = phone, Mobile = mobile, Fax = fax };
            var message = Las.UpdateSuccessfully;
            FakeSession.SetAdminLevel();

            // Act
            _controller.EditInfo(viewModel);

            // Assert
            Assert.AreEqual(message, _controller.ViewBag.Result);
        }

        #endregion Edit info

        #region NUNIT

        [SetUp]
        public void SetUp()
        {
            FakeSession.InitHttpContext();
            var agentListService = FakeServices.AgentListService;
            var memberService = FakeServices.MemberService;
            var downlineService = FakeServices.DownLineService;
            var coreService = FakeServices.CoreService;
            _controller = new AgentController(agentListService, memberService, downlineService, coreService);
        }

        [TearDown]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _controller.Dispose();
            _controller = null;
        }

        #endregion NUNIT
    }
}