namespace LicenseeAgentWebService
{
    using System;
    using System.Web.Services;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Customer information.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
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
                var criteria = new CutomerCriteria
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
    }
}