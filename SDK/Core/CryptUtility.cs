using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Portal.SDK.Core
{
    public static class CryptUtility
    {
        #region 01.字段
        private static readonly byte[] defaultIV = new byte[] { 0x9f, 0xf8, 0x35, 0x66, 0x48, 70, 0xd3, 0xce, 0xa3, 5, 0xc4, 0x71, 0x56, 8, 40, 0xda };

        private static readonly byte[] defaultKEY = new byte[]
            {
                0x7f, 0xf6, 0xa1, 0xa3, 110, 0x7e, 0x1c, 0x88, 0x21, 0xf4, 0xda, 0xe9, 120, 0x75, 0x6a, 100,
                0x68, 0xc4, 0x7f, 0x8e, 0xdd, 0xa2, 0x92, 0x5c, 2, 90, 0x9c, 180, 0x9e, 0xd7, 0x43, 0x88
            };

        private static readonly int SALT_BYTES_LENGTH = 0x10;
        #endregion

        #region 02.加密方法
        /// <summary>
        /// 使用：CryptUtility.Encrypt("CLZ@DB@MEDUSA","admin");
        /// </summary>
        /// <param name="key">Key密钥</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string key, string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            byte[][] bufferArray = _GenerateKey(key);
            return Convert.ToBase64String(_Encrypt(bufferArray[0], bufferArray[1], bytes));
        }

        public static byte[] Encrypt(string key, byte[] data)
        {
            byte[][] bufferArray = _GenerateKey(key);
            return _Encrypt(bufferArray[0], bufferArray[1], data);
        }
        #endregion

        #region 03.解密方法
        /// <summary>
        /// 使用：CryptUtility.Decrypt("CLZ@DB@MEDUSA","admin");
        /// </summary>
        /// <param name="key">Key密钥</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(string key, string text)
        {
            byte[] data = Convert.FromBase64String(text);
            byte[][] bufferArray = _GenerateKey(key);
            return Encoding.Unicode.GetString(_Decrypt(bufferArray[0], bufferArray[1], data));
        }

        public static byte[] Decrypt(string key, byte[] data)
        {
            byte[][] bufferArray = _GenerateKey(key);
            return _Decrypt(bufferArray[0], bufferArray[1], data);
        }

        #endregion

        #region 04.私有方法
        private static byte[][] _GenerateKey(string keyString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(keyString);
            byte[] buffer2 = defaultIV.Clone() as byte[];
            byte[] buffer3 = defaultKEY.Clone() as byte[];
            for (int i = 0; i < buffer2.Length; i++)
            {
                if (i >= bytes.Length)
                {
                    break;
                }
                buffer2[i] = (byte)(bytes[i] ^ i);
            }
            for (int j = 0; j < buffer3.Length; j++)
            {
                if (j >= bytes.Length)
                {
                    break;
                }
                buffer3[j] = bytes[j];
            }
            return new byte[][] { buffer2, buffer3 };
        }

        private static ICryptoTransform _CreateTransform(byte[] iv, byte[] key, bool encrypt)
        {
            using (SymmetricAlgorithm algorithm2 = SymmetricAlgorithm.Create("DES"))
            {
                if (iv.Length != 0x10)
                {
                    Array.Resize<byte>(ref iv, algorithm2.IV.Length);
                }
                if (key.Length != algorithm2.KeySize)
                {
                    Array.Resize<byte>(ref key, algorithm2.Key.Length);
                }
                return (encrypt ? algorithm2.CreateEncryptor(key, iv) : algorithm2.CreateDecryptor(key, iv));
            }
        }

        private static byte[] _Encrypt(byte[] iv, byte[] key, byte[] data)
        {
            byte[] buffer;
            MemoryStream stream = new MemoryStream();
            ICryptoTransform transform = _CreateTransform(iv, key, true);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            try
            {
                stream2.Write(data, 0, data.Length);
                stream2.FlushFinalBlock();
                buffer = stream.ToArray();
            }
            finally
            {
                stream2.Close();
                stream.Close();
                transform.Dispose();
            }
            return buffer;
        }

        private static byte[] _EncryptSalt(byte[] iv, byte[] key, byte[] data)
        {
            byte[] buffer = new byte[SALT_BYTES_LENGTH];
            new RNGCryptoServiceProvider().GetBytes(buffer);
            byte[] dst = new byte[buffer.Length + data.Length];
            Buffer.BlockCopy(buffer, 0, dst, 0, buffer.Length);
            Buffer.BlockCopy(data, 0, dst, buffer.Length, data.Length);
            return _Encrypt(iv, key, dst);
        }

        private static byte[] _Decrypt(byte[] iv, byte[] key, byte[] data)
        {
            byte[] buffer;
            MemoryStream stream = new MemoryStream();
            ICryptoTransform transform = _CreateTransform(iv, key, false);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            try
            {
                stream2.Write(data, 0, data.Length);
                stream2.FlushFinalBlock();
                buffer = stream.ToArray();
            }
            finally
            {
                stream2.Close();
                stream.Close();
                transform.Dispose();
            }
            return buffer;
        }

        private static byte[] _DecryptSalt(byte[] iv, byte[] key, byte[] data)
        {
            byte[] src = _Decrypt(iv, key, data);
            byte[] dst = new byte[src.Length - SALT_BYTES_LENGTH];
            Buffer.BlockCopy(src, SALT_BYTES_LENGTH, dst, 0, dst.Length);
            return dst;
        }

        #endregion

        #region 05.其他方法
        public static string EncryptSalt(string key, string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            byte[][] bufferArray = _GenerateKey(key);
            return Convert.ToBase64String(_EncryptSalt(bufferArray[0], bufferArray[1], bytes));
        }

        public static byte[] EncryptSalt(string key, byte[] data)
        {
            byte[][] bufferArray = _GenerateKey(key);
            return _EncryptSalt(bufferArray[0], bufferArray[1], data);
        }

        public static string DecryptSalt(string key, string text)
        {
            byte[] data = Convert.FromBase64String(text);
            byte[][] bufferArray = _GenerateKey(key);
            return Encoding.Unicode.GetString(_DecryptSalt(bufferArray[0], bufferArray[1], data));
        }

        public static byte[] DecryptSalt(string key, byte[] data)
        {
            byte[][] bufferArray = _GenerateKey(key);
            return _DecryptSalt(bufferArray[0], bufferArray[1], data);
        }

        #endregion

    }
}
