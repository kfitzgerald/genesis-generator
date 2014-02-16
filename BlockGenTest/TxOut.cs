using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlockGenTest
{
	public class TxOut : IByteSerializeable
	{
		public Int64 nValue { get; set; }
		public Script scriptPubKey { get; set; }

		public TxOut ()
		{
			nValue = -1;
			scriptPubKey = new Script ();
		}

		public TxOut(Int64 nValueIn) : this()
		{
			nValue = nValueIn;
		}

		public TxOut(Int64 nValueIn, Script scriptPubKeyIn) : this(nValueIn)
		{
			scriptPubKey = scriptPubKeyIn;
		}

		#region IByteSerializeable implementation
		public byte[] Serialize ()
		{
			List<byte> buffer = new List<byte> ();

			// value int64 (8 byte)
			buffer.AddRange (BitConverter.GetBytes (nValue));
			Debug.Assert (buffer.Count == 8, "tx value is not 8 bytes");

			// public key script (1 + n)
			buffer.AddRange (scriptPubKey.Serialize ());
			Debug.Assert (buffer.Count >= 9, "pk script is not long enough");

			return buffer.ToArray ();
		}
		#endregion
	}
}

