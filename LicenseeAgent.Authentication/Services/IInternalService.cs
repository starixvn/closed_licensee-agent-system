namespace LicenseeAgent.Authentication.Services
{
    using LicenseeAgent.Authentication.Criteria;
    using LicenseeAgent.Authentication.Criteria.Internal;
    using LicenseeAgent.Core.Entities;

    public interface IInternalService
    {
        void LicenseeLogin(LicenseeAdminLoginCriteria criteria);

        void LicenseeCheckIn(AdminCheckInCriteria criteria);

        void AdminCheckInLicensee(AdminCheckInLicenseeCriteria criteria);

        void InternalLoginAsAgent(InternalLoginAsUserCriteria criteria);

        AgentInfo InternalGetAgentInfo(InternalGetAgentInfoCriteria criteria);

        string Encode(InternalLoginInfo internalLoginInfo);

        InternalLoginInfo Decode(string authKey);
    }
}