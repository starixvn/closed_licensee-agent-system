namespace LicenseeAgent.Account.Services
{
    using LicenseeAgent.Account.Entities.Account;

    /// <summary>
    /// ICaptchaService class.
    /// </summary>
    public interface ICaptchaService
    {
        Captcha Generate();
    }
}