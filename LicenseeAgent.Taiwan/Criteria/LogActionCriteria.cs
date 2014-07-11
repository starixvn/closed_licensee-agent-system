namespace LicenseeAgent.Taiwan.Criteria
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data.Repository;
    using LicenseeAgent.Taiwan.Actions;

    /// <summary>
    /// LogActionCriteria class.
    /// </summary>
    public abstract class LogActionCriteria : CriteriaBase
    {
        protected LogActionCriteria(LogAction logAction)
        {
            Url = logAction.Url.ToString();
            ApiMethod = logAction.ActionName;
        }

        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Verified.")]
        public string Url { get; private set; }

        public string ApiMethod { get; private set; }

        public string Doing { get; set; }

        public int ApiErrCode { get; set; }

        public string ApiErrMsg { get; set; }

        public override bool IsValid()
        {
            return LicenseeId > 0 && UserId > 0 && !string.IsNullOrEmpty(Doing);
        }
    }
}