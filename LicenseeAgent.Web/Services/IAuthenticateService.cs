namespace LicenseeAgent.Web.Services
{
    /// <summary>
    /// IAuthenticateService interface.
    /// </summary>
    public interface IAuthenticateService
    {
        void Login(string userName);

        void Login(string userName, bool persistent);

        void Logout();
    }
}