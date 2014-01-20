// -----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Controllers
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Fanex.Common;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.Account.ViewModels;
    using LicenseeAgent.Account.ViewResults;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Core;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// Account controllers.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        private readonly ICurrencyService _currencyService;

        public AccountController(IAccountService accountService, ICurrencyService currencyService)
        {
            _accountService = accountService;
            _currencyService = currencyService;
        }

        [HttpGet]
        [AppAuthorize(IsValidateAgent = false)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Generate Captcha Image.
        /// </summary>
        /// <param name="noisy">Checking noise or not.</param>
        /// <returns>Return image.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult CaptchaImage(bool noisy = true)
        {
            var captchaImage = new CaptchaImage
            {
                Width = 160,
                Height = 32
            };

            using (var mem = new MemoryStream())
            {
                var img = captchaImage.RenderImage();
                Session[Constants.AppSessionCaptcha] = captchaImage.Text;
                img.Save(mem, System.Drawing.Imaging.ImageFormat.Png);

                return File(mem.GetBuffer(), "image/png");
            }
        }

        /// <summary>
        /// Sign-in page.
        /// </summary>
        /// <returns>Returns Sign-in HTML content.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignIn()
        {
            if (AppSettings.IsAdminSite)
            {
                return RedirectToAction("SignIn", "Internal");
            }

            var model = new AgentSignInViewModel
            {
                Language = Language,
                Captcha = AppSettings.LocalTest ? "999999" : string.Empty
            };

            return View(model);
        }

        /// <summary>
        /// Post SignIn Page.
        /// </summary>
        /// <param name="model">Input model.</param>
        /// <returns>Redirect to home page.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(AgentSignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var captcha = Session[Constants.AppSessionCaptcha] as string;
                if (string.IsNullOrEmpty(captcha))
                {
                    ModelState.AddModelError("Captcha", "Session Error. Please contact the administrator");
                }
                else
                {
                    if ((AppSettings.LocalTest && model.Captcha == "999999") || string.Compare(captcha, model.Captcha, true) == 0)
                    {
                        var licenseeId = DomainMapper.GetLicenseeId(Request);
                        if (licenseeId == null)
                        {
                            ModelState.AddModelError(string.Empty, string.Format(Las.NoLicenseeDomainFound, Request.Url.Host));
                        }
                        else
                        {
                            if (AuthenticateUser(model, licenseeId.Value))
                            {
                                return CheckAgentPasswordStatus(model);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, Las.IncorrectCaptchaMsg);
                    }
                }
            }

            // Always remove captcha value from input because it can't be reused.
            if (!ModelState.IsValid && !AppSettings.LocalTest)
            {
                ModelState.Remove("Captcha");
                model.Captcha = string.Empty;
            }

            return View(model);
        }

        /// <summary>
        /// Checks the agent password status.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns view.</returns>
        [NonAction]
        public ActionResult CheckAgentPasswordStatus(AgentSignInViewModel model)
        {
            // check account password
            var agentInfo = CurrentSession.AgentInfo;
            var passwordStatus = PasswordRule.GetPasswordStatus(agentInfo, agentInfo.LastLoggedTime);
            if (passwordStatus == PasswordStatus.Normal)
            {
                return Redirect(FormsAuthentication.DefaultUrl);
            }
            else
            {
                ViewBag.Result = new SignInViewResult
                {
                    IsConfirm = passwordStatus == PasswordStatus.Alert,
                    Message = PasswordRule.GetStatusMessage(passwordStatus, agentInfo.PwdExpiry)
                };
            }

            return View(model);
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <returns>Returns view.</returns>
        [HttpGet]
        [AppAuthorize(Role = AccountType.Agent, IsValidateAgent = false)]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns a result.</returns>
        [HttpPost]
        [AppAuthorize(Role = AccountType.Agent, IsValidateAgent = false)]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(AgentChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldPassword = Cryptography.CreateHash(model.CurrentPassword, CurrentSession.AgentInfo.UserSalt);
                var newPassword = Cryptography.CreateHash(model.NewPassword, CurrentSession.AgentInfo.UserSalt);
                var criteria = new AgentChangePwdCriteria
                {
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    OldPwd = oldPassword,
                    Pwd = newPassword,
                    LoginIp = Request.GetClientIp()
                };

                _accountService.AgentChangePassword(criteria);
                if (criteria.MsgId != 0)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.MsgId));
                }
                else
                {
                    // after changing password successfully, re-update some status to not validate user password again.
                    CurrentSession.AgentInfo.IsFirstLogin = false;
                    CurrentSession.AgentInfo.PwdExpiry = DateTime.Today.AddDays(PasswordRule.PasswordExpiredTotalDays);
                    ViewBag.Result = Las.UpdateSuccessfully;
                }
            }

            return View();
        }

        /// <summary>
        /// Post ReCheckIn.
        /// </summary>
        /// <param name="licenseeId">The licensee identifier.</param>
        /// <param name="currencyId">The currency identifier.</param>
        /// <returns>
        /// Return true or false.
        /// </returns>
        [HttpPost]
        [AjaxOnly]
        [AppAuthorize(IsValidateAgent = false)]
        public JsonResult ReCheckIn(int? licenseeId, int? currencyId)
        {
            var errCode = Result.FailureValue;
            if ((CurrentSession.AdminInfo != null || CurrentSession.LicenseeInfo != null) && CurrentSession.IsDirectSignIn)
            {
                if (CurrentSession.AgentInfo != null)
                {
                    var isValidLicensee = licenseeId == 0 ? true : licenseeId == CurrentSession.AgentInfo.LicenseeId;
                    var isValidCurrency = currencyId == 0 ? true : currencyId == CurrentSession.AgentInfo.CurrencyId;
                    if (!isValidLicensee || !isValidCurrency)
                    {
                        return Json(new
                        {
                            ErrCode = errCode,
                            Reload = true
                        });
                    }
                }

                if (CurrentSession.AdminInfo != null)
                {
                    errCode = AdminReCheckIn();
                }
                else if (CurrentSession.LicenseeInfo != null)
                {
                    errCode = Result.SuccessValue;
                }
            }
            else
            {
                if (CurrentSession.AgentInfo != null)
                {
                    if (CurrentSession.IsDirectSignIn)
                    {
                        errCode = AgentReCheckIn();
                    }
                    else
                    {
                        errCode = AgentReCheckInLicensee();
                    }
                }
            }

            var result = new Result(
                errCode,
                errCode != Result.SuccessValue ? CurrentSession.IsDirectSignIn ? Las.AlertKickoutMessage : Las.AlertAdminLicenseeChanged : string.Empty);
            return Json(result);
        }

        [NonAction]
        public int AgentReCheckInLicensee()
        {
            var criteria = new AgentReCheckInLicenseeCriteria
            {
                OnAdminId = CurrentSession.OnUserId,
                LicenseeId = CurrentSession.OnLicenseeId
            };

            _accountService.AgentReCheckInLicensee(criteria);
            return criteria.ErrCode;
        }

        /// <summary>
        /// Agent re-check in on current session to kick out when having another login session behalf on current agent.
        /// </summary>
        /// <returns>Returns true whether having another login session.</returns>
        [NonAction]
        public int AgentReCheckIn()
        {
            var criteria = new AgentReCheckInCriteria
            {
                OnAgentId = CurrentSession.OnUserId
            };

            _accountService.AgentReCheckIn(criteria);
            return criteria.ErrCode;
        }

        /// <summary>
        /// Admin re-check in on current session to kick out when having another login session behalf on current admin.
        /// </summary>
        /// <returns>
        /// Returns true whether having another login session.
        /// </returns>
        [NonAction]
        public int AdminReCheckIn()
        {
            var criteria = new AdminReCheckInCriteria
            {
                OnAdminId = CurrentSession.OnUserId
            };

            _accountService.AdminReCheckIn(criteria);
            return criteria.ErrCode;
        }

        /// <summary>
        /// Sign out action.
        /// </summary>
        /// <returns>Redirect to Login Page.</returns>
        [AppAuthorize(IsValidateAgent = false)]
        public ActionResult SignOut()
        {
            if (CurrentSession.AdminInfo != null && CurrentSession.IsDirectSignIn)
            {
                AdminUserLogout();
            }
            else if (CurrentSession.AgentInfo != null && CurrentSession.IsDirectSignIn)
            {
                AgentUserLogout();
            }

            FormsAuthentication.SignOut();
            SessionManager.ClearAll();
            return Redirect(FormsAuthentication.LoginUrl);
        }

        /// <summary>
        /// Remove Agent Session.
        /// </summary>
        [NonAction]
        public void AgentUserLogout()
        {
            var criteria = new AgentUserLogoutCriteria
            {
                OnAgentId = CurrentSession.OnUserId
            };

            _accountService.AgentUserLogout(criteria);
        }

        /// <summary>
        /// Remove Admin Session.
        /// </summary>
        [NonAction]
        public void AdminUserLogout()
        {
            var criteria = new AdminUserLogoutCriteria
            {
                OnAdminId = CurrentSession.OnUserId
            };

            _accountService.AdminUserLogout(criteria);
        }

        /// <summary>
        /// Call to change language.
        /// </summary>
        /// <param name="language">The selected language.</param>
        /// <returns>Returns action status.</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangeLanguage(string language)
        {
            var result = false;
            if (Language != language)
            {
                Language = language;
                result = true;
            }

            return Json(new
            {
                isChanged = result
            });
        }

        [HttpGet]
        [AppAuthorize(IsValidateAgent = false)]
        public ActionResult Avatar()
        {
            var avatarImage = Path.Combine(SitePath.GlobalImagePath, "avatar.png");
            var base64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(avatarImage));
            var imgSrc = string.Format("data:image/gif;base64,{0}", base64);
            return Content(imgSrc);
        }

        /// <summary>
        /// Statistics Total Open/Close down line.
        /// </summary>
        /// <returns>View result.</returns>
        public ViewResult TotalOpenClose()
        {
            return View();
        }

        /// <summary>
        /// Statistics top 10 winning members.
        /// </summary>
        /// <returns>View result.</returns>
        public ViewResult TopWinning()
        {
            return View();
        }

        public ViewResult TopTurnover()
        {
            return View();
        }

        /// <summary>
        /// Validates the Agent user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="licenseeId">Name of the domain.</param>
        /// <returns>
        /// Returns true whether username/password is valid.
        /// </returns>
        private bool AuthenticateUser(AgentSignInViewModel model, int licenseeId)
        {
            var userSaltCriteria = new AgentSaltSelCriteria
            {
                UserName = model.Username,
                LicenseeId = licenseeId
            };

            var userSalt = _accountService.GetAgentSalt(userSaltCriteria);
            if (userSaltCriteria.ErrCode != 0)
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(userSaltCriteria.ErrCode));
                return false;
            }

            var clientInfo = Request.GetClientInfo();
            var criteria = new AgentLoginCriteria
            {
                UserName = model.Username,
                UserPwd = model.Password,
                UserSalt = userSalt,
                LoginIp = clientInfo.Ip,
                LicenseeId = licenseeId,
                BrowserName = clientInfo.BrowserName,
                Platform = clientInfo.Platform,
                CountryCode = clientInfo.CountryCode
            };

            var agentInfo = _accountService.AuthenticateAgent(criteria);
            if (criteria.ErrCode != 0)
            {
                var errorMessage = GetErrMsg(criteria.ErrCode);
                errorMessage = string.Format(errorMessage, criteria.LastChangedPwdDate.ToShortDateString());
                ModelState.AddModelError(string.Empty, errorMessage);
                return false;
            }
            else
            {
                CheckIn(agentInfo, model.Language);
                return true;
            }
        }

        /// <summary>
        /// Store current user session.
        /// </summary>
        /// <param name="agentInfo">The agent information.</param>
        /// <param name="language">The language.</param>
        private void CheckIn(AgentInfo agentInfo, string language)
        {
            var criteria = new AgentCheckInCriteria
            {
                AgentId = agentInfo.AgentId,
                Ip = Request.GetClientIp(),
                NetSessionId = Session.SessionID
            };

            _accountService.AgentCheckIn(criteria);
            SessionManager.CurrentSession = new CurrentSession(agentInfo, criteria.OnUserId, language);
            FormsAuthentication.SetAuthCookie(agentInfo.UserName, true);
        }        
    }
}