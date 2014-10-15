namespace LicenseeAgent.API.LRF.Services
{
    using System;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using LicenseeAgent.API.LRF.Criteria;
    using LicenseeAgent.API.LRF.Entities;

    /// <summary>
    /// AuthorityService class.
    /// </summary>
    public class AuthorityService : IAuthorityService
    {
        /// <summary>
        /// Inserts the internal authority.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void InsertInternalAuthority(InsertInternalAuthorityCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (!criteria.IsValid())
            {
                throw new InvalidOperationException("Invalid criteria");
            }

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                objectDb.ExecuteNonQuery(param);
                var outputParams = objectDb.GetOuputParams();
                criteria.ErrCode = Converter.ToInt(outputParams["errcode"]);
                criteria.ErrMsg = Converter.ToString2(outputParams["errmsg"]);
                criteria.Guid = Converter.ToString2(outputParams["guid"]);
                criteria.DomainName = Converter.ToString2(outputParams["domainname"]);
            }
        }

        /// <summary>
        /// Gets the internal authority.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns internal information.</returns>
        public InternalInfo GetInternalAuthority(GetInternalAuthorityCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (!criteria.IsValid())
            {
                throw new InvalidOperationException("Invalid criteria");
            }

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                var list = objectDb.Query<InternalInfo>(param);
                var outputParams = objectDb.GetOuputParams();
                criteria.ErrCode = Converter.ToInt(outputParams["errcode"]);
                return list.Any() ? list.First() : null;
            }
        }
    }
}