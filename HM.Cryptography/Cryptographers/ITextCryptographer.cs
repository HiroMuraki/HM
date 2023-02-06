namespace HM.Cryptography.Cryptographers
{
    public interface ITextCryptographer
    {
        string Encrypt(string originText);
        string Decrypt(string encryptedText);
    }
}
