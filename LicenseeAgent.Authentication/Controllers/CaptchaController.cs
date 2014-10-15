namespace LicenseeAgent.Authentication.Controllers
{
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using LicenseeAgent.Authentication.Services;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// CaptchaController class.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    public class CaptchaController : BaseController
    {
        #region Variables

        private readonly ICaptchaService _captchaService;

        #endregion Variables

        #region Constructors

        public CaptchaController()
            : this(null)
        {
        }

        public CaptchaController(
            ICaptchaService captchaService)
        {
            _captchaService = captchaService ?? new CaptchaService();
        }

        #endregion Constructors

        /// <summary>
        /// Generate Captcha Image.
        /// </summary>
        /// <returns>
        /// Return image.
        /// </returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var captcha = _captchaService.Generate();
            using (var memoryStream = new MemoryStream())
            {
                Session[Constants.AppSessionCaptcha] = captcha.Text;
                captcha.Image.Save(memoryStream, ImageFormat.Png);
                return File(memoryStream.GetBuffer(), "image/png");
            }
        }
    }
}