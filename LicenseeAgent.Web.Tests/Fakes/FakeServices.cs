namespace LicenseeAgent.Web.Tests.Fakes
{
    using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Verified")]
        public static IUnderMaintenanceService GetUnderMaintenanceService()
        {
            var maintenanceService = Substitute.For<IUnderMaintenanceService>();

            maintenanceService.UnderMaintenanceInfo.Returns(new UnderMaintenanceInfo());

            return maintenanceService;
        }
    }
}