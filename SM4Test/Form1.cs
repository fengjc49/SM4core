using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace SM4Test
{
    public partial class Form1 : Form
    {
        string tempiv = null;//临时初始向量
        //string code = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
           
            SM4Utils sm4 = new SM4Utils();
            //sm4.secretKey = "JeF8U9wHFOMfs2Y8";
            sm4.secretKey = textBox1.Text;
            sm4.hexString = false;
            tempiv = GetRandomString(16, true, true, true, false, null).ToLower();

            sm4.iv = tempiv;
            byte[] bytedata;
            bytedata = Encoding.Default.GetBytes(textBox2.Text);

            string cipher_data = sm4.Encrypt_CBC(Encoding.Default.GetString(bytedata));
            string cipher_data_iv = sm4.iv + cipher_data;
            byte[] cipher_bytedata = Encoding.Default.GetBytes(cipher_data);
            byte[] cipher_bytedata_iv = Encoding.Default.GetBytes(cipher_data_iv);

            string code = Encoding.Default.GetString(cipher_bytedata_iv);
            textBox3.Text = code;
           // MessageBox.Show(Encoding.Default.GetString(cipher_bytedata_iv));

        }

        /// <summary>
        /// 将输入的密钥处理成16字节的伪密钥，密钥长度和分组长度都是128位（16字节）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Keyto16bytes(string key)
        {
            string fakekey;
            int length = key.Length;
            if (length == 16)
            {
                fakekey = key;
                return fakekey;
            }
            else if (length < 16 && length > 0)
            {
                int t = 16 - length;
                string s = string.Empty;
                for (int i = 0; i < t; i++)
                {
                    s += '0';
                }
                fakekey = key + s;
                return fakekey;
            }
            else
            {
                fakekey = key.Substring(0, 16);
                return fakekey;
            }

        }

        

        private void Button2_Click(object sender, EventArgs e)
        {
            SM4Utils sm4 = new SM4Utils();
            //sm4.secretKey = "JeF8U9wHFOMfs2Y8";
            sm4.secretKey = textBox1.Text;
            sm4.hexString = false;
            byte[] bytedata = Encoding.Default.GetBytes(textBox3.Text);
            byte[] temp_iv = new byte[16];

            byte[] file_bytedata = new byte[bytedata.Length - 16];
            Array.Copy(bytedata, temp_iv, 16);
            sm4.iv = Encoding.Default.GetString(temp_iv);
            Array.Copy(bytedata, 16, file_bytedata, 0, (bytedata.Length - 16));
            string plain_data = sm4.Decrypt_CBC(Encoding.Default.GetString(file_bytedata));
           // MessageBox.Show(plain_data, "解密结果");
           textBox4.Text = plain_data;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            SM4Utils sm4 = new SM4Utils();
            //sm4.secretKey = "JeF8U9wHFOMfs2Y8";
            sm4.secretKey = textBox1.Text;
            sm4.hexString = false;
            byte[] bytedata;
            bytedata = Encoding.Default.GetBytes(@"laner");

            string cipher_data = sm4.Encrypt_ECB(Encoding.Default.GetString(bytedata));
            byte[] cipher_bytedata = Encoding.Default.GetBytes(cipher_data);

            //string code = cipher_data;
            // MessageBox.Show(cipher_data,"密文");
            textBox3.Text = cipher_data;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            SM4Utils sm4 = new SM4Utils();
            //sm4.secretKey = "JeF8U9wHFOMfs2Y8";
            sm4.secretKey = textBox1.Text;
            sm4.hexString = false;
            byte[] bytedata;
            bytedata = Encoding.Default.GetBytes(textBox3.Text);
            string plain_data = sm4.Decrypt_ECB(Encoding.Default.GetString(bytedata));
            byte[] plain_bytedata = Encoding.Default.GetBytes(plain_data);

            //MessageBox.Show(plain_data, "解密结果");
            textBox4.Text = plain_data;
        }

        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
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
