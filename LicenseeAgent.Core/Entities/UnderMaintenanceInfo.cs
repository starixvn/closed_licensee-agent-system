namespace LicenseeAgent.Core.Entities
{
    using System;

    /// <summary>
    /// Under Maintenance Page Model.
    /// </summary>
    public class UnderMaintenanceInfo
    {
        public UnderMaintenanceInfo()
        {
            Status = false;
            var nowHour = DateTime.Today.AddHours(DateTime.Now.Hour);
            TimeFrom = nowHour.ToShortTimeString();
            TimeTo = nowHour.AddHours(1).ToShortTimeString();
        }

        public bool Status { get; set; }

        public string TimeFrom { get; set; }

        public string TimeTo { get; set; }
    }
}