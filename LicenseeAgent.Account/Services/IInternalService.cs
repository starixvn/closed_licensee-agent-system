namespace LicenseeAgent.Account.Services
{
    using LicenseeAgent.Account.Criteria;
    using LicenseeAgent.API.LRF.Entities;
    using LicenseeAgent.Core.Entities;

    public interface IInternalService
    {
        string Encode(InternalLoginInfo internalLoginInfo);

        bool IsExpired(LoginInfo loginInfo);

        InternalLoginInfo Decode(string authKey);

        AgentInfo AgentInternalLogin(AgentInternalLoginCriteria criteria);
    }
}