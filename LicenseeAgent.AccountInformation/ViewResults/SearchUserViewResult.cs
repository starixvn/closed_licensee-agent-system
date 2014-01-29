// ----------------------------------------------------------------------
// <copyright file="SearchUserViewResult.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.ViewResults
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SearchUserViewResult
    {
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public bool IsChangeCurrency { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }
    }
}