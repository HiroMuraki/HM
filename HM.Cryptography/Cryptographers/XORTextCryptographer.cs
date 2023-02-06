using System.Text;

namespace HM.Cryptography.Cryptographers
{
    public class XORTextCryptographer : XORCryptographerBase, ITextCryptographer
    {
        public string Encrypt(string originText)
        {
            if (_originKey.Length == 0)
            {
                return originText;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(originText);
            XOREncryptCore(bytes);
            return Convert.ToHexString(bytes);
        }
        public string Decrypt(string encryptedText)
        {
            if (_originKey.Length == 0)
            {
                return encryptedText;
            }
            byte[] bytes = Convert.FromHexString(encryptedText);
            XOREncryptCore(bytes);
            return Encoding.UTF8.GetString(bytes);
        }

        public XORTextCryptographer(byte[] key) : base(key) { }
    }
}
