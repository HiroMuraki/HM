namespace HM.Cryptography.Cryptographers
{
    public abstract class XORCryptographerBase : CryptographerBase
    {
        internal XORCryptographerBase(byte[] key)
        {
            Key = key;
        }

        protected void XOREncryptCore(byte[] bytes)
        {
            XOREncryptCore(bytes.AsSpan());
        }
        protected void XOREncryptCore(Span<byte> bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ _originKey[i % _originKey.Length]);
            }
        }
        protected ReadOnlySpan<byte> XOREncryptCore(ReadOnlySpan<byte> bytes)
        {
            var result = new Span<byte>(new byte[bytes.Length]);
            bytes.CopyTo(result);
            for (int i = 0; i < bytes.Length; i++)
            {
                result[i] = (byte)(bytes[i] ^ _originKey[i % _originKey.Length]);
            }
            return result;
        }
    }
}
