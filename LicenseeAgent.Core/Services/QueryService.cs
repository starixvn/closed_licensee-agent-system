// --------------------------------------------------------------------------
// <copyright file="QueryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Exceptions;

    /// <summary>
    /// This class encapsulate dynamic repository to provide
    /// a flexible way to get data from database by using Stored Procedure.
    /// </summary>
    public class QueryService
    {
        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="T">The input object type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns a object T.</returns>
        public static T GetEntity<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid Criteria");
                }

                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    var result = objectDb.QueryEntity<T>(param);
                    return result;
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Get entities.
        /// </summary>
        /// <typeparam name="T1">Type of data 1 wanted receipt.</typeparam>
        /// <typeparam name="T2">Type of data 2 wanted receipt.</typeparam>
        /// <typeparam name="T3">Type of data 3 wanted receipt.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns a list of object T.</returns>
        public static IDictionary<string, object> GetEntites<T1, T2, T3>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid Criteria");
                }

                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();

                    var result = new Dictionary<string, object>();

                    using (var multiTable = objectDb.QueryMultiple(param))
                    {
                        result.Add(typeof(T1).Name, multiTable.Read<T1>());
                        result.Add(typeof(T2).Name, multiTable.Read<T2>());
                        result.Add(typeof(T3).Name, multiTable.Read<T3>());
                    }

                    UpdateOutput(objectDb, criteria);

                    return result;
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Executes the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public static void Execute(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid Criteria");
                }

                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    objectDb.ExecuteNonQuery(param);
                    UpdateOutput(objectDb, criteria);
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Executes the return.
        /// </summary>
        /// <typeparam name="T">Type of return value.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns value with type T after execute.</returns>
        /// <exception cref="System.InvalidOperationException">Invalid Criteria.</exception>
        /// <exception cref="CriteriaException"></exception>
        public static T Execute<T>(ICriteria criteria) where T : IConvertible
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid Criteria");
                }

                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    return Converter.To<T>(objectDb.ExecuteScalar(param));
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <typeparam name="T">The input object type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns a list of object T.</returns>
        public IList<T> GetEntites<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid Criteria");
                }

                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    var list = objectDb.Query<T>(param);
                    UpdateOutput(objectDb, criteria);

                    return list.ToList();
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Get data table.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Returns data table.
        /// </returns>
        public DataTable ExecuteDataTable(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (!criteria.IsValid())
                {
                    throw new InvalidOperationException("Invalid Criteria");
                }

                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    var dataset = objectDb.ExecuteDataSet(param);
                    UpdateOutput(objectDb, criteria);
                    return dataset.Tables[0];
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }

        /// <summary>
        /// Updates the output in the specified criteria.
        /// </summary>
        /// <param name="objectDb">The object database.</param>
        /// <param name="criteria">The criteria.</param>
        private static void UpdateOutput(ObjectDb objectDb, ICriteria criteria)
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
                        if (paramAttributes.Length == 1)
                        {
                            var sqlParamName = ((SpParamAttribute)paramAttributes[0]).SqlParamName;
                            if (!string.IsNullOrEmpty(sqlParamName))
                            {
                                attributeName = sqlParamName.ToLower();
                            }
                        }

                        if (property.CanWrite && attributeName == outputParam)
                        {
                            var value = outputParams[outputParam];
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