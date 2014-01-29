// --------------------------------------------------------------------------
// <copyright file="Breadcrumb.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Entities
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Web;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Breadcrumb : Collection<BreadcrumbItem>
    {
        public Breadcrumb(IList<BreadcrumbItem> list)
            : base(list)
        {
        }

        public static Breadcrumb FromString(string xml)
        {
            var list = ObjectSerializer<BreadcrumbItem>.DeserializeList(HttpUtility.HtmlDecode(xml));
            return new Breadcrumb(list);
        }

        public override string ToString()
        {
            return HttpUtility.HtmlEncode(ObjectSerializer<BreadcrumbItem>.SerializeList(this));
        }

        /// <summary>
        /// Joins the specified breadcrumb item.
        /// </summary>
        /// <param name="breadcrumbItem">The breadcrumb item.</param>
        /// <returns>Returns boolean.</returns>
        public bool Join(BreadcrumbItem breadcrumbItem)
        {
            int id = -1;
            foreach (var item in this)
            {
                if (item.AgentId == breadcrumbItem.AgentId)
                {
                    if (item.AgentLevel != breadcrumbItem.AgentLevel)
                    {
                        return false;
                    }
                    else
                    {
                        id = this.IndexOf(item);
                        break;
                    }
                }
            }

            if (id == -1)
            {
                Add(breadcrumbItem);
            }
            else
            {
                var length = this.Count;
                for (int i = length - 1; i > id; i--)
                {
                    RemoveAt(i);
                }
            }

            return IsValid();
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>Returns true if model is valid.</returns>
        public bool IsValid()
        {
            if (Count == 0)
            {
                return false;
            }

            var agentLevel = 0;
            foreach (var item in this)
            {
                if (item.AgentLevel <= 0)
                {
                    return false;
                }
                else if (agentLevel != 0 && item.AgentLevel - agentLevel != 1)
                {
                    return false;
                }

                agentLevel = item.AgentLevel;
            }

            return true;
        }
    }
}