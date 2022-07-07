using System.Text;

namespace HM.Cryptography
{
    public class AesTextEncrypter : AesEncrypterBase, ITextEncrypter
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

        public AesTextEncrypter(byte[] key) : base(key) { }
        public AesTextEncrypter(byte[] key, byte[] iv) : base(key, iv) { }
    }
}
