namespace HM.Cryptography
{
    public interface IBytesEncrypter
    {
        ReadOnlySpan<byte> Encrypt(ReadOnlySpan<byte> bytes);
        ReadOnlySpan<byte> Decrypt(ReadOnlySpan<byte> bytes);
    }
}
