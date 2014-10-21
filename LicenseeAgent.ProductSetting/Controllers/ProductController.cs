namespace LicenseeAgent.ProductSetting.Controllers
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
    using LicenseeAgent.ProductSetting.Entities;
    using LicenseeAgent.ProductSetting.Services.ProductSetting;
    using LicenseeAgent.ProductSetting.ViewModels;
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
                    case (int)ProductCode.Product_5:
                        url = Url.Action("EditAgent", "VirtualSports", new { agentId = model.AgentId });
                        break;
                    case (int)ProductCode.Product_8:
                        url = Url.Action("EditAgent", "Keno", new { agentId = model.AgentId });
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
            return View("EditProduct", viewModel);
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
                    case (int)ProductCode.Product_5:
                        url = Url.Action("EditMember", "VirtualSports", new { memberId = model.CustId });
                        break;
                    case (int)ProductCode.Product_8:
                        url = Url.Action("EditMember", "Keno", new { memberId = model.CustId });
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
            return View("EditProduct", viewModel);
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
                ProductId = (int)model.ProductId,
                Ip = Request.GetClientIp()
            };

            if (!CheckPermissionForEditing(settingContext))
            {
                return RedirectToAccessDeniedJavascript();
            }

            var result = new Result { ErrCode = Result.FailureValue };
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(Formatter.FormatJson("settingContext", settingContext));
            var productService = GetProductService(model.ProductId);
            var productSetting = productService.GetSettingForUpdate(settingContext, result);
            if (result.ErrCode == Result.SuccessValue)
            {
                if (model.TargetRoleId == (int)Role.Member && productSetting.CustInfo != null)
                {
                    productSetting.CustInfo.CustId = model.TargetCustId;
                    GetMemberInfo(productSetting.CustInfo);
                }

                stringBuilder.AppendLine(Formatter.FormatJson(productService.Name, productSetting));
                if (model.ProductId == ProductCode.Product_0)
                {
                    if (CoreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, (int)ProductCode.Product_2))
                    {
                        var numberGameSetting = NumberGameService.GetSettingForUpdate(settingContext, result);
                        if (result.ErrCode == Result.SuccessValue)
                        {
                            stringBuilder.Append(Formatter.FormatJson(NumberGameService.Name, numberGameSetting));
                        }
                    }
                }
            }

            if (result.ErrCode != Result.SuccessValue)
            {
                stringBuilder.Append(Formatter.FormatJson("result", new Result(result.ErrCode, ResourceManager.GetErrMsg(result.ErrCode))));
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
                ProductId = (int)model.ProductId,
                Ip = Request.GetClientIp()
            };

            if (!CheckPermissionForEditing(settingContext))
            {
                return RedirectToAccessDeniedJson();
            }

            var result = new Result { ErrCode = Result.FailureValue };
            var productService = GetProductService(model.ProductId);
            var productSetting = productService.GetSettingForUpdate(settingContext, result);
            CustomerSetting2 numberGameSetting = null;
            if (model.TargetRoleId == (int)Role.Member && productSetting.CustInfo != null)
            {
                productSetting.CustInfo.CustId = model.TargetCustId;
                GetMemberInfo(productSetting.CustInfo);
            }

            if (model.ProductId == ProductCode.Product_0)
            {
                if (CoreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, (int)ProductCode.Product_2))
                {
                    numberGameSetting = NumberGameService.GetSettingForUpdate(settingContext, result);
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
        /// Get Product Service.
        /// </summary>
        /// <param name="productId">ID of product.</param>
        /// <returns>Setting Service.</returns>
        [NonAction]
        public ISettingService GetProductService(ProductCode productId)
        {
            switch (productId)
            {
                case ProductCode.Product_0:
                    return SportsBookService;

                case ProductCode.Product_5:
                    return VirtualSportsService;

                case ProductCode.Product_8:
                    return KenoService;
            }

            return null;
        }

        #endregion Get product setting
    }
}