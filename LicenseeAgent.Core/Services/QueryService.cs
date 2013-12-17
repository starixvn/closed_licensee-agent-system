// --------------------------------------------------------------------------
// <copyright file="QueryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
//   <change who="Anders.Le" date="2013.4.1">Change name from ReportServiceBase to QueryService</change>
//   <change who="Anders" date="2013.4.8">Change function name from GetReportData to GetData</change>
// </history>

namespace LicenseeAgent.Core.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// This class encapsulate dynamic repository to provide
    /// a flexible way to get data from database by using Stored Procedure.
    /// </summary>
    public class QueryService : ServiceBase
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <typeparam name="T">The input object type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns a list of object T.</returns>
        public IEnumerable<T> GetEntites<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    var list = objectDb.Query<T>(param);
                    UpdateOutput(objectDb, param, criteria);

                    return list;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
                throw;
            }
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="T">The input object type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns a object T.</returns>
        public T GetEntity<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var param = criteria.GetSpParams();
                        var result = objectDb.QueryEntity<T>(param);
                        return result;
                    }
                }

                return (T)new object();
            }
            catch (Exception exception)
            {
                HandleException(exception);
                throw;
            }
        }

        /// <summary>
        /// Executes the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void Execute(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    objectDb.ExecuteNonQuery(param);
                    UpdateOutput(objectDb, param, criteria);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
                throw;
            }
        }

        /// <summary>
        /// Updates the output in the specified criteria.
        /// </summary>
        /// <param name="objectDb">The object database.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="criteria">The criteria.</param>
        private void UpdateOutput(ObjectDb objectDb, Hashtable param, ICriteria criteria)
        {
            var outputParams = objectDb.GetOuputParams();
            if (outputParams != null)
            {
                foreach (string outputParam in outputParams.Keys)
                {
                    var properties = criteria.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        var attributeName = property.Name.ToLower();
                        var paramAttributes = property.GetCustomAttributes(typeof(SpParamAttribute), true);
                        if (paramAttributes != null && paramAttributes.Length == 1)
                        {
                            var sqlParamName = ((SpParamAttribute)paramAttributes[0]).SqlParamName;
                            if (!string.IsNullOrEmpty(sqlParamName))
                            {
                                attributeName = sqlParamName.ToLower();
                            }
                        }

                        if (property.CanWrite && attributeName == outputParam)
                        {
                            var value = param[outputParam];
                            if (value == DBNull.Value)
                            {
                                var type = property.GetType();
                                value = type.IsValueType ? Activator.CreateInstance(type) : null;
                            }

                            property.SetValue(criteria, value, null);
                        }
                    }
                }
            }
        }
    }
}