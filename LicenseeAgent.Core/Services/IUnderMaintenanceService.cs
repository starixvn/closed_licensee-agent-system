namespace LicenseeAgent.Core.Services
{
    using LicenseeAgent.Core.Entities;

    public interface IUnderMaintenanceService
    {
        UnderMaintenanceInfo UnderMaintenanceInfo { get; }

        void Dispose();
    }
}