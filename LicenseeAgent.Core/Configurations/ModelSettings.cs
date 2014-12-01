namespace LicenseeAgent.Core.Configurations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// ModelSettings class.
    /// </summary>
    public static class ModelSettings
    {
        #region Variables

        private static IList<SelectListItem> _languages = new List<SelectListItem>
        {
            new SelectListItem { Text = "English", Value = "en-US" },
            new SelectListItem { Text = "繁體中文", Value = "zh-TW" },
            new SelectListItem { Text = "简体中文", Value = "zh-CN" }
        };

        private static IList<SelectListItem> _betlistStatuses = new List<SelectListItem>
        {
            new SelectListItem { Text = Las.All, Value = string.Empty },
            new SelectListItem { Text = Las.Waiting, Value = ((int)TicketStatus.Waiting).ToString() },
            new SelectListItem { Text = Las.Running, Value = ((int)TicketStatus.Running).ToString() },
            new SelectListItem { Text = Las.Won, Value = ((int)TicketStatus.Won).ToString() },
            new SelectListItem { Text = Las.Lose, Value = ((int)TicketStatus.Lose).ToString() },
            new SelectListItem { Text = Las.Draw, Value = ((int)TicketStatus.Draw).ToString() },
            new SelectListItem { Text = Las.Void, Value = ((int)TicketStatus.Void).ToString() },
            new SelectListItem { Text = Las.Reject, Value = ((int)TicketStatus.Reject).ToString() },
            new SelectListItem { Text = Las.Refund, Value = ((int)TicketStatus.Refund).ToString() },
        };

        private static IList<SelectListItem> _statusList = new List<SelectListItem>
        {
            new SelectListItem { Text = Las.All, Value = string.Empty },
            new SelectListItem { Text = Las.Open, Value = ((int)AccountStatus.Open).ToString() },
            new SelectListItem { Text = Las.Closed, Value = ((int)AccountStatus.Closed).ToString() }
        };

        #endregion Variables

        #region Properties

        public static string DefaultLanguage
        {
            get
            {
                return _languages.First().Value;
            }
        }

        public static IList<SelectListItem> Languages
        {
            get
            {
                return _languages;
            }
        }

        public static IList<SelectListItem> BetListStatuses
        {
            get
            {
                return _betlistStatuses;
            }
        }

        public static IList<SelectListItem> StatusList
        {
            get
            {
                return _statusList;
            }
        }

        #endregion Properties
    }
}