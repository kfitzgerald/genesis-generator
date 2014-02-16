using System;
using System.Numerics;

namespace BlockGenTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
//			Console.WriteLine ("Hello World!");
//
//			byte[] array = Utilities.StringToByteArrayFastest("3BA3EDFD7A7B12B27AC72C3E67768F617FC81BC3888A51323A9FB8AA4B1E5E4A");
//
//			foreach(byte b in array) 
//			{
//				Console.Write("{0} ", Convert.ToString(b, 16));
//			}
//
//			Console.WriteLine ("");
//
//			byte[] intByes = BitConverter.GetBytes (1);
//
//			foreach(byte b in intByes) 
//			{
//				Console.Write("{0} ", Convert.ToString(b, 16).PadLeft(2, '0'));
//			}

//			uint test = 0;
//			byte[] bytes = Utilities.GetVarIntBytes (test);
//			Console.WriteLine ("var int test: {0} => {1}", test, Utilities.GetBytesString (bytes));
//
//			test = 0xfc;
//			bytes = Utilities.GetVarIntBytes (test);
//			Console.WriteLine ("var int test: {0} => {1}", test, Utilities.GetBytesString (bytes));
//
//			test = 0xfd;
//			bytes = Utilities.GetVarIntBytes (test);
//			Console.WriteLine ("var int test: {0} => {1}", test, Utilities.GetBytesString (bytes));
//
//			test = 0xffff;
//			bytes = Utilities.GetVarIntBytes (test);
//			Console.WriteLine ("var int test: {0} => {1}", test, Utilities.GetBytesString (bytes));
//
//			test = 0xffff + 1;
//			bytes = Utilities.GetVarIntBytes (test);
//			Console.WriteLine ("var int test: {0} => {1}", test, Utilities.GetBytesString (bytes));
//
//			test = 0xffffffff;
//			bytes = Utilities.GetVarIntBytes (test);
//			Console.WriteLine ("var int test: {0} => {1}", test, Utilities.GetBytesString (bytes));
//
//			ulong test2 = 0xffffffff;
//			test2 += 0x01;
//			bytes = Utilities.GetVarIntBytes (test2);
//			Console.WriteLine ("var int test: {0} => {1}", test2, Utilities.GetBytesString (bytes));
//
//			test2 = ulong.MaxValue;
//			bytes = Utilities.GetVarIntBytes (test2);
//			Console.WriteLine ("var int test: {0} => {1}", test2, Utilities.GetBytesString (bytes));

//			Console.WriteLine ("BigInteger 4 to bytes: {0}", Utilities.GetBytesString ((new BigInteger (4)).ToByteArray()));
//
//			//
//			// Genisis Block Validation
//			//
//			{
//				Block genesis = new Block ();
//
//				Transaction txNew = new Transaction ();
//				txNew.vin.Add (new TxIn ());
//				txNew.vout.Add (new TxOut ());
//
//				txNew.vin [0].scriptSig = new Script (); 
//				txNew.vin [0].scriptSig += 486604799;
//				txNew.vin [0].scriptSig += new BigInteger (4);
//				txNew.vin [0].scriptSig += "The Times 03/Jan/2009 Chancellor on brink of second bailout for banks";
//
//				txNew.vout [0].nValue = 50 * Utilities.COIN;
//				txNew.vout [0].scriptPubKey = new Script ();
//				txNew.vout [0].scriptPubKey += Utilities.StringToByteArrayFastest ("04678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5f");
//				txNew.vout [0].scriptPubKey += OpCodeType.OP_CHECKSIG;
//
//				genesis.vtx.Add (txNew);
//				genesis.hashPrevBlock = 0;
//				Console.WriteLine ("Computed merkle root is {0}", Utilities.GetBytesString (genesis.BuildMerkleTree ().ToByteArray ()));
//				genesis.hashMerkleRoot = genesis.BuildMerkleTree (); // Utilities.StringToBigInteger("4A5E1E4BAAB89F3A32518A88C31BC87F618F76673E2CC77AB2127B7AFDEDA33B", true);
//				genesis.nVersion = 1;
//				genesis.nTime = 1231006505;
//				genesis.nBits = 0x1d00ffff;
//				genesis.nNonce = 2083236893;
//
//				byte[] blockBytes = genesis.Serialize ();
//
//				Utilities.PrintBytes (blockBytes);
//
//				Console.WriteLine ();
//				Console.WriteLine ("Block hash  : {0}", Utilities.GetBytesString (genesis.GetHash (), false));
//				Console.WriteLine ("Block pretty: {0}", Utilities.GetBytesString (Utilities.ByteSwap(genesis.GetHash()), false));
//			}
//
//			Console.WriteLine ();
//			Console.WriteLine ();


			//
			// TATCOIN BLOCK
			//
//			{
//				Block genesis = new Block ();
//
//				Transaction txNew = new Transaction ();
//				txNew.vin.Add (new TxIn ());
//				txNew.vout.Add (new TxOut ());
//
//				txNew.vin [0].scriptSig = new Script (); 
//				txNew.vin [0].scriptSig += 315532800;
//				txNew.vin [0].scriptSig += new BigInteger (4);
//				txNew.vin [0].scriptSig += "Anatol Rapoport - 1980 Being 'nice' can be beneficial, but it can also lead to being suckered.";
//
//				txNew.vout [0].nValue = 50 * Utilities.COIN;
//				txNew.vout [0].scriptPubKey = new Script ();
//				txNew.vout [0].scriptPubKey += Utilities.StringToByteArrayFastest ("04855B332D7AB6FCABCE6B7282C6D447DB25E77B3FB705F497BD673F3C47FCBF6F2968623BB4047F0D8B59AA1E5CC0A9FFF8315F7D46C75C4D1671EBB6CB27BF46");
//				txNew.vout [0].scriptPubKey += OpCodeType.OP_CHECKSIG;
//
//				genesis.vtx.Add (txNew);
//				genesis.hashPrevBlock = 0;
//				Console.WriteLine ("Computed merkle root is {0}", Utilities.GetBytesString (genesis.BuildMerkleTree ().ToByteArray ()));
//				genesis.hashMerkleRoot = genesis.BuildMerkleTree (); // Utilities.StringToBigInteger("4A5E1E4BAAB89F3A32518A88C31BC87F618F76673E2CC77AB2127B7AFDEDA33B", true);
//				genesis.nVersion = 1;
//
//				genesis.nTime = 1390585017;
//				genesis.nBits = 0x12cea600;
//				genesis.nNonce = 1539371062;
//
//				byte[] blockBytes = genesis.Serialize ();
//
//				Utilities.PrintBytes (blockBytes);Console.WriteLine ();
//				Console.WriteLine ("Block hash  : {0}", Utilities.GetBytesString (genesis.GetHash (), false));
//				Console.WriteLine ("Block pretty: {0}", Utilities.GetBytesString (Utilities.ByteSwap(genesis.GetHash()), false));
//			}
//
//			Console.WriteLine ();
//			Console.WriteLine ();

			//
			// KNOWN BLOCK #282903
			//
//			{
//				Block genesis = new Block ();
//
//				genesis.hashPrevBlock = Utilities.StringToBigInteger("00000000000000006901323a51977f2d324a896a482b81b1a53822c9ed465317", true);
//				genesis.hashMerkleRoot = Utilities.StringToBigInteger("0a4c3baad2c2b43bb8ae35fda46db1d849990df3f13beee0d77ac2d84dcc74d0", true);
//				genesis.nVersion = 2;
//
//				genesis.nTime = 1390923544;
//				genesis.nBits = 419558700;
//				genesis.nNonce = 3670972514;
//
//				byte[] blockBytes = genesis.Serialize ();
//
//				Utilities.PrintBytes (blockBytes);Console.WriteLine ();
//				Console.WriteLine ("Block hash  : {0}", Utilities.GetBytesString (genesis.GetHash (), false));
//				Console.WriteLine ("Block pretty: {0}", Utilities.GetBytesString (Utilities.ByteSwap(genesis.GetHash()), false));
//			}
//
//			Console.WriteLine ();
//			Console.WriteLine ();

			// BITCOIN SOLVER
//			Solver s = new Solver ("04678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5f",  // pubkey
//			                       "The Times 03/Jan/2009 Chancellor on brink of second bailout for banks", // timestamp
//			                       486604799, // nBits
//			                       1231006505, // nTime
//			                       2083236893); // nNonce

			//
			// TATCOIN
			//

			// TATCOIN SOLVER
			{
				Console.WriteLine ("################################ TATCOIN MAIN CHAIN ##################################");
				Solver s = new Solver (
					"04855B332D7AB6FCABCE6B7282C6D447DB25E77B3FB705F497BD673F3C47FCBF6F2968623BB4047F0D8B59AA1E5CC0A9FFF8315F7D46C75C4D1671EBB6CB27BF46", // pubkey
					"Anatol Rapoport-1980 Being 'nice' can be beneficial, but it can also lead to being suckered", // timestamp
					486604799, // timestamp bits
					486604799, // nBits
					315532800, // nTime
					2214851013, // nNonce
					2); // threads

				s.Solve ();
			}
//
//			// TATCOIN TESTNET SOLVER
			{
				Console.WriteLine ("########################### TATCOIN TESTNET CHAIN ############################");
				Solver s = new Solver (
					"04855B332D7AB6FCABCE6B7282C6D447DB25E77B3FB705F497BD673F3C47FCBF6F2968623BB4047F0D8B59AA1E5CC0A9FFF8315F7D46C75C4D1671EBB6CB27BF46", // pubkey
					"Anatol Rapoport-1980 Being 'nice' can be beneficial, but it can also lead to being suckered", // timestamp
					486604799, // timestamp bits
					486604799, // nBits
					315532807, // nTime
					20431863, // nNonce
					2); // threads

				s.Solve ();
			}
//
			// TATCOIN REGTEST SOLVER
			{
				Console.WriteLine ("########################### TATCOIN REGNET CHAIN #############################");
				Solver s = new Solver (
					"04855B332D7AB6FCABCE6B7282C6D447DB25E77B3FB705F497BD673F3C47FCBF6F2968623BB4047F0D8B59AA1E5CC0A9FFF8315F7D46C75C4D1671EBB6CB27BF46", // pubkey
					"Anatol Rapoport-1980 Being 'nice' can be beneficial, but it can also lead to being suckered", // timestamp
					486604799, // timestamp bits
					0x207fffff, // nBits
					1391388473, // nTime
					0, // nNonce
					2); // threads

				s.Solve ();
			}

			//
			// TITCOIN
			//


			// TITCOIN SOLVER
			{
				Console.WriteLine ("################################ TITCOIN MAIN CHAIN ##################################");
//				Solver s = new Solver (
//					"04724E30D26668117A39D62E5C96EA9462437F6A198D206329297A5AA61E363A6EA47F2467C3E64DCF1DE8C1FA689DED52D8B90371CA9FDCCFE28521CB48D60754", // pubkey
//					"Horizon - Richard Dawkins 13/Apr/1986 Nice Guys Finish First", // timestamp
//					486604799, // timestamp bits
//					486604799, // nBits
//					315532800, // nTime
//					2214851013, // nNonce
//					2); // threads
//
//				s.Solve ();
			}
//
//			// TITCOIN TESTNET SOLVER
			{
				Console.WriteLine ("########################### TITCOIN TESTNET CHAIN ############################");
				Solver s = new Solver (
					"04724E30D26668117A39D62E5C96EA9462437F6A198D206329297A5AA61E363A6EA47F2467C3E64DCF1DE8C1FA689DED52D8B90371CA9FDCCFE28521CB48D60754", // pubkey
					"Horizon - Richard Dawkins 13/Apr/1986 Nice Guys Finish First", // timestamp
					486604799, // timestamp bits
					486604799, // nBits
					513820825, // nTime
					1846343381, // nNonce
					2); // threads

				s.Solve ();
			}
//
			// TITCOIN REGTEST SOLVER
			{
				Console.WriteLine ("########################### TITCOIN REGNET CHAIN #############################");
				Solver s = new Solver (
					"04724E30D26668117A39D62E5C96EA9462437F6A198D206329297A5AA61E363A6EA47F2467C3E64DCF1DE8C1FA689DED52D8B90371CA9FDCCFE28521CB48D60754", // pubkey
					"Horizon - Richard Dawkins 13/Apr/1986 Nice Guys Finish First", // timestamp
					486604799, // timestamp bits
					0x207fffff, // nBits
					1391565998, // nTime
					0, // nNonce
					2); // threads

				s.Solve ();
			}


			// Lets check if the nBits parser target thingie works
//			BigInteger target = Utilities.GetBigIntegerFromCompact (0x1d00ffff);
//			BigInteger hash = Utilities.StringToBigInteger ("000000007ac0245a5ec56b6d11698d350b6e28d9ab16bbdbf32f0529d4cb4729", true, true);
//			bool isSolution = hash < target;
//
//			Console.WriteLine ("hash: {0} {1}", hash, Utilities.GetBytesString (hash.ToByteArray (), false));
//			Console.WriteLine ("hash meets difficulty target: {0}", isSolution.ToString ());
//
//
//			BigInteger target2 = Utilities.GetBigIntegerFromCompact (0x207fffff);
//
//			BigInteger target3 = Utilities.GetBigIntegerFromCompact (0x20800001);


			Console.ReadLine ();

		}
	}
}
