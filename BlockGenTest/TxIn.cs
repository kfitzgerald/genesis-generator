using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;

namespace BlockGenTest
{
	public class TxIn : IByteSerializeable
	{
		public OutPoint prevout { get; set; }
		public Script scriptSig { get; set; }
		public uint nSequence { get; set; }

		public TxIn ()
		{
			nSequence = uint.MaxValue;
			scriptSig = new Script ();
			prevout = new OutPoint ();
		}

		public TxIn(OutPoint prevoutIn) : this()
		{
			prevout = prevoutIn;
		}

		public TxIn(OutPoint prevoutIn, Script scriptSigIn) : this(prevoutIn)
		{
			scriptSig = scriptSigIn;
		}

		public TxIn(OutPoint prevoutIn, Script scriptSigIn, uint nSequenceId) : this(prevoutIn, scriptSigIn)
		{
			nSequence = nSequenceId;
		}

		#region IByteSerializeable implementation

		public byte[] Serialize ()
		{
			List<byte> buffer = new List<byte> ();

			// Outpoint
			buffer.AddRange (prevout.Serialize ());
			Debug.Assert (buffer.Count == 36, "outpoint should be 36 bytes");

			// Signature Script
			buffer.AddRange (scriptSig.Serialize ());
			Debug.Assert (buffer.Count >= 37, "scriptsig shoudl be at least one byte long");

			// Sequence
			buffer.AddRange (BitConverter.GetBytes (nSequence));

			return buffer.ToArray ();
		}


		#endregion
	}
}

