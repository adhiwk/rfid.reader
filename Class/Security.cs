using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Rfid.Reader.Class
{
    public sealed class Security
    {
		private static readonly TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
		private static readonly MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
		private static readonly TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
		private static readonly string cHashKey = "S4nd1k4n12345!@#";

		//private byte[] TruncateHash(string key, int length)
		//{

		//	SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

		//	// Hash the key.
		//	byte[] keyBytes = System.Text.Encoding.Unicode.GetBytes(key);
		//	byte[] hash = sha1.ComputeHash(keyBytes);

		//	// Truncate or pad the hash.
		//	Array.Resize(ref hash, length);
		//	sha1.Dispose();
		//	return hash;
		//}

		public static string EncryptData(string plaintext)
		{

			// Convert the plaintext string to a byte array.
			byte[] plaintextBytes = System.Text.Encoding.Unicode.GetBytes(plaintext);

			// Create the stream.
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			// Create the encoder to write to the stream.
			CryptoStream encStream = new CryptoStream(ms, TripleDes.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

			// Use the crypto stream to write the byte array to the stream.
			encStream.Write(plaintextBytes, 0, plaintextBytes.Length);
			encStream.FlushFinalBlock();
			encStream.Dispose();
			// Convert the encrypted stream to a printable string.
			return Convert.ToBase64String(ms.ToArray());
		}

		public static string DecryptData(string encryptedtext)
		{

			// Convert the encrypted text string to a byte array.
			byte[] encryptedBytes = Convert.FromBase64String(encryptedtext);

			// Create the stream.
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			// Create the decoder to write to the stream.
			CryptoStream decStream = new CryptoStream(ms, TripleDes.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

			// Use the crypto stream to write the byte array to the stream.
			decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
			decStream.FlushFinalBlock();
			decStream.Dispose();
			// Convert the plaintext stream to a string.
			return System.Text.Encoding.Unicode.GetString(ms.ToArray());
		}

		public static byte[] MD5Hash(string value)
		{
			return MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value));
		}

		public static string Encrypt(string stringToEncrypt)
		{
			DES.Key = MD5Hash(cHashKey.Trim());
			DES.Mode = CipherMode.ECB;
			byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(stringToEncrypt);
			return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
		}

		public static string Decrypt(string encryptedString)
		{
			string cString = "";
			try
			{
				DES.Key = MD5Hash(cHashKey.Trim());
				DES.Mode = CipherMode.ECB;
				byte[] Buffer = Convert.FromBase64String(encryptedString);
				cString = ASCIIEncoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
			}
			catch (Exception ex)
			{
				MessageBox.Show("Invalid Key", "Decryption Failed, " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			return cString.Trim();
		}

		public static string GetMd5Hash(string input)
		{
			byte[] data = MD5.ComputeHash(Encoding.UTF8.GetBytes(input));
			StringBuilder sBuilder = new StringBuilder();

			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			return sBuilder.ToString();
		}
	}
}
