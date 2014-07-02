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
        /// Get member service.
        /// </summary>
        /// <returns>Member service.</returns>
        public static IUnderMaintenanceService GetUnderMaintenanceService()
        {
            var maintenanceService = Substitute.For<IUnderMaintenanceService>();

            maintenanceService.GetUnderMaintenanceInfo().Returns(new UnderMaintenanceInfo());

            return maintenanceService;
        }
    }
}
