using System;
using System.Numerics;
using System.Collections.Generic;
using System.Timers;
using System.Threading;

namespace BlockGenTest
{
	public class Solver
	{
		public Block genesis { get; set; }
		public uint time { get; set; }
		public uint nonce { get; set; }

		protected long hashesSinceLastTick;
		protected System.Timers.Timer hashTimer { get; set; }
		protected Mutex jobMutex { get; set; }
		protected bool jobDone { get; set; }
		protected bool isFirstJob { get; set; }
		public uint WorkerCount { get; set; }

		public Solver (string publicKey, string timestamp, uint timestampBits, uint nBits, uint startTime, uint startNonce, uint workerCount)
		{
			genesis = new Block ();

			Transaction txNew = new Transaction ();
			txNew.vin.Add (new TxIn ());
			txNew.vout.Add (new TxOut ());

			txNew.vin [0].scriptSig = new Script (); 
			txNew.vin [0].scriptSig += timestampBits;
			txNew.vin [0].scriptSig += new BigInteger (4);
			txNew.vin [0].scriptSig += timestamp;

			txNew.vout [0].nValue = 50 * Utilities.COIN;
			txNew.vout [0].scriptPubKey = new Script ();
			txNew.vout [0].scriptPubKey += Utilities.StringToByteArrayFastest (publicKey);
			txNew.vout [0].scriptPubKey += OpCodeType.OP_CHECKSIG;

			genesis.vtx.Add (txNew);
			genesis.hashPrevBlock = 0;
			Console.WriteLine ("Merkle root is {0}", Utilities.GetBytesString (genesis.BuildMerkleTree ().ToByteArray (), false));
			Console.WriteLine ("Merkle swap is {0}", Utilities.GetBytesString (Utilities.ByteSwap(genesis.BuildMerkleTree ().ToByteArray ()), false));
			BigInteger diff = Utilities.GetBigIntegerFromCompact (nBits);
			Console.WriteLine ();
			Console.WriteLine ("Target: {0}", diff);
			Console.WriteLine ("Difficulty: {0}", Utilities.GetBytesString (Utilities.ByteSwap(Utilities.BytePad(diff.ToByteArray (), 32, 0)), false));
			Console.WriteLine ();
			genesis.hashMerkleRoot = genesis.BuildMerkleTree ();

			genesis.nVersion = 1;
			genesis.nBits = nBits;

			// Set the starting values
			time = startTime;
			nonce = startNonce;

			genesis.nTime = time == 0 ? Utilities.GetCurrentTimestamp () : time;
			genesis.nNonce = nonce;

			jobMutex = new Mutex ();
			jobDone = false;
			hashesSinceLastTick = 0;
			isFirstJob = true;
			WorkerCount = workerCount;
		}


		public void Solve()
		{
			// Environment.ProcessorCount

			// Start the monitoring timer
			hashTimer = new System.Timers.Timer (5000);
			hashTimer.Elapsed += new ElapsedEventHandler (OnTimedEvent);
			hashTimer.Enabled = true;

			Thread[] workers = new Thread[WorkerCount];

			Console.WriteLine ("Spawning {0} workers to solve the genesis block...", WorkerCount);

			// Spawn a worker thread for each processor we have on this machine
			for(var i = 0; i < WorkerCount; i++)
			{
				workers [i] = new Thread (new ThreadStart (JobWorker));
				workers [i].Start ();
			}

			// Wait for the workers to complete
			for(var i = 0; i < WorkerCount; i++)
			{
				// Wait for the workers to complete
				workers [i].Join ();
			}

			Console.WriteLine ("Work complete.");

//
//			while (true) 
//			{
//				byte[] hash = genesis.GetHash ();
//				uint last4 = BitConverter.ToUInt32 (hash, hash.Length - 4);
//
//				if (last4 == 0) 
//				{
//					hashTimer.Enabled = false;
//					Console.WriteLine ();
//					Console.WriteLine (" ** BLOCK SOLVED ** nNonce {0}, nTime {1}", genesis.nNonce, genesis.nTime);
//					Console.WriteLine ("Block hash  : {0}", Utilities.GetBytesString (hash, false));
//					Console.WriteLine ("Block pretty: {0}", Utilities.GetBytesString (Utilities.ByteSwap (hash), false));
//
//					Console.WriteLine ();
//					byte[] blockBytes = genesis.Serialize ();
//					Utilities.PrintBytes (blockBytes);Console.WriteLine ();
//					Console.WriteLine ();
//
//					break;
//				}
//
//				if (genesis.nNonce == uint.MaxValue) 
//				{
//					genesis.nTime++;
//					genesis.nNonce = 0;
//				}
//				else
//				{
//					genesis.nNonce++;
//				}
//
//				hashesSinceLastTick++;
//
//			}

		}

		protected uint[] GetNextJob()
		{
			if (jobMutex.WaitOne ()) {

				if (isFirstJob) {
					// First job - don't increment or decrement
					isFirstJob = false;
				} else {

					// Get the next job
					if (genesis.nNonce == uint.MaxValue) {
						genesis.nTime++;
						genesis.nNonce = 0;
					} else {
						genesis.nNonce++;
					}
				}

				var job = new uint[2] { genesis.nTime, genesis.nNonce };

				jobMutex.ReleaseMutex ();

				return job;

			} else {
				return GetNextJob ();
			}
		}

		protected void JobWorker()
		{
			// Get the header block template
			byte[] headerBlock = genesis.SerializeHeader ();
			BigInteger difficulty = Utilities.GetBigIntegerFromCompact (genesis.nBits);


			while (!jobDone) 
			{
				// Gext the next job
				uint[] job = GetNextJob();

				// Apply the job to the header
				// nTime  -> index 68-72
				// nNonce -> index 76-80
				BitConverter.GetBytes (job [0]).CopyTo (headerBlock, 68);
				BitConverter.GetBytes (job [1]).CopyTo (headerBlock, 76);

				// Get the hash of the block
				byte[] hash = Utilities.Hash (headerBlock);

				// Check debug check - run one job
//				if (jobMutex.WaitOne ()) {
//					Utilities.PrintBytes (headerBlock); 
//					Console.WriteLine ();
//					Console.WriteLine ();
//					Console.WriteLine ("Block hash  : {0}", Utilities.GetBytesString (hash, false));
//					Console.WriteLine ("Block pretty: {0}", Utilities.GetBytesString (Utilities.ByteSwap (hash), false));
//					jobMutex.ReleaseMutex ();
//					return;
//				}

				// Check the hash against the nBits difficulty target
				BigInteger hashVal = Utilities.BytesToBigInteger (hash, true);

				// Check if we have the winning block
				if (hashVal < difficulty) {

					//
					// FOUND THE BLOCK
					//

					// Stop the workers
					if (jobMutex.WaitOne ()) {

						jobDone = true;
						hashTimer.Enabled = false;

						// Apply the final values
						genesis.nTime = job [0];
						genesis.nNonce = job [1];

						// Show the magic
						Console.WriteLine ();
						Console.WriteLine (" ** BLOCK SOLVED **");
						Console.WriteLine ("Found at: {0} {1} ({2})", DateTime.Now.ToShortDateString (), DateTime.Now.ToShortTimeString (), Utilities.GetCurrentTimestamp());
						Console.WriteLine ("nBits   : {0} (0x{1})", genesis.nBits, Utilities.GetBytesString(Utilities.ByteSwap(BitConverter.GetBytes(genesis.nBits)), false));
						Console.WriteLine ("nTime   : {0}", genesis.nTime);
						Console.WriteLine ("nNonce  : {0}", genesis.nNonce);
						Console.WriteLine ();
						Console.WriteLine ("Hash    : {0}", Utilities.GetBytesString (hash, false));
						Console.WriteLine ("Swapped : {0}", Utilities.GetBytesString (Utilities.ByteSwap (hash), false));
						Console.WriteLine ("Numeric : {0}", hashVal);
						Console.WriteLine ();

						// Show the raw block
						byte[] blockBytes = genesis.Serialize ();
						Utilities.PrintBytes (blockBytes);
						Console.WriteLine ();
						Console.WriteLine ();

						jobMutex.ReleaseMutex ();
					}

					// Get out of here
					return;

				}

				// Not solved, iterate the hash counter and do the next one
				Interlocked.Increment (ref hashesSinceLastTick);

			}

		}

		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			Console.Write("\rHashrate: {0}/sec, nNonce={1}, nTime={2}              ", hashesSinceLastTick / 5, genesis.nNonce, genesis.nTime);
			Interlocked.Exchange (ref hashesSinceLastTick, 0);
		}
	}
}

