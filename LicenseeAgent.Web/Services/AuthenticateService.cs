namespace LicenseeAgent.Web.Services
{
    using System.Web.Security;

    /// <summary>
    /// AuthenticateService class.
    /// </summary>
    public class AuthenticateService : IAuthenticateService
    {
        public void Login(string userName)
        {
            Login(userName, true);
        }

        public void Login(string userName, bool persistent)
        {
            FormsAuthentication.SetAuthCookie(userName, persistent);
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}