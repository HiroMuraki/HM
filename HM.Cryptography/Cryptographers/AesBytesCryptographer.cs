namespace HM.Cryptography.Cryptographers
{
    public class AesBytesCryptographer : AesCryptographerBase, IBytesCryptographer
    {
        public ReadOnlySpan<byte> Decrypt(ReadOnlySpan<byte> bytes)
        {
            return ProcessCore(bytes, _aes.CreateDecryptor());
        }
        public ReadOnlySpan<byte> Encrypt(ReadOnlySpan<byte> bytes)
        {
            return ProcessCore(bytes, _aes.CreateEncryptor());
        }

        public AesBytesCryptographer(byte[] key) : base(key) { }
        public AesBytesCryptographer(byte[] key, byte[] iv) : base(key, iv) { }
    }
}
