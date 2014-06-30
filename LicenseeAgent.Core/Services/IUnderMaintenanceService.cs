namespace LicenseeAgent.Core.Services
{
    using LicenseeAgent.Core.Entities;

    public interface IUnderMaintenanceService
    {
        UnderMaintenanceInfo GetUnderMaintenanceInfo();

        void Dispose();
    }
}