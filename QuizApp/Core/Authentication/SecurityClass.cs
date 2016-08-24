using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace QuizApp.Core.Authentication
{
    public static class SecurityClass
    {
        private const string initVector = "$2a81oP[}'(`sY&6";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;
        //Encrypt
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        //Decrypt
        public static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }



        public class SimpleAES
        {
            // Change these keys
            private byte[] Key = { 123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };
            private byte[] Vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 2521, 112, 79, 32, 114, 156 };


            private ICryptoTransform EncryptorTransform, DecryptorTransform;
            private System.Text.UTF8Encoding UTFEncoder;

            public SimpleAES()
            {
                //This is our encryption method
                RijndaelManaged rm = new RijndaelManaged();

                //Create an encryptor and a decryptor using our encryption method, key, and vector.
                EncryptorTransform = rm.CreateEncryptor(this.Key, this.Vector);
                DecryptorTransform = rm.CreateDecryptor(this.Key, this.Vector);

                //Used to translate bytes to text and vice versa
                UTFEncoder = new System.Text.UTF8Encoding();
            }

            /// -------------- Two Utility Methods (not used but may be useful) -----------
            /// Generates an encryption key.
            static public byte[] GenerateEncryptionKey()
            {
                //Generate a Key.
                RijndaelManaged rm = new RijndaelManaged();
                rm.GenerateKey();
                return rm.Key;
            }

            /// Generates a unique encryption vector
            static public byte[] GenerateEncryptionVector()
            {
                //Generate a Vector
                RijndaelManaged rm = new RijndaelManaged();
                rm.GenerateIV();
                return rm.IV;
            }


            /// ----------- The commonly used methods ------------------------------    
            /// Encrypt some text and return a string suitable for passing in a URL.
            public string EncryptToString(string TextValue)
            {
                return ByteArrToString(Encrypt(TextValue));
            }

            /// Encrypt some text and return an encrypted byte array.
            public byte[] Encrypt(string TextValue)
            {
                //Translates our text value into a byte array.
                Byte[] bytes = UTFEncoder.GetBytes(TextValue);

                //Used to stream the data in and out of the CryptoStream.
                MemoryStream memoryStream = new MemoryStream();

                /*
                 * We will have to write the unencrypted bytes to the stream,
                 * then read the encrypted result back from the stream.
                 */
                #region Write the decrypted value to the encryption stream
                CryptoStream cs = new CryptoStream(memoryStream, EncryptorTransform, CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
                #endregion

                #region Read encrypted value back out of the stream
                memoryStream.Position = 0;
                byte[] encrypted = new byte[memoryStream.Length];
                memoryStream.Read(encrypted, 0, encrypted.Length);
                #endregion

                //Clean up.
                cs.Close();
                memoryStream.Close();

                return encrypted;
            }

            /// The other side: Decryption methods
            public string DecryptString(string EncryptedString)
            {
                return Decrypt(StrToByteArray(EncryptedString));
            }

            /// Decryption when working with byte arrays.    
            public string Decrypt(byte[] EncryptedValue)
            {
                #region Write the encrypted value to the decryption stream
                MemoryStream encryptedStream = new MemoryStream();
                CryptoStream decryptStream = new CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write);
                decryptStream.Write(EncryptedValue, 0, EncryptedValue.Length);
                decryptStream.FlushFinalBlock();
                #endregion

                #region Read the decrypted value from the stream.
                encryptedStream.Position = 0;
                Byte[] decryptedBytes = new Byte[encryptedStream.Length];
                encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                encryptedStream.Close();
                #endregion
                return UTFEncoder.GetString(decryptedBytes);
            }

            /// Convert a string to a byte array.  NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so).
            //      System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            //      return encoding.GetBytes(str);
            // However, this results in character values that cannot be passed in a URL.  So, instead, I just
            // lay out all of the byte values in a long string of numbers (three per - must pad numbers less than 100).
            public byte[] StrToByteArray(string str)
            {
                if (str.Length == 0)
                    throw new Exception("Invalid string value in StrToByteArray");

                byte val;
                byte[] byteArr = new byte[str.Length / 3];
                int i = 0;
                int j = 0;
                do
                {
                    val = byte.Parse(str.Substring(i, 3));
                    byteArr[j++] = val;
                    i += 3;
                }
                while (i < str.Length);
                return byteArr;
            }

            // Same comment as above.  Normally the conversion would use an ASCII encoding in the other direction:
            //      System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            //      return enc.GetString(byteArr);    
            public string ByteArrToString(byte[] byteArr)
            {
                byte val;
                string tempStr = "";
                for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
                {
                    val = byteArr[i];
                    if (val < (byte)10)
                        tempStr += "00" + val.ToString();
                    else if (val < (byte)100)
                        tempStr += "0" + val.ToString();
                    else
                        tempStr += val.ToString();
                }
                return tempStr;
            }
        }
}