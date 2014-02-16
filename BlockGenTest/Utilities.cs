using System;
using System.Numerics;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Text;

namespace BlockGenTest
{
	public abstract class Utilities
	{
		public const long COIN = 100000000;
		public const long CENT = 1000000;


		public static uint GetCurrentTimestamp()
		{
			return (uint)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		}

		public static byte[] StringToBytes(string text)
		{
//		    byte[] bytes = new byte[str.Length * sizeof(char)];
//		    System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);

			byte[] bytes = System.Text.Encoding.Default.GetBytes (text);

//			Debug.WriteLine ("String of length {0} became an array of length {1}", str.Length, bytes.Length);
			return bytes;
		}

		public static byte[] StringToByteArrayFastest(string hex) {
			hex = hex.ToUpper ();
			if (hex.Length % 2 == 1)
				throw new Exception("The binary key cannot have an odd number of digits");

			byte[] arr = new byte[hex.Length >> 1];

			for (int i = 0; i < hex.Length >> 1; ++i)
			{
				arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
			}

			return arr;
		}

		public static int GetHexVal(char hex) {
			int val = (int)hex;
			//For uppercase A-F letters:
			return val - (val < 58 ? 48 : 55);
			//For lowercase a-f letters:
			//return val - (val < 58 ? 48 : 87);
			//Or the two combined, but a bit slower:
			//return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
		}

		public static byte[] ByteSwap(byte[] bytes)
		{
			byte[] reversed = new byte[bytes.Length];
			for (var i = 0; i < bytes.Length; i++) 
			{
				reversed [i] = bytes [bytes.Length - 1 - i];
			}
			return reversed;
		}

		public static BigInteger StringToBigInteger(string hex, bool isUnsigned) {
			return StringToBigInteger (hex, true, isUnsigned);
		}

		public static BigInteger StringToBigInteger(string hex, bool byteSwap, bool isUnsigned) {
			hex = hex.ToUpper ();
			byte[] bytes = StringToByteArrayFastest (hex);

			if (byteSwap) {
				bytes = ByteSwap (bytes);
			}

			return BytesToBigInteger (bytes, isUnsigned);
		}

		public static byte[] BytePad(byte[] bytes, int length, byte value)
		{
			List<byte> buffer = new List<byte> (bytes);
			int padCount = length - bytes.Length;

			for (int i = 0; i < padCount; i++) 
			{
				//buffer.Insert (0, value);
				buffer.Add (value);
			}

			return buffer.ToArray();
		}

		public static byte[] BigInt256ToBytes(BigInteger b)
		{
			return BytePad (b.ToByteArray (), 32, 0); 
		}

		public static BigInteger BytesToBigInteger(byte[] bytes, bool isUnsigned)
		{
			// Force it to be unsigned
			if (isUnsigned && (bytes[bytes.Length - 1] & 0x80) > 0) 
			{
				// Copy the bytes into a new temporary array
				byte[] temp = new byte[bytes.Length];
				Array.Copy(bytes, temp, bytes.Length);

				// Resize the original array and copy the bytes back, leaving one 0x00 on the end
				bytes = new byte[temp.Length + 1];
				Array.Copy(temp, bytes, temp.Length);
			}

			return new BigInteger (bytes);
		}

		#region Displaying Bytes As Strings

		public static void PrintBytes(byte[] bytes) 
		{
			Console.Write (GetBytesString (bytes));
		}

		public static string GetBytesString(IEnumerable<byte> bytes) 
		{
			return GetBytesString (bytes, true);
		}

		public static string GetBytesString(IEnumerable<byte> bytes, bool showSpacing) 
		{
			StringBuilder sb = new StringBuilder ();
			int byteCounter = 0;
			foreach(byte b in bytes) 
			{
				byteCounter++;
				sb.AppendFormat("{0}{1}{2}", Convert.ToString(b, 16).PadLeft(2, '0'), showSpacing && (byteCounter % 4 == 0) ? " " : "", showSpacing && (byteCounter % 16 == 0) ? Environment.NewLine : "");
			}
			return sb.ToString ();
		}

		#endregion

		#region Double Hashing 

		public static byte[] Hash(params byte[][] bytes) 
		{
			SHA256 crypto = SHA256Managed.Create ();

			// Join all the byte arrays
			List<byte> totalBytes = new List<byte> ();
			foreach(byte[] b in bytes) 
			{
				totalBytes.AddRange (b);
			}

			// Hash it
			byte[] hash1 = crypto.ComputeHash (totalBytes.ToArray());

			// Hash it again
			byte[] hash2 = crypto.ComputeHash (hash1);

			// Return the double hash
			return hash2; 
		}


		public static BigInteger HashAsBigInteger(params byte[][] bytes) 
		{
			return new BigInteger (Hash (bytes));
		}

		#endregion

		#region Variable Length Unsigned Integer Byte Conversion

		public static byte[] GetVarIntBytes(int n)
		{
			return GetVarIntBytes ((uint)n);
		}

		public static byte[] GetVarIntBytes(uint n)
		{
			return GetVarIntBytes ((ulong)n);
		}

		public static byte[] GetVarIntBytes(ulong n)
		{
			List<byte> bytes = new List<byte> ();
			if (n < 0xfd) { 
				bytes.Add (Convert.ToByte(n));
			} else if (n <= 0xffff) {
				bytes.Add (0xfd);
				bytes.AddRange (BitConverter.GetBytes (Convert.ToUInt16(n)));
			} else if (n <= 0xffffffff) {
				bytes.Add (0xfe);
				bytes.AddRange (BitConverter.GetBytes (Convert.ToUInt32(n)));
			} else {
				bytes.Add (0xff);
				bytes.AddRange (BitConverter.GetBytes(Convert.ToUInt64(n)));
			}

			return bytes.ToArray();
		}

		#endregion

		public static byte[] GetByteCountWithNumber(ulong n)
		{
			List<byte> bytes = new List<byte> ();
			if (n < 0xff) { 
				bytes.Add (0x01);
				bytes.Add (Convert.ToByte(n));
			} else if (n <= 0xffff) {
				bytes.Add (0x02);
				bytes.AddRange (BitConverter.GetBytes (Convert.ToUInt16(n)));
			} else if (n <= 0xffffffff) {
				bytes.Add (0x04);
				bytes.AddRange (BitConverter.GetBytes (Convert.ToUInt32(n)));
			} else {
				bytes.Add (0x08);
				bytes.AddRange (BitConverter.GetBytes(Convert.ToUInt64(n)));
			}

			return bytes.ToArray();
		}



		public static BigInteger GetBigIntegerFromCompact(uint nCompact)
		{
			uint nSize     =  nCompact >> 24;
			bool fNegative = (nCompact & 0x00800000) != 0;
			uint nWord     =  nCompact & 0x007fffff;

			BigInteger result;

			if (nSize <= 3)
			{
				nWord >>= (int)(8 * (3 - nSize));
				result = new BigInteger (nWord);
			}
			else
			{
				result = new BigInteger (nWord);
				result <<= (int)(8 * (nSize - 3));
			}

			if (fNegative) 
			{
				result = result * -1;
			}


//			Console.WriteLine ("Given:  {0} {1}", nCompact, Utilities.GetBytesString(BitConverter.GetBytes(nCompact), false));
//			Console.WriteLine ("nSize:  {0} {1}", nSize, Utilities.GetBytesString(BitConverter.GetBytes(nSize), false));
//			Console.WriteLine ("nWord:  {0} {1}", nWord, Utilities.GetBytesString(BitConverter.GetBytes(nWord), false));
//			Console.WriteLine ("isNeg:  {0}", fNegative.ToString());
//			Console.WriteLine ("Result: {0} {1}", result, Utilities.GetBytesString (result.ToByteArray (), false));

			return result;
		}
	}
}

