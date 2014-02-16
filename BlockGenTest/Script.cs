using System;
using System.Numerics;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlockGenTest
{
	public class Script : IByteSerializeable
	{
		public List<byte> Buffer { get; set;}

		public static Script operator +(Script s1, Script s2) 
		{
			Debug.WriteLine ("Appending two scripts together");
			return new Script (s1, s2);
		}

		public static Script operator +(Script s1, OpCodeType b) 
		{
//			Debug.WriteLine ("Appending opcode {0} which turns into {1}", b, (byte)b);
			s1.Buffer.Add ((byte)b);
			return s1;
		}

		public static Script operator +(Script s1, IEnumerable<byte> b) 
		{
			List<byte> bytes = new List<byte>(b);
//			Debug.WriteLine ("Appending bytes: {0}", Utilities.GetBytesString (b));
			s1.Buffer.Add ((byte)bytes.Count);
			s1.Buffer.AddRange (b);
			return s1;
		}

		public static Script operator +(Script s1, uint b) 
		{
//			Debug.WriteLine ("Appending uint {0}", b);
			s1.Buffer.AddRange (Utilities.GetByteCountWithNumber(b));
			return s1;
		}

		public static Script operator +(Script s1, BigInteger b) 
		{
//			Debug.WriteLine ("Appending BigInteger {0}", b);
			byte[] bytes = b.ToByteArray ();
			s1.Buffer.Add ((byte)bytes.Length);
			s1.Buffer.AddRange (b.ToByteArray());
			return s1;
		}

		public static Script operator +(Script s1, string b) 
		{
//			Debug.WriteLine ("Appending string {0}", b);

			byte[] bytes = Utilities.StringToBytes (b);

			if (bytes.Length < (int)OpCodeType.OP_PUSHDATA1)
			{
				s1.Buffer.Add ((byte)bytes.Length);
			}
			else if (b.Length <= 0xff)
			{
				s1.Buffer.Add ((byte)OpCodeType.OP_PUSHDATA1);
				s1.Buffer.Add ((byte)bytes.Length);
			}
			else if (b.Length <= 0xffff)
			{
				s1.Buffer.Add ((byte)OpCodeType.OP_PUSHDATA2);
				s1.Buffer.AddRange (BitConverter.GetBytes((ushort)bytes.Length));
			}
			else
			{
				s1.Buffer.Add ((byte)OpCodeType.OP_PUSHDATA4);
				s1.Buffer.AddRange (BitConverter.GetBytes((uint)bytes.Length));
			}
			
			s1.Buffer.AddRange (Utilities.StringToBytes(b));
			return s1;
		}

		public Script ()
		{
			Buffer = new List<byte> ();
		}



		public Script(Script s1, Script s2) 
		{
			Buffer.AddRange(s1.Buffer);
			Buffer.AddRange(s2.Buffer);
		}

		public void pushInt64(Int64 n)
		{
			if (n == -1 || (n >= 1 && n <= 16)) 
			{
				Buffer.AddRange (BitConverter.GetBytes (n + Convert.ToInt64(OpCodeType.OP_1 - 1)));
			}
			else 
			{
				Buffer.AddRange (BitConverter.GetBytes (n));
			}
		}

		public void pushUint64(UInt64 n) 
		{
			if (n >= 1 && n <= 16) 
			{
				Buffer.AddRange (BitConverter.GetBytes (n + Convert.ToUInt64(OpCodeType.OP_1 - 1)));
			}
			else 
			{
				Buffer.AddRange (BitConverter.GetBytes (n));
			}
		}

		#region IByteSerializeable implementation
		public byte[] Serialize ()
		{
			List<byte> buffer = new List<byte> ();

			// Var length int
			buffer.AddRange (Utilities.GetVarIntBytes(Buffer.Count)); 

			// The script itself
			buffer.AddRange (Buffer);

			return buffer.ToArray ();
		}
		#endregion
	}
}

