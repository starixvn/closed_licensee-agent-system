namespace LicenseeAgent.Account.Services
{
    using LicenseeAgent.Account.Entities.Account;
    using LicenseeAgent.Account.Utility;

    /// <summary>
    /// CaptchaService class.
    /// </summary>
    public class CaptchaService : ICaptchaService
    {
        private readonly CaptchaImage _captchaImage;

        public CaptchaService()
            : this(null)
        {
        }

        public CaptchaService(CaptchaImage captchaImage)
        {
            _captchaImage = captchaImage ?? new CaptchaImage();
        }

        public Captcha Generate()
        {
            var text = _captchaImage.RandomText();
            var image = _captchaImage.RenderImage();
            return new Captcha
            {
                Text = text,
                Image = image
            };
        }
    }
}