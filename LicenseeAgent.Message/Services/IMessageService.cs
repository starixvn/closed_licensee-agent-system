// -----------------------------------------------------------------------
// <copyright file="IMessageService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2014.02.24">Created and Implement Interface for the message.</change>
// </history>

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
        IEnumerable<MessageEntity> GetMessageList(SearchMessageCriteria criteria);
    }
}