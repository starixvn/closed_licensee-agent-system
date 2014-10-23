namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Web.Entities;

    public interface IDownLinePermissionService
    {
        bool CheckForEditingCustomerSetting(CustomerSettingBelonging customerSettingBelonging);

        bool IsDownLineMember(int memberId);

        bool IsDownLineAgent(int agentId);

        bool IsDirectDownLineAgent(int agentId);

        bool IsDirectDownLineMember(int memberId);

        bool IsDirectDownLineMember(int agentId, int memberId);

        bool IsIndirectDownLineAgent(int agentId);

        bool IsIndirectDownLineMember(int memberId);

        bool IsDownLineTransaction(long transactionId, int sportType, int betType);

        bool IsDownLineReferenceNumber(long referenceNumber);
    }
}