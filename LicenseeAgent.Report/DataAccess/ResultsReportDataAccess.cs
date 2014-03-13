// ----------------------------------------------------------------------
// <copyright file="ResultsReportDataAccess.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/01/21">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.DataAccess
{
    using System;
    using System.Data;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using LicenseeAgent.Core.Exceptions;

    /// <summary>
    /// Results Report Data Access.
    /// </summary>
    public class ResultsReportDataAccess : IResultsReportDataAccess
    {
        /// <summary>
        /// Get outright results.
        /// </summary>
        /// <param name="criteria">Bet List Details Criteria.</param>
        /// <returns>List of outright results.</returns>
        public DataTable GetResults(ICriteria criteria)
        {
            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    var dataset = objectDb.ExecuteDataSet(param);

                    return dataset.Tables[0];
                }
            }
            catch (Exception exception)
            {
                throw new CriteriaException(criteria, exception.Message, exception.InnerException);
            }
        }
    }
}