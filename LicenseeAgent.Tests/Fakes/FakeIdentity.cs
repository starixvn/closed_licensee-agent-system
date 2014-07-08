namespace LicenseeAgent.Tests.Fakes
{
    using System.Security.Principal;

    /// <summary>
    /// FakeIdentity class.
    /// </summary>
    public class FakeIdentity : IIdentity
    {
        private readonly string _name;

        public FakeIdentity(string userName)
        {
            _name = userName;
        }

        public string AuthenticationType
        {
            get { return "forms"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(_name); }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}