namespace HM.Cryptography.Cryptographers
{
    public abstract class CryptographerBase
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public byte[] Key
        {
            get
            {
                _originKey.CopyTo(_copyedKey, 0);
                return _copyedKey;
            }
            set
            {
                _originKey = new byte[value.Length];
                value.CopyTo(_originKey, 0);
                value.CopyTo(_copyedKey, 0);
            }
        }

        protected byte[] _copyedKey = Array.Empty<byte>();
        protected byte[] _originKey = Array.Empty<byte>();
    }
}
