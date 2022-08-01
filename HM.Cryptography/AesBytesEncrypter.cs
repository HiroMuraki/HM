namespace HM.Cryptography
{
    public class AesBytesEncrypter : AesEncrypterBase, IBytesEncrypter
    {
        public ReadOnlySpan<byte> Decrypt(ReadOnlySpan<byte> bytes)
        {
            return ProcessCore(bytes, _aes.CreateDecryptor());
        }
        public ReadOnlySpan<byte> Encrypt(ReadOnlySpan<byte> bytes)
        {
            return ProcessCore(bytes, _aes.CreateEncryptor());
        }

        public AesBytesEncrypter(byte[] key) : base(key) { }
        public AesBytesEncrypter(byte[] key, byte[] iv) : base(key, iv) { }
    }
}
