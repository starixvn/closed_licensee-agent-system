namespace LicenseeAgent.Taiwan.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Results;

    public interface IMemberService
    {
        ResponseResult CheckUserName(CheckUserNameAction action);

        ResponseResult InsertNewMember(InsertNewMemberAction action, bool isReinvoke = false, int logId = 0);

        ResponseResult ResetPassword(ResetPasswordAction action, bool isReinvoke = false, int logId = 0);

        ResponseResult UpdateMemberInfo(UpdateMemberInfoAction action, bool isReinvoke = false, int logId = 0);

        UpdateMemberStatusResult UpdateMemberStatus(UpdateMemberStatusAction action, bool isReinvoke = false, int logId = 0);

        ResponseResult GetMemberInfo<T>(T memberEntity, GetMemberInfoAction action);

        GetMemberInfoListResult GetMemberInfoList<T>(IList<T> memberEntitys, GetMemberInfoListAction action);

        ResponseResult GetDepositMemberInfo<T>(T memberEntity, GetDepositMemberInfoAction action);

        GetDepositMemberInfoListResult GetDepositMemberInfoList(GetDepositMemberInfoListAction action);

        ResponseResult UpdateAgentId(UpdateAgentIdAction action, bool isReinvoke = false, int logId = 0);
    }
}