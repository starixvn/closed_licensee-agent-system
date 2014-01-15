// ----------------------------------------------------------------------
// <copyright file="MemberService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Filter;
    using LicenseeAgent.Taiwan.Results;
    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberService : IMemberService
    {
        public ResponseResult CheckUserName(IAction action)
        {
            return ServiceWrapper.GetResponseData(action);
        }

        public ResponseResult InsertNewMember(IAction action)
        {
            return ServiceWrapper.GetResponseData(action);
        }

        public ResponseResult ResetPassword(IAction action)
        {
            return ServiceWrapper.GetResponseData(action);
        }

        public ResponseResult UpdateMemberInfo(IAction action)
        {
            return ServiceWrapper.GetResponseData(action);
        }

        public ResponseResult UpdateMemberStatus(IAction action)
        {
            return ServiceWrapper.GetResponseData(action);
        }

        public T GetMemberInformation<T>(T memberEntity, IAction action)
        {
            var response = ServiceWrapper.GetResponseData(action);

            if (response.ErrorCode == 0)
            {
                var memberResult = JsonConvert.DeserializeObject<MemberResult>(response.Data);

                ServiceMapper.MapObject<MemberResult, T>(memberResult, memberEntity);
            }

            return memberEntity;
        }

        public IList<T> GetMemberInformations<T>(IList<T> memberEntitys, IAction action)
        {
            var response = ServiceWrapper.GetResponseData(action);

            if (response.ErrorCode == 0)
            {
                var memberResults = JsonConvert.DeserializeObject<IList<MemberResult>>(response.Data);

                ServiceMapper.MapObjects<MemberResult, T>(memberResults, memberEntitys, new MemberFilter());
            }

            return memberEntitys;
        }
    }
}
