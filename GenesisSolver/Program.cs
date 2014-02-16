using System;
using BlockGenTest;

namespace GenesisSolver
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Bitcoin Genesis Block Solver");
			Console.WriteLine ();

			if (args.Length < 3) 
			{
				DisplayUsage ();
				return;
			}

			// Required params
			string pubKey = args [0];
			string timestamp = args [1];
			string nBitsString = args [2];
			string timestampBitsString = args [3];

			// Optional params
			string nTimeString = args.Length >= 5 ? args [4] : null;
			string nNonceString = args.Length >= 6 ? args [5] : null;
			string threadString = args.Length >= 7 ? args [6] : null;

			//
			// Validate params
			//

			// Check public key
			if (pubKey.Length != 130) {
				Console.Error.WriteLine ("Invalid public key. Must be an ECDSA public key, 65 characters in length. {0}" +
					"E.g. 04678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5f", Environment.NewLine);
				return;
			}

			// Check timestamp
			if (timestamp.Length <= 0 || timestamp.Length >= 255) {
				Console.Error.WriteLine (@"Invalid timestamp. Must be a string with length between 1 and 254. {0}" +
					@"E.g. ""The Times 03/Jan/2009 Chancellor on brink of second bailout for banks""", Environment.NewLine);
				return;
			}

			// Check timestamp bits
			uint timestampBits;
			if (!uint.TryParse(timestampBitsString, out timestampBits)) {
				Console.Error.WriteLine ("Invalid timestamp bits. Must be a numeric value, in uint form. {0}" +
					"E.g. 486604799", Environment.NewLine);
				return;
			}

			// Check nBits
			uint nBits;
			if (!uint.TryParse(nBitsString, out nBits)) {
				Console.Error.WriteLine ("Invalid nBits. Must be a numeric value, in uint form. {0}" +
					"E.g. 486604799", Environment.NewLine);
				return;
			}

			// Check nTime
			uint nTimeStart = Utilities.GetCurrentTimestamp(); // Default to now
			if (!String.IsNullOrEmpty(nTimeString) && (!uint.TryParse(nTimeString, out nTimeStart) || nTimeStart == 0)) {
				Console.Error.WriteLine ("Invalid start nTime. Must be a unix timestamp, greater-than zero. {0}" +
					"E.g. 1231006505", Environment.NewLine);
				return;
			}

			// Check nNonce
			uint nNonceStart = 0; // Default to now
			if (!String.IsNullOrEmpty(nNonceString) && !uint.TryParse(nNonceString, out nNonceStart)) {
				Console.Error.WriteLine ("Invalid start nNonce. Must be a uint value, greather-than or equal to zero. {0}" +
					"E.g. 2083236893", Environment.NewLine);
				return;
			}

			// Check thread count
			uint threadCount = 2; // Default to 2
			if (!String.IsNullOrEmpty(threadString) && (!uint.TryParse(threadString, out threadCount) || threadCount < 1)) {
				Console.Error.WriteLine ("Invalid thread count. Must be a uint value, greather-than zero. Should not exceed your processor count. {0}" +
					"E.g. 2", Environment.NewLine);
				return;
			}


			//
			// We got everything we need, let the solving commence!
			//

			// Initialize the block solver
			Solver s = new Solver (pubKey, timestamp, timestampBits, nBits, nTimeStart, nNonceStart, threadCount);

			// Option to override block parameters here e.g.
			// KNOWN BLOCK #282903
//			s.genesis.hashPrevBlock = Utilities.StringToBigInteger("00000000000000006901323a51977f2d324a896a482b81b1a53822c9ed465317", true);
//			s.genesis.hashMerkleRoot = Utilities.StringToBigInteger("0a4c3baad2c2b43bb8ae35fda46db1d849990df3f13beee0d77ac2d84dcc74d0", true);
//			s.genesis.nVersion = 2;
//			s.genesis.nTime = 1390923544;
//			s.genesis.nBits = 419558700;
//			s.genesis.nNonce = 3670972514;

			// Solve it!
			s.Solve ();

			// The winning genesis block is stored here :
//			s.genesis

			// Exit, but some systems are dumb and close the console without asking first
			Console.Write ("Press <enter> to exit... ");
			Console.ReadLine ();

		}

		public static void DisplayUsage()
		{
			Console.WriteLine (@"Usage: {0} <ecdsaPublicKey> <""timestamp""> <timestampBits> <nBits> [starting nTime] [starting nNonce] [threads]", Environment.GetCommandLineArgs()[0]);
		}
	}
}
