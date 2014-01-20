// --------------------------------------------------------------------------
// <copyright file="RijndaelHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.API.Utils
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RijndaelHelper : IDisposable
    {
        private Rijndael _rijndael;
        private UTF8Encoding _encoding;

        public RijndaelHelper(byte[] key, byte[] vector)
        {
            _encoding = new UTF8Encoding();
            _rijndael = Rijndael.Create();
            _rijndael.Key = key;
            _rijndael.IV = vector;
        }

        public string EncryptObject(object obj)
        {
            var bytes = ObjectToByteArray(obj);
            var encode = Convert.ToBase64String(bytes);
            return EncryptString(encode);
        }

        public T DecryptObject<T>(string encryptedValue)
        {
            var encode = DecryptString(encryptedValue);
            var decode = Convert.FromBase64String(encode);
            return ByteArrayToObject<T>(decode);
        }

        public string EncryptString(string valueToEncrypt)
        {
            return Convert.ToBase64String(Encrypt(valueToEncrypt));
        }

        public string DecryptString(string encryptedValue)
        {
            return Decrypt(Convert.FromBase64String(encryptedValue));
        }

        /// <summary>
        /// Encrypts the specified value to encrypt.
        /// </summary>
        /// <param name="valueToEncrypt">The value to encrypt.</param>
        /// <returns>Returns encrypted byte array.</returns>
        public byte[] Encrypt(string valueToEncrypt)
        {
            var bytes = _encoding.GetBytes(valueToEncrypt);
            using (var encryptor = _rijndael.CreateEncryptor())
            using (var stream = new MemoryStream())
            using (var crypto = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
            {
                crypto.Write(bytes, 0, bytes.Length);
                crypto.FlushFinalBlock();
                stream.Position = 0;
                var encrypted = new byte[stream.Length];
                stream.Read(encrypted, 0, encrypted.Length);
                return encrypted;
            }
        }

        /// <summary>
        /// Decrypts the specified encrypted value.
        /// </summary>
        /// <param name="encryptedValue">The encrypted value.</param>
        /// <returns>Returns decrypted string.</returns>
        public string Decrypt(byte[] encryptedValue)
        {
            using (var decryptor = _rijndael.CreateDecryptor())
            using (var stream = new MemoryStream())
            using (var crypto = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
            {
                crypto.Write(encryptedValue, 0, encryptedValue.Length);
                crypto.FlushFinalBlock();
                stream.Position = 0;
                var decryptedBytes = new byte[stream.Length];
                stream.Read(decryptedBytes, 0, decryptedBytes.Length);
                return _encoding.GetString(decryptedBytes);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_rijndael != null)
            {
                _rijndael.Dispose();
            }
        }

        private byte[] ObjectToByteArray(object obj)
        {
            var binaryFormatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        private T ByteArrayToObject<T>(byte[] arrBytes)
        {
            var binaryFormatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                stream.Write(arrBytes, 0, arrBytes.Length);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}