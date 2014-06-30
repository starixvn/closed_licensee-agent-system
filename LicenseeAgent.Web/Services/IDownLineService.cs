namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Web.Criteria;

    public interface IDownLineService
    {
        bool IsDownLine(CheckDownlineBelongCriteria criteria);

        bool IsDirectDownLine(CheckDownlineBelongCriteria criteria);

        bool IsIndirectDownLine(CheckDownlineBelongCriteria criteria);

        bool IsDownLineTicket(TicketDownLineCriteria criteria);

        bool IsDownLineReferenceNumber(ReferenceNumberCriteria criteria);
    }
}