using System;
using System.Collections.Generic;

namespace BlockGenTest
{
	public class TxUndo
	{
		public List<TxInUndo> vprevout { get; set; }

		public TxUndo ()
		{
			vprevout = new List<TxInUndo> ();
		}
	}
}

