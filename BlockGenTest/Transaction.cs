using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlockGenTest
{
	public class Transaction : IByteSerializeable
	{
		public const int CURRENT_VERSION = 1;

		public Int64 nMinTxFee { get; set; }
		public Int64 nMinRelayTxFee { get; set; }
		public int nVersion { get; set; }
		public List<TxIn> vin { get; set; }
		public List<TxOut> vout { get; set; }
		public uint nLockTime { get; set; }

		public Transaction ()
		{
			nVersion = CURRENT_VERSION;
			vin = new List<TxIn> ();
			vout = new List<TxOut> ();
			nLockTime = 0;
		}

		#region IByteSerializeable implementation

		public byte[] Serialize ()
		{
			List<byte> buffer = new List<byte> ();

			// Version
			buffer.AddRange (BitConverter.GetBytes (nVersion));
			Debug.Assert (buffer.Count == 4, "tx version not 4 bytes");

			// TX IN
			buffer.AddRange (Utilities.GetVarIntBytes(vin.Count)); // Var length int
			foreach (TxIn t in vin) {
				buffer.AddRange (t.Serialize ());
			}

			// TX OUT
			buffer.AddRange (Utilities.GetVarIntBytes(vout.Count)); // Var length int
			foreach (TxOut t in vout) {
				buffer.AddRange (t.Serialize ());
			}

			// Locktime
			buffer.AddRange (BitConverter.GetBytes (nLockTime));

			return buffer.ToArray ();
		}

		#endregion

		public byte[] GetHash() 
		{
			return Utilities.Hash (Serialize ());
		}
	}
}

