using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace SM4Core
{
    public class SM4Utils
    {
        public string secretKey = "";
        public string iv = "";
        public bool hexString = false;
        public byte[] secretKeyBuff;

        public string Encrypt_ECB(string plainData)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_ENCRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Decode(secretKey);
            }
            else
            {
                keyBytes = Encoding.ASCII.GetBytes(secretKey);
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_ecb(ctx, Encoding.Default.GetBytes(plainData));
            //return encrypted;
            string cipherText = Encoding.Default.GetString(Encode(encrypted));
            return cipherText;
        }

        public string Decrypt_ECB(string cipherText)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_DECRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Decode(secretKey);
            }
            else
            {
                keyBytes = Encoding.ASCII.GetBytes(secretKey);
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_ecb(ctx, Decode(cipherText));
            if (decrypted == null)
            {
                return string.Empty;
            }
            else
            {
                return Encoding.Default.GetString(decrypted);
            }
        }

        public string Encrypt_CBC(string plainData)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_ENCRYPT;

            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Decode(secretKey);
                ivBytes = Decode(iv);
            }
            else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
                ivBytes = Encoding.Default.GetBytes(iv);
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Encoding.Default.GetBytes(plainData));

            //return Hex.Encode(encrypted);
            //return encrypted;
            string cipherText = Encoding.Default.GetString(Encode(encrypted));
            return cipherText;
        }

        public string Decrypt_CBC(string cipherData)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_DECRYPT;

            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Decode(secretKey);
                ivBytes = Decode(iv);
            }
            else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
                ivBytes = Encoding.Default.GetBytes(iv);
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Decode(cipherData));
            if (decrypted == null)
            {
                return string.Empty;
            }
            else
            {
                return Encoding.Default.GetString(decrypted);
            }
            //return decrypted;
        }


        public byte[] Encode(byte[] data)
        {
            int off = 0;
            int length = data.Length;

            MemoryStream memoryStream = new MemoryStream(length * 2);

            for (int index = off; index < off + length; ++index)
            {
                int num = (int)data[index];
                memoryStream.WriteByte(this.encodingTable[num >> 4]);
                memoryStream.WriteByte(this.encodingTable[num & 15]);
            }
            length =  length * 2;

            return memoryStream.ToArray();
        }


 

        public byte[] Decode(string data)
        {
            MemoryStream memoryStream = new MemoryStream((data.Length + 1) / 2);


            int num1 = 0;
            int length = data.Length;
            while (length > 0 && Ignore(data[length - 1]))
                --length;
            int index1 = 0;
            InitialiseDecodingTable();
            while (index1 < length)
            {
                while (index1 < length && Ignore(data[index1]))
                    ++index1;
                byte[] decodingTable1 = this.decodingTable;
                string str1 = data;
                int index2 = index1;
                int index3 = index2 + 1;
                int index4 = (int)str1[index2];
                byte num2 = decodingTable1[index4];
                while (index3 < length && Ignore(data[index3]))
                    ++index3;
                byte[] decodingTable2 = this.decodingTable;
                string str2 = data;
                int index5 = index3;
                index1 = index5 + 1;
                int index6 = (int)str2[index5];
                byte num3 = decodingTable2[index6];
                if (((int)num2 | (int)num3) >= 128)
                    throw new IOException("invalid characters encountered in Hex data");
                memoryStream.WriteByte((byte)((uint)num2 << 4 | (uint)num3));
                ++num1;
            }
            // return num1;
            byte[] rst = memoryStream.ToArray();
            return memoryStream.ToArray();
        }

        


        public byte[] Decode(byte[] data)
        {
            int off = 0;
            int length = data.Length;
            MemoryStream memoryStream = new MemoryStream(length * 2);

            for (int index = off; index < off + length; ++index)
            {
                int num = (int)data[index];
                memoryStream.WriteByte(this.encodingTable[num >> 4]);
                memoryStream.WriteByte(this.encodingTable[num & 15]);
            }
            length = length * 2;

            return memoryStream.ToArray();

        }

        protected  byte[] decodingTable = new byte[128];


        public static void Fill(byte[] buf, byte b)
        {
            int length = buf.Length;
            while (length > 0)
                buf[--length] = b;
        }
        protected void InitialiseDecodingTable()
        {
            //Arrays.Fill(this.decodingTable, byte.MaxValue);

            int length = this.decodingTable.Length;
            while (length>0)
            {
                this.decodingTable[--length] = byte.MaxValue;
            }


            for (int index = 0; index < this.encodingTable.Length; ++index)
                this.decodingTable[(int)this.encodingTable[index]] = (byte)index;
            this.decodingTable[65] = this.decodingTable[97];
            this.decodingTable[66] = this.decodingTable[98];
            this.decodingTable[67] = this.decodingTable[99];
            this.decodingTable[68] = this.decodingTable[100];
            this.decodingTable[69] = this.decodingTable[101];
            this.decodingTable[70] = this.decodingTable[102];
        }

        protected byte[] encodingTable =
        {
            (byte) 48,
            (byte) 49,
            (byte) 50,
            (byte) 51,
            (byte) 52,
            (byte) 53,
            (byte) 54,
            (byte) 55,
            (byte) 56,
            (byte) 57,
            (byte) 97,
            (byte) 98,
            (byte) 99,
            (byte) 100,
            (byte) 101,
            (byte) 102
        };

        private static bool ignore(char c)
        {
            return c == '\n' || c == '\r' || c == '\t' || c == ' ';
        }

        private static bool Ignore(char c)
        {
            if (c != '\n' && c != '\r' && c != '\t')
                return c == ' ';
            return true;
        }
    }
}
