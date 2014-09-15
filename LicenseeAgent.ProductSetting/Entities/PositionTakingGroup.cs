namespace LicenseeAgent.ProductSetting.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Group of position taking, representative for each sport.
    /// </summary>
    public class PositionTakingGroup
    {
        public PositionTakingGroup()
        {
            PositionTakings = new List<PositionTakingItem>();
        }

        public int SportId { get; set; }

        public bool IsLive { get; set; }

        public IList<PositionTakingItem> PositionTakings { get; set; }
    }
}