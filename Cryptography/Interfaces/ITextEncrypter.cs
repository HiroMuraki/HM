namespace HM.Cryptography
{
    public interface ITextEncrypter
    {
        string Encrypt(string originText);
        string Decrypt(string encryptedText);
    }
}
