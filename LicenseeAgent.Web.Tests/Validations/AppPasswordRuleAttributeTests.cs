namespace LicenseeAgent.Web.Tests.Validations
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Tests;
    using LicenseeAgent.Web.Validations;
    using NUnit.Framework;

    /// <summary>
    /// AppPasswordRuleAttributeTests class.
    /// </summary>
    [TestFixture]
    public class AppPasswordRuleAttributeTests
    {
        /// <summary>
        /// Test is Valid.
        /// </summary>
        [Test]
        public void IsValid_ValueInvalid_ReturnFalse()
        {
            // Arrange
            var appPassword = new AppPasswordRuleAttribute();

            // Act
            var result = appPassword.IsValid("^");

            // Assert
            TestExtensions.ShouldBeFalse(result);
        }

        /// <summary>
        /// Test is valid.
        /// </summary>
        [Test]
        public void IsValid_ValueValid_ReturnTrue()
        {
            // Arrange
            var appPassword = new AppPasswordRuleAttribute();

            // Act
            var result = appPassword.IsValid("abcdABCD1234");

            // Assert
            TestExtensions.ShouldBeTrue(result);
        }
    }
}
