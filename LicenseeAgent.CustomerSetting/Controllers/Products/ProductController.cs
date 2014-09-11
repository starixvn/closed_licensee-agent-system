namespace LicenseeAgent.CustomerSetting.Controllers.Products
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Services.ProductSetting;
    using LicenseeAgent.CustomerSetting.ViewModels;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// ProductSettingController class.
    /// </summary>
    public class ProductController : ProductBaseController
    {
        #region Page to display all of 3rd supported products to edit setting.

        /// <summary>
        /// Edit agent's 3rd product setting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns view to edit product settings.</returns>
        [HttpGet]
        public ViewResult EditAgent(EditProductAgentViewModel model)
        {
            var editProductItems = new List<EditProductItem>();
            var products = CoreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            var listProducts3RD = GetProducts3RD(products);
            foreach (var product in listProducts3RD)
            {
                var url = string.Empty;
                switch (product.ProductId)
                {
                    case (int)Product.Product_5:
                        url = Url.Action("EditAgent", "VirtualSports", new { area = "CustomerSetting", agentId = model.AgentId });
                        break;
                }

                if (!string.IsNullOrEmpty(url))
                {
                    editProductItems.Add(new EditProductItem
                    {
                        ProductId = product.ProductId,
                        ProductName = DataRefWrapper.GetProductName(product.ProductId),
                        Url = url
                    });
                }
            }

            var viewModel = new EditProductViewModel
            {
                ProductId = editProductItems.FirstOrDefault().ProductId,
                Products = editProductItems
            };
            return View("~/Areas/CustomerSetting/Views/Products/EditProduct.cshtml", viewModel);
        }

        /// <summary>
        /// Edit member's 3rd product setting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns view to edit product settings.</returns>
        [HttpGet]
        [LegendType(LegendType.Credit)]
        public ViewResult EditMember(EditProductMemberViewModel model)
        {
            var editProductItems = new List<EditProductItem>();
            var products = CoreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            var listProducts3RD = GetProducts3RD(products);
            foreach (var product in listProducts3RD)
            {
                var url = string.Empty;
                switch (product.ProductId)
                {
                    case (int)Product.Product_5:
                        url = Url.Action("EditMember", "VirtualSports", new { area = "CustomerSetting", memberId = model.CustId });
                        break;
                }

                if (!string.IsNullOrEmpty(url))
                {
                    editProductItems.Add(new EditProductItem
                    {
                        ProductId = product.ProductId,
                        ProductName = DataRefWrapper.GetProductName(product.ProductId),
                        Url = url
                    });
                }
            }

            var viewModel = new EditProductViewModel
            {
                ProductId = editProductItems.FirstOrDefault().ProductId,
                Products = editProductItems
            };
            return View("~/Areas/CustomerSetting/Views/Products/EditProduct.cshtml", viewModel);
        }

        #endregion Page to display all of 3rd supported products to edit setting.

        #region Get product setting

        /// <summary>
        /// Get Setting for updating customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JAVASCRIPT string object.
        /// </returns>
        [HttpGet]
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        public JavaScriptResult GetSetting(GetSettingForUpdateViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = (FormType)model.FormId,
                CustId = model.CustId,
                RoleId = model.RoleId,
                TargetCustId = model.TargetCustId,
                TargetRoleId = model.TargetRoleId,
                Ip = Request.GetClientIp()
            };

            if (!CheckPermissionForEditing(settingContext))
            {
                return RedirectToAccessDeniedJavascript();
            }

            var errCode = Result.FailureValue;
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(Formatter.FormatJson("settingContext", settingContext));
            var productService = GetProductService(model.ProductId);
            var productSetting = productService.GetSettingForUpdate(settingContext, out errCode);
            if (errCode == Result.SuccessValue)
            {
                if (model.TargetRoleId == (int)Role.Member && productSetting.CustInfo != null)
                {
                    productSetting.CustInfo.CustId = model.TargetCustId;
                    MemberService.GetMemberInformation(
                        productSetting.CustInfo,
                        new GetMemberInfoAction
                        {
                            SiteName = CurrentSession.AgentInfo.LicenseeName,
                            IBCCustId = model.TargetCustId
                        });
                }

                stringBuilder.AppendLine(Formatter.FormatJson(productService.Name, productSetting));
                if (model.ProductId == Product.Product_0)
                {
                    if (CoreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, (int)Product.Product_2))
                    {
                        var numberGameSetting = NumberGameService.GetSettingForUpdate(settingContext, out errCode);
                        if (errCode == Result.SuccessValue)
                        {
                            stringBuilder.Append(Formatter.FormatJson(NumberGameService.Name, numberGameSetting));
                        }
                    }
                }
            }

            if (errCode != Result.SuccessValue)
            {
                var result = new Result(errCode, ResourceManager.GetErrMsg(errCode));
                stringBuilder.Append(Formatter.FormatJson("result", result));
            }

            return JavaScript(stringBuilder.ToString());
        }

        /// <summary>
        /// Get Setting after updating customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        [ActionName("GetSetting")]
        public JsonResult GetSetingPost(GetSettingForUpdateViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = (FormType)model.FormId,
                CustId = model.CustId,
                RoleId = model.RoleId,
                TargetCustId = model.TargetCustId,
                TargetRoleId = model.TargetRoleId,
                Ip = Request.GetClientIp()
            };

            if (!CheckPermissionForEditing(settingContext))
            {
                return RedirectToAccessDeniedJson();
            }

            var errCode = Result.FailureValue;
            var productSetting = GetProductSetting(model.ProductId, settingContext, out errCode);
            CustomerSetting2 numberGameSetting = null;
            if (model.TargetRoleId == (int)Role.Member && productSetting.CustInfo != null)
            {
                productSetting.CustInfo.CustId = model.TargetCustId;
                MemberService.GetMemberInformation(
                    productSetting.CustInfo,
                    new GetMemberInfoAction
                    {
                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                        IBCCustId = model.TargetCustId
                    });
            }

            if (model.ProductId == Product.Product_0)
            {
                if (CoreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, (int)Product.Product_2))
                {
                    numberGameSetting = NumberGameService.GetSettingForUpdate(settingContext, out errCode);
                }
            }

            return Json(new
            {
                settingContext = settingContext,
                productSetting = productSetting,
                productSetting3RD = numberGameSetting
            });
        }

        /// <summary>
        /// Gets the product setting.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="settingContext">The setting context.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>Returns product setting.</returns>
        [NonAction]
        public CustomerSetting2 GetProductSetting(Product productId, SettingContext settingContext, out int errCode)
        {
            switch (productId)
            {
                case Product.Product_0:
                    return SportsBookService.GetSettingForUpdate(settingContext, out errCode);

                case Product.Product_5:
                    return VirtualSportsService.GetSettingForUpdate(settingContext, out errCode);
            }

            errCode = Result.FailureValue;
            return null;
        }

        /// <summary>
        /// Get Product Service.
        /// </summary>
        /// <param name="productId">ID of product.</param>
        /// <returns>Setting Service.</returns>
        [NonAction]
        public ISettingService GetProductService(Product productId)
        {
            switch (productId)
            {
                case Product.Product_0:
                    return SportsBookService;

                case Product.Product_5:
                    return VirtualSportsService;
            }

            return null;
        }
        #endregion Get product setting
    }
}