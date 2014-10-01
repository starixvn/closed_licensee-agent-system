namespace LicenseeAgent.CustomerList.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerList.Controllers;
    using LicenseeAgent.CustomerList.Entities.Member;
    using LicenseeAgent.CustomerList.Tests.Fakes;
    using LicenseeAgent.CustomerList.ViewModels;
    using LicenseeAgent.CustomerList.ViewModels.Member;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Tests.Fakes;
    using LicenseeAgent.Tests.Helpers;
    using NUnit.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestFixture]
    public class MemberControllerTests : IDisposable
    {
        #region Variables

        private MemberController _controller;

        #endregion Variables

        #region Index

        /// <summary>
        /// Get members.
        /// </summary>
        [Test]
        public void GetMembers_CustomerListViewModel_ReturnMemberEntities()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 100 };
            FakeSession.SetAgentLevel();

            // Act
            var result = _controller.Index(viewModel);
            var data = (dynamic)result.Data;

            // Assert
            Assert.IsInstanceOf<IList<MemberEntity>>(data.List);
        }

        /// <summary>
        /// Get members.
        /// </summary>
        [Test]
        public void GetMembers_CustomerListViewModel_ReturnNotSuccessWhenServiceError()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 99 };
            var message = "Success";
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Index(viewModel);
            var error = (dynamic)result.Data;

            // Assert
            Assert.AreNotEqual(Result.SuccessValue, (int)error.ErrCode);
            Assert.AreNotEqual(message, (string)error.ErrMsg);
        }

        /// <summary>
        /// Search members.
        /// </summary>
        [Test]
        public void SearchMembers_CustomerListViewModel_ReturnMemberEntities()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 100 };
            FakeSession.SetAgentLevel();

            // Act
            var result = _controller.Index(viewModel);
            var data = (dynamic)result.Data;

            // Assert
            Assert.IsInstanceOf<IList<MemberEntity>>(data.List);
        }

        /// <summary>
        /// Search members.
        /// </summary>
        [Test]
        public void SearchMembers_CustomerListViewModel_ReturnMemberEntitiesStatusOpen()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 100, Status = (int)AccountStatus.Open };
            FakeSession.SetAgentLevel();

            // Act
            var result = _controller.Index(viewModel);
            var data = (dynamic)result.Data;
            var members = (IEnumerable<MemberEntity>)data.List;

            // Assert
            Assert.AreEqual(2, members.Count());
        }

        /// <summary>
        /// Search members.
        /// </summary>
        [Test]
        public void SearchMembers_CustomerListViewModel_ReturnNotSuccessWhenServiceError()
        {
            // Arrange
            var viewModel = new CustomerListViewModel { AgentId = 99 };
            var message = "Success";
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.Index(viewModel);
            var error = (dynamic)result.Data;

            // Assert
            Assert.AreNotEqual(Result.SuccessValue, (int)error.ErrCode);
            Assert.AreNotEqual(message, (string)error.ErrMsg);
        }

        #endregion Index

        #region Change status

        /// <summary>
        /// Change member status.
        /// </summary>
        [Test]
        public void ChangeMemberStatusAction_MemberStatusViewModel_ReturnFailureWhenAccessDenied()
        {
            // Arrange
            var viewModel = new MemberStatusViewModel { AgentId = 100, MemberId = 999 };
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
        /// Change member status.
        /// </summary>
        [Test]
        public void ChangeMemberStatusAction_MemberStatusIsOpen_ReturnJsonResult()
        {
            // Arrange
            var viewModel = new MemberStatusViewModel { AgentId = 100, MemberId = 1000, Status = (int)AccountStatus.Open };
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.ChangeStatus(viewModel);
            ////var error = (dynamic)result.Data;

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Change member status.
        /// </summary>
        [Test]
        public void ChangeMemberStatusAction_MemberStatusIsOpen_ReturnFailureWhenServiceError()
        {
            // Arrange
            var viewModel = new MemberStatusViewModel { AgentId = 100, MemberId = 2000, Status = (int)AccountStatus.Open };
            var errorMessage = Las.HttpError_Default;
            FakeSession.SetAdminLevel();

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
        /// Reset password member.
        /// </summary>
        [Test]
        public void ResetPasswordMemberAction_MemberId_ReturnRedirectResultWhenAccessDenied()
        {
            // Arrange
            var memberId = 999;

            // Act
            var result = _controller.ResetPassword(memberId);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        /// <summary>
        /// Reset password member.
        /// </summary>
        [Test]
        public void ResetPasswordMemberAction_MemberId_ReturnViewName()
        {
            // Arrange
            var memberId = 1000;
            var viewName = "~/Views/CustomerList/ResetPassword.cshtml";

            // Act
            var result = _controller.ResetPassword(memberId) as ViewResult;

            // Assert
            Assert.AreEqual(viewName, result.ViewName);
        }

        /// <summary>
        /// Reset password member.
        /// </summary>
        [Test]
        public void ResetPasswordMemberAction_MemberId_ReturnViewModel()
        {
            // Arrange
            var memberId = 1000;

            // Act
            var result = _controller.ResetPassword(memberId) as ViewResult;
            var model = result.Model;

            // Assert
            Assert.IsInstanceOf<ResetPasswordViewModel>(model);
        }

        /// <summary>
        /// Reset password member.
        /// </summary>
        /// <param name="password">Password want reset.</param>
        /// <param name="confirmPassword">Confirm password want reset.</param>
        [Test]
        [TestCase("123aaAA", "123aaAA")]
        [TestCase("1234aaAAaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "1234aaAAaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        [TestCase("memberaaa", "memberaaa")]
        [TestCase("1234aaAA", "1234aaBB")]
        public void ResetPasswordMemberAction_ResetPasswordViewModelInvalid_ReturnInvalid(string password, string confirmPassword)
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel { Password = password, ConfirmPassword = confirmPassword };

            // Act
            var validationResults = TestHelper.ValidateModel(viewModel);

            // Assert
            Assert.IsTrue(validationResults.Count > 0);
        }

        /// <summary>
        /// Reset password member.
        /// </summary>
        /// <param name="password">Password want reset.</param>
        /// <param name="confirmPassword">Confirm password want reset.</param>
        [Test]
        [TestCase("1234aaAA", "1234aaAA")]
        public void ResetPasswordMemberAction_ResetPasswordViewModel_ReturnRedirectResultWhenAccessDenied(string password, string confirmPassword)
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel { Password = password, ConfirmPassword = confirmPassword, CustId = 999 };

            // Act
            var result = _controller.ResetPassword(viewModel);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        /// <summary>
        /// Reset password member.
        /// </summary>
        /// <param name="password">Password want reset.</param>
        /// <param name="confirmPassword">Confirm password want reset.</param>
        [Test]
        [TestCase("1234aaAA", "1234aaAA")]
        public void ResetPasswordMemberAction_ResetPasswordViewModel_ReturnErrorWhenServiceError(string password, string confirmPassword)
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel { Password = password, ConfirmPassword = confirmPassword, CustId = 2000 };
            FakeSession.SetAdminLevel();

            // Act
            _controller.ResetPassword(viewModel);

            // Assert
            Assert.IsTrue(_controller.ModelState.Count > 0);
        }

        /// <summary>
        /// Reset password member.
        /// </summary>
        /// <param name="password">Password want reset.</param>
        /// <param name="confirmPassword">Confirm password want reset.</param>
        [Test]
        [TestCase("1234aaAA", "1234aaAA")]
        public void ResetPasswordMemberAction_ResetPasswordViewModel_ReturnSuccess(string password, string confirmPassword)
        {
            // Arrange
            var viewModel = new ResetPasswordViewModel { Password = password, ConfirmPassword = confirmPassword, CustId = 1000 };
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
        /// Edit information member.
        /// </summary>
        [Test]
        public void EditInfoMemberAction_MemberId_ReturnRedirectResultWhenAccessDenied()
        {
            // Arrange
            var memberId = 999;
            var userName = "member";
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.EditInfo(memberId, userName);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        /// <summary>
        /// Edit information member.
        /// </summary>
        [Test]
        public void EditInfoMemberAction_MemberId_ReturnViewName()
        {
            // Arrange
            var memberId = 1000;
            var userName = "member";
            var viewName = "~/Views/CustomerList/EditInfo.cshtml";
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.EditInfo(memberId, userName) as ViewResult;

            // Assert
            Assert.AreEqual(viewName, result.ViewName);
        }

        /// <summary>
        /// Edit information member.
        /// </summary>
        [Test]
        public void EditInfoMemberAction_MemberId_ReturnEditInfoViewModel()
        {
            // Arrange
            var memberId = 1000;
            var userName = "member";
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.EditInfo(memberId, userName) as ViewResult;

            // Assert
            Assert.IsInstanceOf<EditInfoViewModel>(result.Model);
        }

        /// <summary>
        /// Edit information member.
        /// </summary>
        /// <param name="phone">Phone of member.</param>
        /// <param name="mobile">Mobile of member.</param>
        /// <param name="fax">Fax of member.</param>
        [Test]
        [TestCase("123456a", "123456", "123456")]
        [TestCase("123456", "123456a", "123456")]
        [TestCase("123456", "123456", "123456a")]
        public void EditInfoMemberAction_EditInfoViewModelIsInvalid_ReturnInvalid(string phone, string mobile, string fax)
        {
            // Arrange
            var viewModel = new EditInfoViewModel { CustId = 1000, UserName = "member", Phone = phone, Mobile = mobile, Fax = fax };

            // Act
            var validationResults = TestHelper.ValidateModel(viewModel);

            // Assert
            Assert.IsTrue(validationResults.Count > 0);
        }

        /// <summary>
        /// Edit information member.
        /// </summary>
        /// <param name="phone">Phone of member.</param>
        /// <param name="mobile">Mobile of member.</param>
        /// <param name="fax">Fax of member.</param>
        [Test]
        [TestCase("123456", "123456", "123456")]
        public void EditInfoMemberAction_EditInfoViewModel_ReturnRedirectResultWhenAccessDenied(string phone, string mobile, string fax)
        {
            // Arrange
            var viewModel = new EditInfoViewModel { CustId = 999, UserName = "member", Phone = phone, Mobile = mobile, Fax = fax };
            FakeSession.SetAdminLevel();

            // Act
            var result = _controller.EditInfo(viewModel);

            // Assert
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        /// <summary>
        /// Edit information member.
        /// </summary>
        /// <param name="phone">Phone of member.</param>
        /// <param name="mobile">Mobile of member.</param>
        /// <param name="fax">Fax of member.</param>
        [Test]
        [TestCase("123456", "123456", "123456")]
        public void EditInfoMemberAction_EditInfoViewModel_ReturnErrorWhenServiceError(string phone, string mobile, string fax)
        {
            // Arrange
            var viewModel = new EditInfoViewModel { CustId = 2000, UserName = "member", Phone = phone, Mobile = mobile, Fax = fax };
            FakeSession.SetAdminLevel();

            // Act
            _controller.EditInfo(viewModel);

            // Assert
            Assert.IsTrue(_controller.ModelState.Count > 0);
        }

        /// <summary>
        /// Edit information member.
        /// </summary>
        /// <param name="phone">Phone of member.</param>
        /// <param name="mobile">Mobile of member.</param>
        /// <param name="fax">Fax of member.</param>
        [Test]
        [TestCase("123456", "123456", "123456")]
        public void EditInfoMemberAction_EditInfoViewModel_ReturnSuccess(string phone, string mobile, string fax)
        {
            // Arrange
            var viewModel = new EditInfoViewModel { CustId = 1000, UserName = "member", Phone = phone, Mobile = mobile, Fax = fax };
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
            var memberListService = FakeServices.MemberListService;
            var memberService = FakeServices.MemberService;
            var downlineService = FakeServices.DownLineService;
            var coreService = FakeServices.CoreService;
            _controller = new MemberController(memberListService, memberService, downlineService, coreService);
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