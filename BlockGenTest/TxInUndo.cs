using System;

namespace BlockGenTest
{
	public class TxInUndo
	{
		public TxOut txout { get; set; } 
		public bool fCoinBase { get; set; }
		public uint nHeight { get; set; }
		public int nVersion { get; set; }

		public TxInUndo ()
		{

		}


	}
}

