namespace LicenseeAgent.Core.Tests
{
    using LicenseeAgent.Core.Utility;
    using NUnit.Framework;

    /// <summary>
    /// Extension Test class.
    /// </summary>
    [TestFixture]
    public class ExtensionsTest
    {
        [Test]
        public void ContainsString_ReturnTrue()
        {
            Assert.IsTrue("ABCD".Contains(string.Empty, System.StringComparison.CurrentCultureIgnoreCase), "@Text ABCD Should return true");
            Assert.IsTrue("ABCD".Contains("a", System.StringComparison.CurrentCultureIgnoreCase), "@Text ABCD Should return true");
            Assert.IsTrue("ABCD".Contains("abcd", System.StringComparison.CurrentCultureIgnoreCase), "@Text ABCD Should return true");
            Assert.IsFalse("ABCD".Contains("e", System.StringComparison.CurrentCultureIgnoreCase), "@Text ABCD Should return false");
            Assert.IsFalse("ABCD".Contains("a", System.StringComparison.CurrentCulture), "@Text ABCD Should return false");
        }

        [Test]
        public void ContainsString_ReturnFalse()
        {
            // ARRANGE.
            // ACT.
            // ASSERT.
            Assert.IsFalse("ABCD".Contains("e", System.StringComparison.CurrentCultureIgnoreCase), "@Text ABCD Should return false");
            Assert.IsFalse("ABCD".Contains("a", System.StringComparison.CurrentCulture), "@Text ABCD Should return false");
        }

        /// <summary>
        /// Test with first name, last name.
        /// </summary>
        [Test]
        public void FormatCustomerName_HasLastNameFirstName_ReturnTrue()
        {
            // ARRANGE.
            string firstName = "Vinh";
            string lastName = "Trinh";

            // ACT.
            string expectedResult = "Vinh Trinh";
            var actualResult = Extensions.FormatCustomerName(firstName, lastName);
            
            // ASSERT.
            Assert.AreEqual(expectedResult, actualResult, "Customer full name should be: Vinh Trinh");
        }

        /// <summary>
        /// Test with first name empty.
        /// </summary>
        [Test]
        public void FormatCustomerName_HasLastName_ReturnTrue()
        {
            // ARRANGE.
            string firstName = string.Empty;
            string lastName = "Trinh";
            
            // ACT.
            string expectedResult = "Trinh";
            var actualResult = Extensions.FormatCustomerName(firstName, lastName);
            
            // ASSERT.
            Assert.AreEqual(expectedResult, actualResult, "Customer full name should be: Trinh");
        }

        /// <summary>
        /// Test with last name empty.
        /// </summary>
        [Test]
        public void FormatCustomerName_HasFirstName_ReturnTrue()
        {
            // ARRANGE.
            string firstName = "Vinh";
            string lastName = string.Empty;
            
            // ACT.
            string expectedResult = "Vinh";
            var actualResult = Extensions.FormatCustomerName(firstName, lastName);
            
            // ASSERT.
            Assert.AreEqual(expectedResult, actualResult, "Customer full name should be: Vinh");
        }

        /// <summary>
        /// Test with first name and last name empty.
        /// </summary>
        [Test]
        public void FormatCustomerName_Empty_ReturnTrue()
        {
            // ARRANGE.
            string firstName = string.Empty;
            string lastName = string.Empty;
            
            // ACT.
            string expectedResult = "---";
            var actualResult = Extensions.FormatCustomerName(firstName, lastName);
            
            // ASSERT.
            Assert.AreEqual(expectedResult, actualResult, "Customer full name should be: ---");
        }
    }
}