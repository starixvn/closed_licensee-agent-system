namespace LicenseeAgent.Taiwan.Criteria
{
    using Fanex.Data.Repository;
    using LicenseeAgent.Taiwan.Actions;

    /// <summary>
    /// MultipleLogActionCriteria class.
    /// </summary>
    public abstract class MultipleLogActionCriteria : LogActionCriteria
    {
        protected MultipleLogActionCriteria(LogAction logAction)
            : base(logAction)
        {
        }

        [SpParam(SqlParamName = "strXML")]
        public string LogXml { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && !string.IsNullOrEmpty(LogXml);
        }
    }
}