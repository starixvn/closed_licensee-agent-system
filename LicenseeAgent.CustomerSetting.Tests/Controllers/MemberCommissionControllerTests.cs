namespace LicenseeAgent.CustomerSetting.Tests.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.CustomerSetting.Controllers;
    using NUnit.Framework;

    [TestFixture]
    public class MemberCommissionControllerTests
    {
        private MemberCommissionController _controller;

        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            _controller = CreateMemberCommissionController();
        }

        [TearDown]
        public void Dispose()
        {
            _controller.Dispose();
            _controller = null;
        }

        #endregion

        #region Tests

        [Test]
        public void IndexAction_GroupId_ReturnCommissionGroups()
        {
            // Arrange
            int numberGroup = 4;

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual(numberGroup, result.ViewData.Count);
        }

        #endregion

        /// <summary>
        /// Create Member Commission Controller.
        /// </summary>
        /// <returns> Member Commission controller.</returns>
        private MemberCommissionController CreateMemberCommissionController()
        {
            return new MemberCommissionController();
        }
    }
}
