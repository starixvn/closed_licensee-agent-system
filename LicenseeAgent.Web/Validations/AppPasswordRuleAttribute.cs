// --------------------------------------------------------------------------
// <copyright file="AppPasswordRuleAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Web.Validations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;

    /// <summary>
    /// Password Rule Validation Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class AppPasswordRuleAttribute : RegularExpressionAttribute
    {
        public AppPasswordRuleAttribute()
            : base(PasswordRule.PasswordPattern)
        {
            ErrorMessageResourceName = "PasswordSecurity";
            ErrorMessageResourceType = typeof(Las);
        }
    }
}