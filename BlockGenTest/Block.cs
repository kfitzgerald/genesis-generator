using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;

namespace BlockGenTest
{
	public class Block : BlockHeader
	{
		public List<Transaction> vtx { get; set; }
		public List<byte[]> vMerkleTree { get; set; }

		public Block () : base()
		{
			vtx = new List<Transaction> ();
			vMerkleTree = new List<byte[]> ();
		}

//		protected int GetTreeNodeCount(int n)
//		{
//			if (n <= 1) {
//				return 1;
//			} else {
//				n = (n % 2 == 1 ? n + 1 : n); 
//				return n + GetTreeNodeCount(n / 2); 
//			}
//		}

		public BigInteger BuildMerkleTree()
		{
			vMerkleTree = new List<byte[]> ();

			foreach (Transaction tx in vtx) 
			{
				vMerkleTree.Add (tx.GetHash());
			}

			int j = 0;
			for (int nSize = vtx.Count; nSize > 1; nSize = (nSize + 1) / 2)
			{
				for (int i = 0; i < nSize; i += 2)
				{
					int i2 = Math.Min(i+1, nSize-1);
					vMerkleTree.Add(Utilities.Hash(vMerkleTree[j+i], vMerkleTree[j+i2]));
				}
				j += nSize;
			}

			return vMerkleTree.Count == 0 ? new BigInteger (0) : new BigInteger(vMerkleTree [vMerkleTree.Count - 1]);
		}

		public override byte[] Serialize ()
		{
			// Start with the header
			List<byte> buffer = new List<byte> (base.Serialize ());
			Debug.Assert (buffer.Count == 80, "block header should be 80 bytes");
//			List<byte> buffer = new List<byte> ();


			// Tx count (varint)
			buffer.AddRange (Utilities.GetVarIntBytes(vtx.Count)); // Var length int
//			Debug.WriteLine ("tx count {0}, bytes {1}", vtx.Count, buffer.Count);


			// Add transactions
			foreach (Transaction tx in vtx) {
				buffer.AddRange (tx.Serialize ());
			}

			return buffer.ToArray ();
		}

	}
}

/*
 * 1 dhash(a)
 * 2 dhash(b)
 * 3 dhash(c)
 * 4 dhash(d)
 * 5 dhash(dhash(a) + dhash(b))
 * 6 dhash(dhash(c) + dhash(d))
 * 7 dhash(dhash(dhash(a) + dhash(b)) + dhash(dhash(c) + dhash(d)))
 * 
 */