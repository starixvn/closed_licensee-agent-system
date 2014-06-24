namespace LicenseeAgent.API.LRF.Services
{
    using LicenseeAgent.API.LRF.Entities;

    public interface IDomainService
    {
        LicenseeDomain GetLicenseeDomain(int licenseeId);
    }
}