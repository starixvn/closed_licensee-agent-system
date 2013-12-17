// --------------------------------------------------------------------------
// <copyright file="ModelSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.1.11">Created</change>
// </history>

namespace LicenseeAgent.Account.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ModelSettings
    {
        private static IEnumerable<SelectListItem> usernameIndexes;

        static ModelSettings()
        {
            usernameIndexes = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList().Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = x.ToString()
                });
        }

        public static IEnumerable<SelectListItem> UsernameIndexes
        {
            get
            {
                return usernameIndexes;
            }
        }
    }
}