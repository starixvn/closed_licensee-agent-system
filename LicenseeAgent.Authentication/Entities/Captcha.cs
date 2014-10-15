namespace LicenseeAgent.Authentication.Entities
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