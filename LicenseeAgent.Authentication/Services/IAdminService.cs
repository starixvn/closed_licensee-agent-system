namespace LicenseeAgent.Authentication.Services
{
    using LicenseeAgent.Authentication.Criteria;
    using LicenseeAgent.Authentication.Criteria.Admin;
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// IAdminService class.
    /// </summary>
    public interface IAdminService
    {
        string GetAdminSalt(AdminSaltSelectCriteria criteria);

        AdminInfo AuthenticateAdmin(AdminLoginCriteria criteria);

        void AdminCheckIn(AdminCheckInCriteria criteria);
    }
}