// -----------------------------------------------------------------------
// <copyright file="Demo.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Tests
{
    using NUnit.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestFixture]
    public class Demo
    {
        [Test]
        public void TestPassed()
        {
            var x = 10;
            var y = 20;
            var total = x + y;
            total.ShouldEqual(30);
        }

        [Test]
        public void TestFailed()
        {
            var x = 10;
            var y = 20;
            var total = x + y;
            total.ShouldEqual(31);
        }
    }
}