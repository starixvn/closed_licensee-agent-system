namespace LicenseeAgent.Core.Entities
{
    using System;

    /// <summary>
    /// Product Entity.
    /// </summary>
    [Serializable]
    public class ProductInfo
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
    }
}