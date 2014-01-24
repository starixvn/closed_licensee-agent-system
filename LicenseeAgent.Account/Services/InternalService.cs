// --------------------------------------------------------------------------
// <copyright file="InternalService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Account.Services
{
    using System.Web;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InternalService : IInternalService
    {
        private const int Timeout = 1 * 60; // 1 minute

        private static byte[] aesKey =
        {
            123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37,
            112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209
        };

        private static byte[] aesVector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 221, 112, 79, 32, 114, 156 };

        public string Encode(InternalLoginInfo internalLoginInfo)
        {
            using (var rijndaelHelper = new RijndaelHelper(aesKey, aesVector))
            {
                var authKey = HttpUtility.UrlEncode(rijndaelHelper.EncryptObject(internalLoginInfo));
                return authKey;
            }
        }

        /// <summary>
        /// Decodes the specified authentication key.
        /// </summary>
        /// <param name="authKey">The authentication key.</param>
        /// <returns>Returns internal login information.</returns>
        public InternalLoginInfo Decode(string authKey)
        {
            using (var rijndaelHelper = new RijndaelHelper(aesKey, aesVector))
            {
                return rijndaelHelper.DecryptObject<InternalLoginInfo>(authKey);
            }
        }
    }
}