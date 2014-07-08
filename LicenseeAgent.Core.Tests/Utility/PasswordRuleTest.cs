namespace LicenseeAgent.Core.Tests
{
    using System;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;
    using NUnit.Framework;

    /// <summary>
    /// Password Rule Test class.
    /// </summary>
    [TestFixture]
    public class PasswordRuleTest
    {
        private AgentInfo _alertLoginAgent2Week;
        private AgentInfo _alertLoginAgent1Week;
        ////private AgentInfo _alertLoginAgent1Day;
        private AgentInfo _alertLoginAgent2Week_False;
        private AgentInfo _alertLoginAgent1Week_False;
        private AgentInfo _alertLoginAgent1Day_False;

        /// <summary>
        /// Initialize Set up.
        /// </summary>
        [SetUp]
        public void Init()
        {
            // ARRANGE.
            var today = DateTime.Now;
            _alertLoginAgent2Week = new AgentInfo
            {
                PwdExpiry = today.AddDays(13),
                LastLoggedTime = today.AddDays(-15),
                IsFirstLogin = false
            };
            ////_alertLoginAgent1Week = new AgentInfo
            ////{
            ////    PwdExpiry = today.AddDays(6),
            ////    LastLoggedTime = today.AddDays(-8),
            ////    IsFirstLogin = false
            ////};
            ////_alertLoginAgent1Day = new AgentInfo
            ////{
            ////    PwdExpiry = today,
            ////    LastLoggedTime = today.AddDays(-2),
            ////    IsFirstLogin = false
            ////};

            _alertLoginAgent2Week_False = new AgentInfo
            {
                PwdExpiry = today.AddDays(13),
                LastLoggedTime = today,
                IsFirstLogin = false
            };
            _alertLoginAgent1Week_False = new AgentInfo
            {
                PwdExpiry = today.AddDays(6),
                LastLoggedTime = today,
                IsFirstLogin = false
            };
            _alertLoginAgent1Day_False = new AgentInfo
            {
                PwdExpiry = today,
                LastLoggedTime = today,
                IsFirstLogin = false
            };
        }

        /// <summary>
        /// Test for is first login.
        /// </summary>
        [Test]
        public void GetPasswordStatus_IsFirstLogin()
        {
            // ARRANGE.
            var firstLoginAgentInfo = new AgentInfo
            {
                IsFirstLogin = true
            };
            var firstLoginAgentInfo_False = new AgentInfo
            {
                IsFirstLogin = false
            };

            // ACT.
            var result = PasswordRule.GetPasswordStatus(firstLoginAgentInfo);
            var result_False = PasswordRule.GetPasswordStatus(firstLoginAgentInfo_False);

            // ASSERT.
            Assert.AreEqual(result, PasswordStatus.FirstLogin, "The Password status should be FirstLogin");
            Assert.AreNotEqual(result_False, PasswordStatus.FirstLogin, "The Password status should NOT be FirstLogin");
        }

        /// <summary>
        /// Test for password expire.
        /// </summary>
        [Test]
        public void GetPasswordStatus_PasswordExpire()
        {
            // ARRANGE.
            var today = DateTime.Today;
            var expiredLoginAgent = new AgentInfo
            {
                PwdExpiry = today.AddSeconds(-1),
                IsFirstLogin = false
            };
            var expiredLoginAgent_False = new AgentInfo
            {
                PwdExpiry = today,
                IsFirstLogin = false
            };

            // ACT.
            var result = PasswordRule.GetPasswordStatus(expiredLoginAgent);
            var result_False = PasswordRule.GetPasswordStatus(expiredLoginAgent_False);

            // ASSERT.
            Assert.AreEqual(result, PasswordStatus.Expired, "The Password status should be Expired");
            Assert.AreNotEqual(result_False, PasswordStatus.Expired, "The Password status should NOT be Expired");
        }

        /// <summary>
        /// Test for No last login time.
        /// </summary>
        [Test]
        public void GetPasswordStatus_PasswordNormal_NoLastLoginTime()
        {
            // ARRANGE.
            var today = DateTime.Today;
            var normalLoginAgent = new AgentInfo
            {
                PwdExpiry = today,
                IsFirstLogin = false
            };
            var normalLoginAgent_False = new AgentInfo
            {
                PwdExpiry = today.AddDays(-1),
                IsFirstLogin = false
            };

            // ACT.
            var result = PasswordRule.GetPasswordStatus(normalLoginAgent, null);
            var result_False = PasswordRule.GetPasswordStatus(normalLoginAgent_False, null);
            
            // ASSERT.
            Assert.AreEqual(result, PasswordStatus.Normal, "The Password status should be Expired");
            Assert.AreNotEqual(result_False, PasswordStatus.Normal, "The Password status should NOT be Expired");
        }

        /// <summary>
        /// Test for password alert.
        /// </summary>
        [Test]
        public void GetPasswordStatus_PasswordAlert()
        {
            // ARRANGE in Init Already.
            // ACT.
            var result2Week = PasswordRule.GetPasswordStatus(_alertLoginAgent2Week, _alertLoginAgent2Week.LastLoggedTime);
            var result1Week = PasswordRule.GetPasswordStatus(_alertLoginAgent1Week, _alertLoginAgent1Week.LastLoggedTime);
            ////var result1Day = PasswordRule.GetPasswordStatus(_alertLoginAgent1Day, _alertLoginAgent1Day.LastLoggedTime);
            var result2Week_False = PasswordRule.GetPasswordStatus(_alertLoginAgent2Week_False, _alertLoginAgent2Week_False.LastLoggedTime);
            var result1Week_False = PasswordRule.GetPasswordStatus(_alertLoginAgent1Week_False, _alertLoginAgent1Week_False.LastLoggedTime);
            var result1Day_False = PasswordRule.GetPasswordStatus(_alertLoginAgent1Day_False, _alertLoginAgent1Day_False.LastLoggedTime);
            
            // ASSERT.
            Assert.AreEqual(result2Week, PasswordStatus.Alert, "The Password status should be Alert");
            Assert.AreEqual(result1Week, PasswordStatus.Alert, "The Password status should be Alert");
            Assert.AreEqual(result1Week, PasswordStatus.Alert, "The Password status should be Alert");
            Assert.AreNotEqual(result2Week_False, PasswordStatus.Alert, "The Password status should NOT be Alert");
            Assert.AreNotEqual(result1Week_False, PasswordStatus.Alert, "The Password status should NOT be Alert");
            Assert.AreNotEqual(result1Day_False, PasswordStatus.Alert, "The Password status should NOT be Alert");
        }

        /// <summary>
        /// Test Get status message.
        /// </summary>
        [Test]
        public void GetStatusMessage()
        {
            // ARRANGE in Init Already.
            var pwdExpiredDate = DateTime.Now;
            
            // ACT.
            var resultNormal = PasswordRule.GetStatusMessage(PasswordStatus.Normal, pwdExpiredDate);
            var resultFirstLogin = PasswordRule.GetStatusMessage(PasswordStatus.FirstLogin, pwdExpiredDate);
            var resultAlert = PasswordRule.GetStatusMessage(PasswordStatus.Alert, pwdExpiredDate);
            var resultExpired = PasswordRule.GetStatusMessage(PasswordStatus.Expired, pwdExpiredDate);

            var expectedAlert = string.Format(Las.PasswordWillExpired, pwdExpiredDate.ToShortDateString());
            var expectedExpired = string.Format(Las.PasswordExpired, pwdExpiredDate.ToShortDateString());
            
            // ASSERT.
            Assert.AreEqual(resultNormal, string.Empty, "The Password status should be Normal");
            Assert.AreEqual(resultFirstLogin, Las.PasswordFirstUse, "The Password status should be First Login");
            Assert.AreEqual(resultAlert, expectedAlert, "The Password status should be Alert");
            Assert.AreEqual(resultExpired, expectedExpired, "The Password status should be Expired");
        }

        [TearDown]
        public void Dispose()
        {
            _alertLoginAgent2Week = null;
            _alertLoginAgent1Week = null;
            ////_alertLoginAgent1Day = null;
            _alertLoginAgent2Week_False = null;
            _alertLoginAgent1Week_False = null;
            _alertLoginAgent1Day_False = null;
        }
    }
}