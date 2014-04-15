// -----------------------------------------------------------------------
// <copyright file="IMessageService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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