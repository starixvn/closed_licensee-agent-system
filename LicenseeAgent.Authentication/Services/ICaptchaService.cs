namespace LicenseeAgent.Authentication.Services
{
    using LicenseeAgent.Authentication.Entities;

    /// <summary>
    /// ICaptchaService class.
    /// </summary>
    public interface ICaptchaService
    {
        Captcha Generate();
    }
}