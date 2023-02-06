namespace HM.Cryptography.Cryptographers
{
    public class XORBytesCryptographer : XORCryptographerBase, IBytesCryptographer
    {
        public ReadOnlySpan<byte> Encrypt(ReadOnlySpan<byte> bytes)
        {
            return XOREncryptCore(bytes);
        }
        public ReadOnlySpan<byte> Decrypt(ReadOnlySpan<byte> bytes)
        {
            return XOREncryptCore(bytes);
        }

        public XORBytesCryptographer(byte[] key) : base(key) { }
    }
}
