namespace LicenseeAgent.CommissionSetting.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using LicenseeAgent.CommissionSetting.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;

    /// <summary>
    /// CommissionSettingController class.
    /// </summary>
    [AllowedCommission]
    [AccountType(AccountType.Admin)]
    [LegendType(LegendType.Deposit)]
    public abstract class CommissionSettingController : BaseController
    {
        #region Variables

        private static IList<ProductCommissionColumn> productCommissionColumns = SupportedColumns;
        private readonly ICoreService _coreService;

        #endregion Variables

        #region Constructors

        protected CommissionSettingController()
            : this(null)
        {
        }

        protected CommissionSettingController(ICoreService coreService)
        {
            _coreService = coreService ?? new CoreService();
        }

        #endregion Constructors

        #region Properties

        protected static IList<ProductCommissionColumn> SupportedColumns
        {
            get
            {
                return new List<ProductCommissionColumn>
                {
                    new ProductCommissionColumn(ProductCode.Product_0, "SBCommission", "SBComm"),
                    new ProductCommissionColumn(ProductCode.Product_1, "RacingCommission", "RacingComm"),
                    new ProductCommissionColumn(ProductCode.Product_2, "NGCommission", "NGComm"),
                    new ProductCommissionColumn(ProductCode.Product_4, "LCCommission", "LCComm"),
                    new ProductCommissionColumn(ProductCode.Product_6, "RNGCommission", "RNGComm")
                };
            }
        }

        protected IList<ProductCommissionColumn> OfferedColumns
        {
            get
            {
                var offeredProducts = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
                var offeredProductIds = offeredProducts.Select(x => x.ProductId);
                return productCommissionColumns.Where(x => offeredProductIds.Contains(x.ProductId)).ToList();
            }
        }

        #endregion Properties
    }
}