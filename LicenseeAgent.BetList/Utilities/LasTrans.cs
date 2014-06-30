namespace LicenseeAgent.BetList.Utilities
{
    using Fanex.BetList.Core.Builder.TransBuilder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// Casino Games.
    /// </summary>
    public class LasTrans : Trans1
    {
        public override Trans_Template Render(ITicket ticket)
        {
            Template.transTime = Formatter.FormatDateTime(ticket.TransDate);
            Template.TransTime.refNo = ticket.TransId.ToString();
            return Template;
        }
    }
}