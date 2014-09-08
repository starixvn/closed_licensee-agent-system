namespace LicenseeAgent.Account.Entities.Account
{
    using System.Drawing;

    /// <summary>
    /// Captcha class.
    /// </summary>
    public class Captcha
    {
        public string Text { get; set; }

        public Image Image { get; set; }
    }
}