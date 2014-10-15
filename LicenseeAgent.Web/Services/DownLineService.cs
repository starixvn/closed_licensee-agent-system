namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Criteria;

    /// <summary>
    /// Down-line service.
    /// </summary>
    internal class DownLineService : QueryService, IDownLineService
    {
        /// <summary>
        /// Check if the target user is child of current user.
        /// </summary>
        /// <param name="criteria">Down line criteria.</param>
        /// <returns>Return boolean value indicating the down line value.</returns>
        public bool IsDownLine(CheckDownLineBelongCriteria criteria)
        {
            if (criteria.CustId == criteria.TargetCustId)
            {
                return true;
            }

            Execute(criteria);
            return criteria.IsBelongTo;
        }

        /// <summary>
        /// Check if the target user is direct child of current user.
        /// </summary>
        /// <param name="criteria">Down line criteria.</param>
        /// <returns>Return boolean value indicating the direct down line value.</returns>
        public bool IsDirectDownLine(CheckDownLineBelongCriteria criteria)
        {
            if (criteria.CustId == criteria.TargetCustId)
            {
                return true;
            }

            Execute(criteria);
            return criteria.IsBelongTo && criteria.IsDirectDownLine;
        }

        public bool IsIndirectDownLine(CheckDownLineBelongCriteria criteria)
        {
            if (criteria.CustId == criteria.TargetCustId)
            {
                return true;
            }

            Execute(criteria);
            return criteria.IsBelongTo && !criteria.IsDirectDownLine;
        }

        /// <summary>
        /// Check if the ticket is belong to current user.
        /// </summary>
        /// <param name="criteria">Ticket down line criteria.</param>
        /// <returns>Return boolean value indicating the down line value.</returns>
        public bool IsDownLineTicket(TicketDownLineCriteria criteria)
        {
            return Execute<bool>(criteria);
        }

        /// <summary>
        /// Check if the reference number is belong to current user.
        /// </summary>
        /// <param name="criteria">Reference number down line criteria.</param>
        /// <returns>Return boolean value indicating the down line value.</returns>
        public bool IsDownLineReferenceNumber(ReferenceNumberCriteria criteria)
        {
            return Execute<bool>(criteria);
        }
    }
}