namespace LicenseeAgent.Web.Tests.Fakes
{
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Services;
    using NSubstitute;

    /// <summary>
    /// FakeServices class.
    /// </summary>
    public static class FakeServices
    {
        /// <summary>
        /// Gets member service.
        /// </summary>
        /// <value>
        /// Returns under maintenance service.
        /// </value>
        public static IUnderMaintenanceService UnderMaintenanceService
        {
            get
            {
                var maintenanceService = Substitute.For<IUnderMaintenanceService>();
                maintenanceService.UnderMaintenanceInfo.Returns(new UnderMaintenanceInfo());
                return maintenanceService;
            }
        }
    }
}