namespace LicenseeAgentWebService
{
    using System;
    using System.Web.Services;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Logging;
    using LicenseeAgentWebService.Criteria;

    /// <summary>
    /// Customer information.
    /// </summary>
    [WebService]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class LAS_WS : WebService
    {
        /// <summary>
        /// Check Username.
        /// </summary>
        /// <param name="username">Username input to check.</param>
        /// <param name="licenseeName">Licensee name.</param>
        /// <returns>Username is exist or not.</returns>
        [WebMethod]
        public bool CheckUsernameExist(string username, string licenseeName)
        {
            try
            {
                var criteria = new CheckUserNameCriteria
                {
                    UserName = username,
                    SiteName = licenseeName
                };

                if (!criteria.IsValid())
                {
                    return false;
                }

                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objDb.ExecuteReader(criteria.GetSpParams());

                    var errorCode = Convert.ToInt32(objDb.GetParameterValue("ErrCode"));
                    if (errorCode != 0)
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
        /// Checks the agent username.
        /// </summary>
        /// <param name="agentUsername">The agent username.</param>
        /// <param name="licenseeName">Name of the licensee.</param>
        /// <param name="agentId">The agent identifier.</param>
        /// <param name="agentCurrency">The agent currency.</param>
        /// <param name="errCode">The error code.</param>
        /// <param name="errMsg">The error MSG.</param>
        /// <returns>Agent username is existing or not existing.</returns>
        [WebMethod]
        public bool CheckAgentUsername(
            string agentUsername,
            string licenseeName,
            out int agentId,
            out string agentCurrency,
            out int errCode,
            out string errMsg)
        {
            try
            {
                agentId = 0;
                agentCurrency = string.Empty;
                errCode = 0;
                errMsg = string.Empty;
                var criteria = new CheckAgentUsernameCriteria
                {
                    AgentUsername = agentUsername,
                    SiteName = licenseeName
                };

                if (!criteria.IsValid())
                {
                    return false;
                }

                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objDb.ExecuteReader(criteria.GetSpParams());

                    var outputParams = objDb.GetOuputParams();
                    agentId = Converter.ToInt(outputParams["agentid"]);
                    agentCurrency = Converter.ToString2(outputParams["agentcurrency"]);
                    errCode = Converter.ToInt(outputParams["errcode"]);
                    errMsg = Converter.ToString2(outputParams["errmsg"]);

                    if (errCode == 0)
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
    }
}