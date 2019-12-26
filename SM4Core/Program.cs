using System;
using System.Text;

namespace SM4Core
{
    class Program
    {
        static void Main(string[] args)
        {
            string cbc = CBCEncode();

            string cbctst =  CBCDecode(cbc);


            string ebc = EBCEncode();

            string rst = EBCDecode(ebc);

        }

        private static string EBCDecode(string ebc)
        {
            SM4Utils sm4 = new SM4Utils {secretKey = "JeF8U9wHFOMfs2Y8", hexString = false};
            string plain_data = sm4.Decrypt_ECB(ebc); 
            Console.WriteLine("解密结果是{0}", plain_data);
            return plain_data;
        }

        private static string EBCEncode()
        {
            SM4Utils sm4 = new SM4Utils {secretKey = "JeF8U9wHFOMfs2Y8", hexString = false};
            string cipherData = sm4.Encrypt_ECB(@"This is a test string !");
            Console.WriteLine("加密结果是{0}", cipherData);
            return cipherData;
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="cbc"></param>
        /// <returns></returns>
        private static string CBCDecode(string cbc)
        {
            SM4Utils sm4 = new SM4Utils();
            sm4.secretKey = "JeF8U9wHFOMfs2Y8";
            byte[] bytedata = Encoding.Default.GetBytes(cbc);
            byte[] temp_iv = new byte[16];

            byte[] file_bytedata = new byte[bytedata.Length - 16];
            Array.Copy(bytedata, temp_iv, 16);
            sm4.iv = Encoding.Default.GetString(temp_iv);
            Array.Copy(bytedata, 16, file_bytedata, 0, (bytedata.Length - 16));
            string plain_data = sm4.Decrypt_CBC(Encoding.Default.GetString(file_bytedata));

            Console.WriteLine("解密结果是{0}", plain_data);

            return plain_data;



        }

        //加密
        private static string CBCEncode()
        {
            Console.WriteLine("CBC模式加密");
            SM4Utils sm4 = new SM4Utils();
            sm4.secretKey = "JeF8U9wHFOMfs2Y8";
            string tempiv = GetRandomString(16, true, true, true, false, null).ToLower();
            sm4.iv = tempiv; 
            string cipher_data = sm4.Encrypt_CBC(@"This is a test string !");
            string cipher_data_iv = sm4.iv + cipher_data;

            Console.WriteLine("加密结果是{0}", cipher_data_iv);
            return cipher_data_iv;
        }

        private static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
    }
}
