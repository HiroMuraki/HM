using System.Security.Cryptography;

namespace HM.Cryptography
{
    public abstract class AesEncrypterBase : EncrypterBase
    {
        public static readonly int KeyLengh = 256 / 8;
        public static readonly int IVLength = 128 / 8;

        internal AesEncrypterBase(byte[] key)
        {
            if (key.Length != KeyLengh)
            {
                throw new ArgumentException($"Requires a {KeyLengh} length key");
            }
            _aes.Key = key;
            _aes.IV = new byte[IVLength];
        }
        internal AesEncrypterBase(byte[] key, byte[] iv) : this(key)
        {
            _aes.IV = iv;
        }

        protected static byte[] ProcessCore(ReadOnlySpan<byte> buffer, ICryptoTransform transform)
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
        protected static byte[] ProcessCore(byte[] buffer, ICryptoTransform transform)
        {
            return ProcessCore(buffer.AsSpan(), transform);
        }
        protected readonly Aes _aes = Aes.Create();
    }
}
