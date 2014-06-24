namespace LicenseeAgent.Tests
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Demo class.
    /// </summary>
    [TestFixture]
    public class Demo : IDisposable
    {
        [SetUp]
        public void Setup()
        {
        }

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

        [TearDown]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}