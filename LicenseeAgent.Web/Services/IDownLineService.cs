namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Web.Criteria;

    public interface IDownLineService
    {
        bool IsDownLine(CheckDownLineBelongCriteria criteria);

        bool IsDirectDownLine(CheckDownLineBelongCriteria criteria);

        bool IsIndirectDownLine(CheckDownLineBelongCriteria criteria);

        bool IsDownLineTicket(TicketDownLineCriteria criteria);

        bool IsDownLineReferenceNumber(ReferenceNumberCriteria criteria);
    }
}