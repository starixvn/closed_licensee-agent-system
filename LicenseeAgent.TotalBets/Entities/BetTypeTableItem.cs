// -----------------------------------------------------------------------
// <copyright file="BetTypeTableItem.cs" company="Nexcel Solutions Vietnam">
// BetTypeTableItem.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetTypeTableItem
    {
        public int BetType { get; set; }

        public int NumberOfColumn { get; set; }

        public BetTypeRowItem[] Rows { get; set; }
    }
}