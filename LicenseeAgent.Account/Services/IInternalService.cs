namespace LicenseeAgent.Account.Services
{
    using LicenseeAgent.Account.Criteria;
    using LicenseeAgent.Core.Entities;

    public interface IInternalService
    {
        AgentInfo AgentInternalLogin(AgentInternalLoginCriteria criteria);

        string Encode(InternalLoginInfo internalLoginInfo);

        InternalLoginInfo Decode(string authKey);
    }
}