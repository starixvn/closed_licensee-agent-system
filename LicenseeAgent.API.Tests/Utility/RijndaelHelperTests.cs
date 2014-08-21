namespace LicenseeAgent.API.Tests.Utility
{
    using System;
    using LicenseeAgent.API.LRF.Entities;
    using LicenseeAgent.API.Utility;
    using NUnit.Framework;

    /// <summary>
    /// RIJINDAELHelperTests class.
    /// </summary>
    [TestFixture]
    public class RijndaelHelperTests : IDisposable
    {
        private RijndaelHelper _rijndaelHelper;

        [SetUp]
        public void Setup()
        {
            _rijndaelHelper = new RijndaelHelper();
        }

        /// <summary>
        /// Encrypt and decrypt object.
        /// </summary>
        [Test]
        public void Encrypt_And_Decrypt_Object()
        {
            // Arrange
            var result = new ClientInfo
            {
                Ip = "192.168.1.1",
                BrowserName = "Firefox"
            };

            // Act
            var encryptedString = _rijndaelHelper.EncryptObject(result);
            var decryptedObject = _rijndaelHelper.DecryptObject<ClientInfo>(encryptedString);

            // Assert
            Assert.IsNotNull(decryptedObject);
            Assert.AreEqual(decryptedObject, result);
        }

        /// <summary>
        /// Encrypt and decrypt string.
        /// </summary>
        [Test]
        public void Encrypt_And_Decrypt_String()
        {
            // Arrange
            var before = "This is the simple text";

            // Act
            var encryptedText = _rijndaelHelper.EncryptString(before);
            var after = _rijndaelHelper.DecryptString(encryptedText);

            // Assert
            Assert.IsNotNull(after);
            Assert.AreEqual(before, after);
        }

        [TearDown]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_rijndaelHelper != null)
            {
                _rijndaelHelper.Dispose();
            }
        }
    }
}