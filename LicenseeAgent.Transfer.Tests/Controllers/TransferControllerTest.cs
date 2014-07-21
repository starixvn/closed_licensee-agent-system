namespace LicenseeAgent.Transfer.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Tests.Fakes;
    using LicenseeAgent.Transfer.Controllers;
    using LicenseeAgent.Transfer.Criteria;
    using LicenseeAgent.Transfer.Entities;
    using LicenseeAgent.Transfer.Services;
    using LicenseeAgent.Transfer.Tests.Fakes;
    using LicenseeAgent.Transfer.ViewModels;
    using NUnit.Framework;

    /// <summary>
    /// TransferController class.
    /// </summary>
    public class TransferControllerTest
    {
        private TransferController _controller;

        [SetUp]
        public void Init()
        {
            FakeSession.InitHttpContext();
            _controller = CreateTransferController();
        }

        /// <summary>
        /// Test when load page.
        /// </summary>
        [Test]
        public void IndexAction_Get_ReturnView()
        {
            // Arrange
            var viewName = string.Empty;

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual(viewName, result.ViewName);
        }

        /// <summary>
        /// Test when load page.
        /// </summary>
        [Test]
        public void Search_IsAgent_ReturnAgent()
        {
            // Arrange
            var viewModel = new TransferSearchViewModel();
            FakeSession.SetAgentLevel();

            // Act
            var result = _controller.Search(viewModel) as JsonResult;

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Test when load page.
        /// </summary>
        [Test]
        public void Search_IsLastAgent_ReturnMember()
        {
            // Arrange
            var viewModel = new TransferSearchViewModel();
            FakeSession.SetLastAgentLevel();

            // Act
            var result = _controller.Search(viewModel) as JsonResult;

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        /// <summary>
        /// Test when load page.
        /// </summary>
        [Test]
        public void SearchMember_ReturnFail()
        {
            // Arrange
            ////var viewModel = new TransferSearchViewModel();
            var transferService = FakeServices.TransferService;
            var memberService = FakeServices.MemberService;
            var list = transferService.GetMemberBalanceInfo(new GetMemberBalanceInfoCriteria());

            // Act
            var response = memberService.GetMemberInformations<MemberBalanceInfo>(
               list,
               new GetMemberInfoListAction
               {
                   LASAgentId = 1,
                   SiteName = "yq355"
               });
            ////var result = _controller.SearchMember(viewModel) as JsonResult;

            // Assert
            Assert.AreNotEqual(Result.SuccessValue, response.ErrCode);
        }

        /// <summary>
        /// Create Transfer Controller.
        /// </summary>
        /// <returns> Transfer controller.</returns>
        private TransferController CreateTransferController()
        {
            var memberService = FakeServices.MemberService;
            var downlineService = FakeServices.DownLinePermissionService;
            var transferService = FakeServices.TransferService;

            return new TransferController(transferService, memberService, downlineService);
        }
    }
}