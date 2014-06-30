namespace LicenseeAgent.Web.ViewModels
{
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// Modal view modal.
    /// </summary>
    public class HomeViewModel
    {
        public TopMenu TopMenu { get; set; }

        public bool IsAgentChangePassword { get; set; }
    }
}