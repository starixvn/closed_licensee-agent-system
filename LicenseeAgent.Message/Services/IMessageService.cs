namespace LicenseeAgent.Message.Services
{
    using System.Collections.Generic;
    using Criteria;
    using Entities;

    /// <summary>
    /// Message interface.
    /// </summary>
    public interface IMessageService
    {
        IList<MessageEntity> GetMessageList(SearchMessageCriteria criteria);
    }
}