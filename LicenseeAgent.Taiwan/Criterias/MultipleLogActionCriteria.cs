// ----------------------------------------------------------------------
// <copyright file="MultipleLogActionCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class MultipleLogActionCriteria : LogActionCriteria
    {
        [SpParam(SqlParamName = "strXML")]
        public string LogXml { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && !string.IsNullOrEmpty(LogXml);
        }
    }
}