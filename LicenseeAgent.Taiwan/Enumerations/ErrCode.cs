/// <summary>
/// https://docs.google.com/a/nexcel.vn/spreadsheets/d/1HiYmXDQQZAL_Oj775F2O2oNcPmUhVklsNqD-0sExGvk/edit#gid=611402240
/// </summary>
namespace LicenseeAgent.Taiwan.Enumerations
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// ErrorCodes class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed.")]
    public enum GetDepositMemberInfoListErrCode : int
    {
        Success = 0,

        MemberNotExist = 1,

        SystemError = 2
    }
}