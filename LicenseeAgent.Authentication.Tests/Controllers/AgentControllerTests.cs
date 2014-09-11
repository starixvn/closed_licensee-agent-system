namespace LicenseeAgent.Authentication.Tests.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using LicenseeAgent.Authentication.Controllers;
    using LicenseeAgent.Authentication.Criteria.Agent;
    using LicenseeAgent.Authentication.Services;
    using LicenseeAgent.Authentication.Utility;
    using LicenseeAgent.Authentication.ViewModels.Agent;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Tests.Fakes;
    using LicenseeAgent.Tests.Helpers;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Services;
    using NSubstitute;
    using NUnit.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestFixture]
    public class AgentControllerTests : IDisposable
    {
        private IAgentService _accountService;
        private ICoreService _coreService;
        private CaptchaImage _captchaImage;
        private ICaptchaService _captchaService;
        private IAuthenticateService _authenticateService;
        private AgentController _agentController;
        private CaptchaController _captchaController;

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _accountService = Substitute.For<IAgentService>();
            _coreService = Substitute.For<ICoreService>();
            _captchaImage = new CaptchaImage();
            _captchaService = new CaptchaService(_captchaImage);
            _authenticateService = new AuthenticateService();
            _agentController = new AgentController(_accountService, _coreService, _authenticateService);
            _captchaController = new CaptchaController(_captchaService);
        }

        /// <summary>
        /// Captcha the image.
        /// </summary>
        [Test]
        public void Agent_SignIn_With_Captcha_Image()
        {
            // Arrange
            var sessionItems = new SessionStateItemCollection();
            sessionItems[Constants.AppSessionCaptcha] = string.Empty;
            _agentController.ControllerContext = new FakeControllerContext(_agentController, sessionItems);

            // Act
            var result = _captchaController.Index();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<FileContentResult>(result);
            Assert.AreEqual(_captchaImage.Text, _agentController.HttpContext.Session[Constants.AppSessionCaptcha]);
        }

        /// <summary>
        /// Agent can't sign-in with empty user credential.
        /// </summary>
        [Test]
        public void Agent_Cannot_SignIn_With_Empty_UserCredential()
        {
            // Arrange
            var model = new AgentSignInViewModel();
            _agentController.ControllerContext = new FakeControllerContext(_agentController);

            // Act
            var isValid = _agentController.ValidateModel(model);
            var result = _agentController.SignIn(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, isValid);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        /// <summary>
        /// Agent can't sign-in with wrong licensee domain.
        /// </summary>
        [Test]
        public void Agent_Cannot_SignIn_With_Wrong_LicenseeDomain()
        {
            // Arrange
            var captchaText = "123456";
            var model = new AgentSignInViewModel
            {
                Username = "leo",
                Password = "password123",
                Captcha = captchaText,
                Language = "en-US"
            };

            var sessionItems = new SessionStateItemCollection();
            sessionItems[Constants.AppSessionCaptcha] = captchaText;
            _agentController.ControllerContext = new FakeControllerContext(_agentController, sessionItems);

            // Act
            var isValid = _agentController.ValidateModel(model);
            var result = _agentController.SignIn(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, isValid);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
            Assert.AreEqual(result.ViewData.ModelState.GetErrorMessage(), string.Format(Las.NoLicenseeDomainFound, _agentController.Request.Url.Host));
        }

        /// <summary>
        /// Agent can't sign in with wrong captcha.
        /// </summary>
        [Test]
        public void Agent_Cannot_SignIn_With_Wrong_Captcha()
        {
            // Arrange
            var captchaText = "123456";
            var model = new AgentSignInViewModel
            {
                Username = "leo",
                Password = "password123",
                Captcha = "654321",
                Language = "en-US"
            };

            var sessionItems = new SessionStateItemCollection();
            sessionItems[Constants.AppSessionCaptcha] = captchaText;
            _agentController.ControllerContext = new FakeControllerContext(_agentController, sessionItems);

            // Act
            var isValid = _agentController.ValidateModel(model);
            var result = _agentController.SignIn(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, isValid);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
            Assert.AreEqual(result.ViewData.ModelState.GetErrorMessage(), Las.IncorrectCaptchaMsg);
        }

        /// <summary>
        /// Agent sign in successful and redirect to home.
        /// </summary>
        [Test]
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        public void Agent_SignIn_Success_Redirects_To_Home()
        {
            // Arrange
            var captchaText = "123456";
            var model = new AgentSignInViewModel
            {
                Username = "leoagent",
                Password = "password123",
                Captcha = captchaText,
                Language = "en-US"
            };

            var sessionItems = new SessionStateItemCollection();
            sessionItems[Constants.AppSessionCaptcha] = captchaText;
            _agentController.ControllerContext = new FakeControllerContext(_agentController, sessionItems);

            _coreService.GetLicenseeId(Arg.Any<HttpRequestBase>()).Returns((int?)Licensee.Licensee_27);
            _accountService.GetAgentSalt(Arg.Do<AgentSaltSelectCriteria>(x => x.ErrCode = Result.SuccessValue)).Returns("leosalt");
            var agentInfo = new AgentInfo
            {
                AgentId = 1,
                UserName = model.Username,
                AgentLevel = 1,
                NumberOfLevel = 3,
                CurrencyId = 1,
                CurrencyName = "UUS",
                LicenseeId = (int)Licensee.Licensee_27,
                LicenseeName = "yq355"
            };
            _accountService.AuthenticateAgent(Arg.Do<AgentLoginCriteria>(x => x.ErrCode = Result.SuccessValue)).Returns(agentInfo);
            _accountService.AgentCheckIn(Arg.Do<AgentCheckInCriteria>(x => x.OnUserId = 1));

            // Act
            var isValid = _agentController.ValidateModel(model);
            var result = _agentController.SignIn(model) as RedirectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, isValid);
            Assert.IsNotNull(SessionManager.CurrentSession);
            Assert.IsTrue(SessionManager.CurrentSession.IsValid);
            Assert.IsNull(SessionManager.CurrentSession.AdminInfo);
            Assert.IsNotNull(SessionManager.CurrentSession.AgentInfo);
            Assert.AreSame(agentInfo, SessionManager.CurrentSession.AgentInfo);
            Assert.IsFalse(SessionManager.CurrentSession.IsAdmin);
            Assert.IsFalse(SessionManager.CurrentSession.IsCSDIT);
            Assert.AreEqual(SessionManager.CurrentSession.LoginAccountType, AccountType.AgentLevelk);
            Assert.AreEqual(SessionManager.CurrentSession.LoginUserId, agentInfo.AgentId);
            Assert.AreEqual(SessionManager.CurrentSession.LoginUserName, agentInfo.UserName);
            Assert.AreEqual(SessionManager.CurrentSession.OnUserId, 1);
            Assert.AreEqual(SessionManager.CurrentSession.OnLicenseeId, agentInfo.LicenseeId);
            Assert.AreEqual(SessionManager.CurrentSession.Language, model.Language);
        }

        [TearDown]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _agentController.Dispose();
            _captchaController.Dispose();
        }
    }
}