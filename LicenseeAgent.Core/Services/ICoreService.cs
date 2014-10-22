namespace LicenseeAgent.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;

    public interface ICoreService
    {
        SelectBalanceUpDay BalanceUpDay { get; }

        IList<LicenseeDomain> LicenseeDomains { get; }

        Uri GetLicenseeDomain(int licenseeId);

        int? GetLicenseeId(string licenseeName);

        int? GetLicenseeId(HttpRequestBase request);

        IList<ProductInfo> GetProducts(int licenseeId);

        bool HasProductOffered(int licenseeId, int productId);

        bool HasProductOffered(int licenseeId, ProductCode product);

        bool HasProductOffered(IList<ProductInfo> productInfos, int productId);

        AgentInfo GetAgentInfo(int agentId);
    }
}