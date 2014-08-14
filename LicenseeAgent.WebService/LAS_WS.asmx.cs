namespace LicenseeAgent.WebService
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Services;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Logging;
    using LicenseeAgent.WebService.Criteria;
    using LicenseeAgent.WebService.Entities;

    /// <summary>
    /// Customer information.
    /// </summary>
    [WebService]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "Web service name. Can't change because it is being used on Production.")]
    public class LAS_WS : WebService
    {
        public const int SuccessValue = 0;
        public const int FailureValue = -1;

        /// <summary>
        /// Check Username.
        /// </summary>
        /// <param name="username">Username input to check.</param>
        /// <param name="siteName">Licensee name.</param>
        /// <returns>Username is exist or not.</returns>
        [WebMethod]
        public bool CheckUsernameExist(string username, string siteName)
        {
            try
            {
                var criteria = new CheckUserNameCriteria
                {
                    UserName = username,
                    SiteName = siteName
                };

                if (!criteria.IsValid())
                {
                    return false;
                }

                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objDb.ExecuteReader(criteria.GetSpParams());

                    var errorCode = Convert.ToInt32(objDb.GetParameterValue("ErrCode"));
                    if (errorCode != SuccessValue)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Checks the agent name for cash system.
        /// </summary>
        /// <param name="siteName">Name of the site.</param>
        /// <param name="userName">Name of the agent.</param>
        /// <param name="currency">The currency.</param>
        /// <returns>
        /// Returns result of cash agent.
        /// </returns>
        [WebMethod]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Verified")]
        public CheckAgentUsernameResult CheckAgentUsername(string siteName, string userName, int currency)
        {
            try
            {
                var criteria = new CheckAgentUsernameCriteria
                {
                    SiteName = siteName,
                    UserName = userName,
                    Currency = currency
                };

                if (!criteria.IsValid())
                {
                    return new CheckAgentUsernameResult
                    {
                        ErrCode = 2,
                        ErrMsg = "Invalid criteria"
                    };
                }

                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objDb.ExecuteReader(criteria.GetSpParams());
                    var outputParams = objDb.GetOuputParams();
                    return new CheckAgentUsernameResult
                    {
                        LASAgentId = Converter.ToInt(outputParams["agentid"]),
                        ErrCode = Converter.ToInt(outputParams["errcode"]),
                        ErrMsg = string.Empty
                    };
                }
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message);
                return new CheckAgentUsernameResult
                {
                    ErrCode = 2,
                    ErrMsg = exception.Message
                };
            }
        }
    }
}