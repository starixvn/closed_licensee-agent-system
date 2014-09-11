namespace LicenseeAgent.Web.Tests.Validations
{
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Account.ViewModels.Account;
    using LicenseeAgent.Web.Validations;
    using NSubstitute;
    using NUnit.Framework;

    /// <summary>
    /// NotEqualAttributeTests class.
    /// </summary>
    [TestFixture]
    public class NotEqualAttributeTests
    {
        /// <summary>
        /// Get client validation rules.
        /// </summary>
        [Test]
        public void GetClientValidationRules_Should_ReturnRule()
        {
            // Arrange
            var model = new AgentChangePasswordViewModel();
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, typeof(AgentChangePasswordViewModel));
            var controllerContext = Substitute.For<ControllerContext>();
            var otherProperty = "NewPassword";
            var notEqual = new NotEqualAttribute(otherProperty);

            // Act
            var modelClientValidationRule = notEqual.GetClientValidationRules(metadata, controllerContext);

            // Assert
            Assert.IsTrue(modelClientValidationRule.Count<ModelClientValidationRule>() > 0);
        }
    }
}