using System;
using System.Numerics;

namespace BlockGenTest
{
	public class InPoint
	{
		public Transaction ptx { get; set; }
		public uint n;

		public InPoint ()
		{
			ptx = null;
			n = uint.MaxValue;
		}
	}
}

