using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HM.Cryptography;
using System.Security.Cryptography;

namespace LibTest.Cryptography
{
    [TestClass]
    public class AesEncrypterTest
    {
        [TestMethod]
        public void TextEncryptionTest()
        {
            byte[][] keys;
            using (var sha256 = SHA256.Create())
            {
                keys = new byte[][]
                {
                    sha256.ComputeHash(Encoding.ASCII.GetBytes("123456")),
                    sha256.ComputeHash(Encoding.ASCII.GetBytes("")),
                    sha256.ComputeHash(Encoding.ASCII.GetBytes("Abc_123456")),
                    sha256.ComputeHash(Encoding.ASCII.GetBytes("joieajioaset9ae8sut43u986239846j436j4236j-4392626u489236u")),
                    sha256.ComputeHash(Encoding.ASCII.GetBytes("_F(S*EJF(*SE*(SEUTSE*(TSJT"))
                };
            }


            Assert.ThrowsException<ArgumentException>(() => new AesTextEncrypter(new byte[] { 1, 2, 3 }));
            Assert.ThrowsException<ArgumentException>(() => new AesTextEncrypter(new byte[] { }));

            foreach (var key in keys)
            {
                string[] texts =
                {
                    "Hello World!",
                    "",
                    @"Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello
                      World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!",
                    "中文测试中文测试中文测试中文测试中文测试中文测试"
                };
                string[] eTexts = texts.Select(t => new AesTextEncrypter(key).Encrypt(t)).ToArray();

                for (int textIndex = 0; textIndex < texts.Length; textIndex++)
                {
                    // 相同密钥，相同明文，每一次加密的结果都应该一样
                    for (int _ = 0; _ < 100; _++)
                    {
                        Assert.AreEqual(eTexts[textIndex], new AesTextEncrypter(key).Encrypt(texts[textIndex]));
                    }
                    // 相同密文，相同明文，每一次加密的结果都应该一样
                    for (int _ = 0; _ < 100; _++)
                    {
                        Assert.AreEqual(texts[textIndex], new AesTextEncrypter(key).Decrypt(eTexts[textIndex]));
                    }
                }
            }
        }

        [TestMethod]
        public void BytesEncryptionTest()
        {
            var comparer = MDArrayComparsion<byte>.Default;
            byte[][] keys;
            using (var sha256 = SHA256.Create())
            {
                keys = new byte[][]
                {
                    sha256.ComputeHash(Encoding.ASCII.GetBytes("123456")),
                    sha256.ComputeHash(Encoding.ASCII.GetBytes("")),
                    sha256.ComputeHash(Encoding.ASCII.GetBytes("Abc_123456")),
                    sha256.ComputeHash(Encoding.ASCII.GetBytes("joieajioaset9ae8sut43u986239846j436j4236j-4392626u489236u")),
                    sha256.ComputeHash(Encoding.ASCII.GetBytes("_F(S*EJF(*SE*(SEUTSE*(TSJT"))
                };
            }

            Assert.ThrowsException<ArgumentException>(() => new AesBytesEncrypter(new byte[] { 1, 2, 3 }));
            Assert.ThrowsException<ArgumentException>(() => new AesBytesEncrypter(new byte[] { }));

            foreach (var key in keys)
            {
                byte[][] bytes =
                {
                    new byte[]{0x00,0x01,0x02,0x03},
                    new byte[]{},
                    new byte[]{0xFF,0xFE,0xFA,0x00,0x2A},
                };
                byte[][] eBytes = bytes.Select(t => new AesBytesEncrypter(key).Encrypt(t).ToArray()).ToArray();

                for (int bIndex = 0; bIndex < bytes.Length; bIndex++)
                {
                    // 相同密钥，相同明文，每一次加密的结果都应该一样
                    for (int _ = 0; _ < 100; _++)
                    {
                        Assert.IsTrue(eBytes[bIndex].SequenceEqual(new AesBytesEncrypter(key).Encrypt(bytes[bIndex]).ToArray()));
                    }
                    // 相同密文，相同明文，每一次加密的结果都应该一样
                    for (int _ = 0; _ < 100; _++)
                    {
                        Assert.IsTrue(bytes[bIndex].SequenceEqual(new AesBytesEncrypter(key).Decrypt(eBytes[bIndex]).ToArray()));
                    }
                }
            }
        }
    }
}
