// -----------------------------------------------------------------------
// <copyright file="BetType.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Enumerations
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed.")]
    public enum BetType : int
    {
        None = 0,
        MixParlay = 9,
        FulltimeHandicap = 1,
        FulltimeOddEven = 2,
        FulltimeOverUnder = 3,
        FulltimeOneXTwo = 5,
        FirstHalfHandicap = 7,
        FirstHalfOverUnder = 8,
        FirstHalfOddEven = 12,
        FirstHalfOneXTwo = 15,
        FulltimeMoneyLine = 20
    }
}