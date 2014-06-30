namespace LicenseeAgent.Taiwan.Attributes
{
    using System;

    /// <summary>
    /// ServiceAttribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ServiceAttribute : Attribute
    {
        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public bool IsParamAction { get; set; }
    }
}