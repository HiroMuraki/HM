namespace HM.Cryptography.Cryptographers
{
    public interface IBytesCryptographer
    {
        ReadOnlySpan<byte> Encrypt(ReadOnlySpan<byte> bytes);
        ReadOnlySpan<byte> Decrypt(ReadOnlySpan<byte> bytes);
    }
}
