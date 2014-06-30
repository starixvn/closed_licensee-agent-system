namespace LicenseeAgent.Account.ViewResults
{
    /// <summary>
    /// SignInViewResult class.
    /// </summary>
    public class SignInViewResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show confirm dialog.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is confirm]; otherwise, <c>false</c>.
        /// </value>
        public bool IsConfirm { get; set; }

        public string Message { get; set; }
    }
}