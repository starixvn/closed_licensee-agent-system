// -----------------------------------------------------------------------
// <copyright file="PageLang.cs" company="Nexcel">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class PageLang : Dictionary<string, string>
    {
        public PageLang(params string[] resourceKeys)
        {
            foreach (var key in resourceKeys)
            {
                Add(key, Las.ResourceManager.GetString(key));
            }
        }

        protected PageLang(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}