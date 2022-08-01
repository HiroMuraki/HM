namespace HM.Cryptography
{
    public class XORBytesEncryptor : XOREncrypterBase, IBytesEncrypter
    {
        public ReadOnlySpan<byte> Encrypt(ReadOnlySpan<byte> bytes)
        {
            return XOREncryptCore(bytes);
        }
        public ReadOnlySpan<byte> Decrypt(ReadOnlySpan<byte> bytes)
        {
            return XOREncryptCore(bytes);
        }

        public XORBytesEncryptor(byte[] key) : base(key) { }
    }
}
