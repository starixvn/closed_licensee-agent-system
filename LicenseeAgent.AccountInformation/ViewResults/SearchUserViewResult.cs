namespace LicenseeAgent.AccountInformation.ViewResults
{
    /// <summary>
    /// SearchUserViewResult class.
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