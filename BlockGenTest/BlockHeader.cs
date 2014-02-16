using System;
using System.Numerics;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlockGenTest
{
	public class BlockHeader : IByteSerializeable
	{
		public const int CURRENT_VERSION = 2;
		public int nVersion { get; set; }
		public BigInteger hashPrevBlock { get; set; }
		public BigInteger hashMerkleRoot { get; set; }
		public uint nTime { get; set; }
		public uint nBits { get; set; }
		public uint nNonce { get; set; }

		public BlockHeader ()
		{
			nVersion = CURRENT_VERSION;
			hashPrevBlock = new BigInteger(0);
			hashMerkleRoot = new BigInteger(0);
			nTime = 0;
			nBits = 0;
			nNonce = 0;
		}

		#region IByteSerializeable implementation

		virtual public byte[] Serialize ()
		{
			return SerializeHeader ();
		}

		public byte[] SerializeHeader()
		{
			List<byte> buffer = new List<byte> ();

			buffer.AddRange (BitConverter.GetBytes (nVersion));
			Debug.Assert (buffer.Count == 4, "version not 4 bytes");

			buffer.AddRange (Utilities.BigInt256ToBytes(hashPrevBlock));
			Debug.Assert (buffer.Count == 36, "hashPrevBlock not 32 bytes");

			buffer.AddRange (Utilities.BigInt256ToBytes(hashMerkleRoot));
			Debug.Assert (buffer.Count == 68, "hashMerkleRoot not 32 bytes");

			buffer.AddRange (BitConverter.GetBytes (nTime));
			Debug.Assert (buffer.Count == 72, "nTime not 4 bytes");

			buffer.AddRange (BitConverter.GetBytes (nBits));
			Debug.Assert (buffer.Count == 76, "nBits not 4 bytes");

			buffer.AddRange (BitConverter.GetBytes (nNonce));
			Debug.Assert (buffer.Count == 80, "nNonce not 4 bytes");

			return buffer.ToArray();
		}

		#endregion

		public byte[] GetHash()
		{
			return Utilities.Hash (SerializeHeader ());
		}

		public BigInteger GetDifficultyTarget() 
		{
			return Utilities.GetBigIntegerFromCompact (nBits);
		}
	}
}

