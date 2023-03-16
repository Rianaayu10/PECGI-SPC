using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace SPCMeasurement
{
    class clsDESEncryption        
    {
        public TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
        
        public clsDESEncryption(string key)
        {
            int x = TripleDes.KeySize / 8;
            int y = TripleDes.BlockSize / 8;
            TripleDes.Key = TruncateHash(key, x);
            TripleDes.IV = TruncateHash("", y);
        }

        private byte[] TruncateHash(string key, int length)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] keyBytes = System.Text.Encoding.Unicode.GetBytes(key);
            byte[] hash = sha1.ComputeHash(keyBytes);
            Array.Resize(ref hash, length);
            return hash;
        }

        public string EncryptData(string plaintext)
        {
            byte[] plaintextBytes = System.Text.Encoding.Unicode.GetBytes(plaintext);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream encStream = new CryptoStream(ms, TripleDes.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
            encStream.Write(plaintextBytes, 0, plaintextBytes.Length);
            encStream.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        public string DecryptData(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream decStream = new CryptoStream(ms, TripleDes.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            decStream.FlushFinalBlock();
            return System.Text.Encoding.Unicode.GetString(ms.ToArray());
        }

        public string Encrypt(string clearText, string EncryptionKey)
        {
            EncryptionKey = EncryptionKey + "WebPECGI2020";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = System.Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Decrypt(string cipherText, string EncryptionKey)
        {
            if (cipherText.Length < 5)
            {
                return cipherText;
            }
            EncryptionKey = EncryptionKey + "WebPECGI2020";
            byte[] cipherBytes = System.Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
