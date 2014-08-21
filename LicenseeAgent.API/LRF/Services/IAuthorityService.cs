namespace LicenseeAgent.API.LRF.Services
{
    using LicenseeAgent.API.LRF.Criteria;
    using LicenseeAgent.API.LRF.Entities;

    /// <summary>
    /// IAuthorityService class.
    /// </summary>
    public interface IAuthorityService
    {
        void InsertInternalAuthority(InsertInternalAuthorityCriteria criteria);

        InternalInfo GetInternalAuthority(GetInternalAuthorityCriteria criteria);
    }
}