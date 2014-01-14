// ----------------------------------------------------------------------
// <copyright file="IMemberService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Taiwan.Actions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMemberService
    {
        bool CheckUserName(IAction action);

        bool InsertNewMember(IAction action);

        bool ResetPassword(IAction action);

        bool UpdateMemberInfo(IAction action);

        bool UpdateMemberStatus(IAction action);

        T GetMemberInformation<T>(T memberEntity, IAction action);

        IList<T> GetMemberInformations<T>(IList<T> memberEntitys, IAction action);
    }
}
