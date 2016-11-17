using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace General.Helpers {
    public class CryptoHelper {

        /// <summary>
        /// Compute SHA1 hash of string
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Base64-encoded hash</returns>
        public static string Hash(string value, string salt) {
            byte[] password = UTF8Encoding.UTF8.GetBytes(value);
            var hmacSHA1 = new HMACSHA1(UTF8Encoding.UTF8.GetBytes(salt));
            var saltedHash = hmacSHA1.ComputeHash(password);
            return Convert.ToBase64String(saltedHash);
        }

        private static Random random = new Random((int)DateTime.Now.Ticks);

        public static string RandomString(int size) {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++) {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public static string HashString(string Value) {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            x.Clear();
            return ret;
        }

        public static string FileHash(string fileName, string salt) {
            return HashString(fileName + salt).Substring(0, 8);
        }

        private static string _GenerateRandom(String salt) {
            Random r = new Random();
            string random = r.Next(1000000000).ToString() + r.Next(1000000000).ToString() + r.Next(1000000000).ToString() + DateTime.Now + salt;
            return random;
        }

        public static string Encrypt(string toEncrypt) {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            // Get the key from config file

            string key = MD5Salt();

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Encrypt(string toEncrypt, string key) {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            // Get the key from config file


            System.Security.Cryptography.MD5CryptoServiceProvider hashmd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
            System.Security.Cryptography.TripleDESCryptoServiceProvider tdes = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = System.Security.Cryptography.CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            System.Security.Cryptography.ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString) {
            try {
                byte[] keyArray;
                //get the byte code of the string

                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                string key = MD5Salt();


                //if hashing was used get the hash code with regards to your key
                System.Security.Cryptography.MD5CryptoServiceProvider hashmd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)

                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor                
                tdes.Clear();
                //return the Clear decrypted TEXT
                return UTF8Encoding.UTF8.GetString(resultArray);
            } catch {
                return "";
            }
        }

        public static string Decrypt(string cipherString, string key) {
            //try
            //{
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //if hashing was used get the hash code with regards to your key
            System.Security.Cryptography.MD5CryptoServiceProvider hashmd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //release any resource held by the MD5CryptoServiceProvider

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string MD5Salt() {
            return _MD5Salt;
        }

        public static string MD5Salt_alt() {
            return "srw989hsrh98$W^Wh96|$^$^$$^w498hgeydsk5Hr4634H90y4hea0idamhotkj$3yW$YW$Y4w6yy4wit6^$Wdsk5Hr4634H90y4";
        }

        public static string MD5Salt_API() {
            return "srh98$98hgeydsk5Hr463hrjt5u5rnj5ea0idamhotkjsrw989h$3yW$YW$Y4w6yy4wit6^$Wdsk5Hr4634H90y4";
        }

        private static string _MD5Salt {
            get { return "dsk5Hr4634H90y4srh96|$^$^$w989hsrh$3yW$YW$Y4w6y98$W^W$^w498hgeyhea0idamhotkjy4wit6^$W)|"; }
        }

        private static byte[] _SHASalt {
            get { return Encoding.UTF8.GetBytes("lue136hsrh38$W^Wh96|a^$^*$^w423hgeydsk5Hr4634H50y4hea0idamhetkj$3yW$YW$Y5w7yy2uau7^$Wdsk5Hr8493H90y4"); }
        }
    }
}
