using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using System.Web.Security;
using System.Web.Configuration;
using System.Web;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

namespace CustomAuth
{
    public class AuthUtility
    {
        public static string AUTHENTICATION_LOGINURL = ConfigurationManager.AppSettings["CustomAuthentication.LoginUrl"];
        public static string AUTHENTICATION_COOKIE_NAME = ConfigurationManager.AppSettings["CustomAuthentication.CookieName"].ToUpper();
        public static string AUTHENTICATION_COOKIE_TIMEOUT = ConfigurationManager.AppSettings["CustomAuthentication.CookieTimeout"];

        private const string Key = "750#$H@mBhU#89";
        private static byte[] key = Encoding.UTF8.GetBytes(Key.Substring(2, 8));
        private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// Created By : Shambhu Pasi
        /// Description : This function is used to Encrypt the Plain Text into Cipher Text.
        /// </summary>
        /// <param name="strPlainText"></param>
        /// <returns></returns>
        public static string Encrypt(object objModel)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray = BinarySerialize(objModel);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Created By : Shambhu Pasi
        /// Description : This function is used to Decrypt the Cipher Text into Plain Text.
        /// </summary>
        /// <param name="strCipherText"></param>
        /// <returns></returns>
        public static object Decrypt(string strCipherText)
        {
            strCipherText = strCipherText.Replace(' ', '+');
            Byte[] inputByteArray = new Byte[strCipherText.Length];
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strCipherText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                BinaryFormatter bf = new BinaryFormatter();
                ms.Position = 0;
                object objResult = bf.Deserialize(ms);
                //MemoryStream ms = new MemoryStream(data);
                return objResult;
                //Encoding encoding = Encoding.UTF8;
                //return encoding.GetString(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }

        public static byte[] BinarySerialize(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static object BinaryDeSerialize(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(data);
                return bf.Deserialize(ms);
            }
        }
    }
}
