// --------------------------------------------------------------------------
// <copyright file="InvalidCriteriaException.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class InvalidCriteriaException : Exception
    {
        public InvalidCriteriaException(ICriteria criteria)
        {
            Criteria = criteria;
        }

        public ICriteria Criteria { get; private set; }

        public override string Message
        {
            get
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("\nSp Name: {0}", Criteria.GetSettingKey());
                var param = Criteria.GetSpParams();
                foreach (var key in param.Keys)
                {
                    var value = param[key];
                    stringBuilder.AppendFormat("\n{0} = {1}", key, value);
                }

                return stringBuilder.ToString();
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}