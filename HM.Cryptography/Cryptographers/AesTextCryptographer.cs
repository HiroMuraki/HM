using System.Text;

namespace HM.Cryptography.Cryptographers
{
    public class AesTextCryptographer : AesCryptographerBase, ITextCryptographer
    {
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public string Decrypt(string encryptedText)
        {
            var decryptedBytes = ProcessCore(Convert.FromHexString(encryptedText), _aes.CreateDecryptor());
            return Encoding.GetString(decryptedBytes);
        }
        public string Encrypt(string originText)
        {
            var encryptedBytes = ProcessCore(Encoding.GetBytes(originText), _aes.CreateEncryptor());
            return Convert.ToHexString(encryptedBytes);
        }

        public AesTextCryptographer(byte[] key) : base(key) { }
        public AesTextCryptographer(byte[] key, byte[] iv) : base(key, iv) { }
    }
}
