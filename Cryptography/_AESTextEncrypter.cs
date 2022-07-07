using System.Security.Cryptography;
using System.Text;

namespace HM.Cryptography
{
    [Obsolete("Use AesTextEncrypter")]
    public class _AESTextEncrypter : ITextEncrypter
    {
        private const int _bitPerByte = 8; // 每字节位数
        private const int _bytePerKByte = 1024; // 每KB的字节数
        private const int _mBytePerKByte = 1024; // 每MB的KB数
        private const int _gBytePerMByte = 1024; // 每GB的MB数
        private const int _aesKeyLength = 128 / _bitPerByte; // AES密钥字节长度，默认为128 / BitPerByte，即16
        private const int _aesBlockSize = _aesKeyLength * _bitPerByte; //AES加密块大小，设置为密钥长度 * BitPerByte
        private readonly Aes _aes = Aes.Create();
        private readonly int _encryptBufferSize = 32 * _bytePerKByte; // 加密缓冲区大小，设置为32KB
        private readonly int _decryptBufferSize = 32 * _bytePerKByte + 16; // 解密缓冲区大小，为加密缓冲区大小加上AES加密会导致多出的16字节

        #region 公共属性
        public int EncryptionBufferSize
        {
            get
            {
                return _encryptBufferSize;
            }
        }
        public int DecryptionBufferSize
        {
            get
            {
                return _decryptBufferSize;
            }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 加密字节流
        /// </summary>
        /// <param name="buffer">要加密的字节流</param>
        /// <returns>加密后的字节流</returns>
        public byte[] Decrypt(byte[] buffer)
        {
            return ProcessCore(buffer, _aes.CreateDecryptor());
        }
        /// <summary>
        /// 解密字节流
        /// </summary>
        /// <param name="buffer">要加密的字节流</param>
        /// <returns>解密后的字节流</returns>
        public byte[] Encrypt(byte[] buffer)
        {
            return ProcessCore(buffer, _aes.CreateEncryptor());
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="source">字符串原文</param>
        /// <returns>加密结果</returns>
        public string Encrypt(string source)
        {
            return Encrypt(Convert.ToHexString(Encoding.UTF8.GetBytes(source)));
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="encrypted">字符串密文</param>
        /// <returns>解密结果</returns>
        public string Decrypt(string encrypted)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromHexString(encrypted)));
        }
        #endregion

        public _AESTextEncrypter(string key)
        {
            _aes.BlockSize = _aesBlockSize;
            _aes.Key = GetKey(key);
            _aes.IV = new byte[_aesKeyLength];
            for (int i = 0; i < _aesKeyLength; i++)
            {
                _aes.IV[i] = _aes.Key[i];
            }
        }

        /// <summary>
        /// 从字符串获取加密密钥
        /// </summary>
        /// <param name="key">字符串</param>
        /// <returns>长度为16（128位）的加密密钥</returns>
        private static byte[] GetKey(string key)
        {
            byte[] keys = new byte[_aesKeyLength];
            byte[] sourceKeys = Encoding.UTF8.GetBytes(key);
            //从字符串中获取密钥，若字符串长度大于16，则取前16位
            //若不足16位，则用0填充末尾
            for (int i = 0; i < _aesKeyLength; i++)
            {
                if (i < sourceKeys.Length)
                {
                    keys[i] = sourceKeys[i];
                }
                else
                {
                    keys[i] = 0;
                }
            }
            return keys;
        }
        /// <summary>
        /// 处理加/解密主要方法
        /// </summary>
        /// <param name="buffer">要加/解密的流</param>
        /// <param name="transform">转换阵</param>
        /// <returns>处理后的流</returns>
        private static byte[] ProcessCore(byte[] buffer, ICryptoTransform transform)
        {
            byte[] output;
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                {
                    using (var writer = new BufferedStream(cryptoStream))
                    {
                        writer.Write(buffer);
                    }
                    output = memoryStream.ToArray();
                }
            }
            return output;
        }
    }
}
