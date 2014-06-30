namespace LicenseeAgent.Message.Services
{
    using System.Collections.Generic;
    using Core.Services;
    using Criteria;
    using Entities;

    /// <summary>
    /// Service Message.
    /// </summary>
    public class MessageService : QueryService, IMessageService
    {
        /// <summary>
        /// Get message list.
        /// </summary>
        /// <param name="criteria">Search message criteria.</param>
        /// <returns>List message.</returns>
        public IList<MessageEntity> GetMessageList(SearchMessageCriteria criteria)
        {
            return GetEntites<MessageEntity>(criteria);
        }
    }
}