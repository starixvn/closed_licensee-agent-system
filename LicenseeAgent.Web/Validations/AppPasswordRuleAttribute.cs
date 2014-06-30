namespace LicenseeAgent.Web.Validations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
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

        public override bool IsValid(object value)
        {
            return base.IsValid(value) ? !value.ToString().Any(c => c > 255) : false;
        }
    }
}