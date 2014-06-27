namespace LicenseeAgent.WebService
{
    using System;
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
    [System.ComponentModel.ToolboxItem(false)]
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
        /// <param name="userName">Name of the agent.</param>
        /// <param name="siteName">Name of the site.</param>
        /// <returns>Returns result of cash agent.</returns>
        [WebMethod]
        public CheckAgentUsernameResult CheckAgentUsername(string userName, string siteName)
        {
            try
            {
                var criteria = new CheckAgentUsernameCriteria
                {
                    AgentUsername = userName,
                    SiteName = siteName
                };

                if (!criteria.IsValid())
                {
                    return new CheckAgentUsernameResult
                    {
                        ErrCode = FailureValue,
                        ErrMsg = "Invalid criteria"
                    };
                }

                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objDb.ExecuteReader(criteria.GetSpParams());
                    var outputParams = objDb.GetOuputParams();
                    return new CheckAgentUsernameResult
                    {
                        AgentId = Converter.ToInt(outputParams["agentid"]),
                        AgentCurrency = Converter.ToString2(outputParams["agentcurrency"]),
                        ErrCode = Converter.ToInt(outputParams["errcode"]),
                        ErrMsg = Converter.ToString2(outputParams["errmsg"])
                    };
                }
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message);
                throw;
            }
        }
    }
}