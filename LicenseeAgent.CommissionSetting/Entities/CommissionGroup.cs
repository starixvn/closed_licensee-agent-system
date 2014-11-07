namespace LicenseeAgent.CommissionSetting.Entities
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// CommissionGroup class.
    /// </summary>
    public class CommissionGroup
    {
        public CommissionGroup()
        {
        }

        public CommissionGroup(int groupCommId, string groupName)
        {
            GroupCommId = groupCommId;
            GroupName = groupName;
        }

        public int GroupCommId { get; set; }

        public string GroupName { get; set; }

        public decimal SBComm { get; set; }

        public decimal NGComm { get; set; }

        public decimal RacingComm { get; set; }

        public decimal LCComm { get; set; }

        public decimal RNGComm { get; set; }

        public int NumberOfCustomer { get; set; }

        public bool IsDefault { get; set; }

        /// <summary>
        /// Generates the product commissions.
        /// </summary>
        /// <param name="productColumns">The product columns.</param>
        /// <returns>Returns list of product commissions.</returns>
        public IList<ProductCommission> GenerateProductCommissions(IList<ProductCommissionColumn> productColumns)
        {
            var productCommissions = new List<ProductCommission>();
            foreach (var column in productColumns)
            {
                decimal? commission = null;
                switch (column.ProductId)
                {
                    case (int)ProductCode.Product_0:
                        commission = SBComm;
                        break;

                    case (int)ProductCode.Product_1:
                        commission = RacingComm;
                        break;

                    case (int)ProductCode.Product_2:
                        commission = NGComm;
                        break;

                    case (int)ProductCode.Product_4:
                        commission = LCComm;
                        break;

                    case (int)ProductCode.Product_6:
                        commission = RNGComm;
                        break;
                }

                if (commission != null)
                {
                    productCommissions.Add(new ProductCommission
                    {
                        ProductId = column.ProductId,
                        Commission = commission.Value
                    });
                }
            }

            return productCommissions;
        }
    }
}