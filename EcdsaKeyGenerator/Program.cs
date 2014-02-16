using System;
using BlockGenTest;

namespace EcdsaKeyGenerator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			//Ecdsa.poop ();

			byte[] message = Utilities.StringToBytes ("Hello World");

			//Ecdsa.FullSignatureTest (message);
			Console.WriteLine (Ecdsa.Generate ().ToString ());
			Console.WriteLine (Ecdsa.Generate ().ToString ());
			Console.WriteLine (Ecdsa.Generate ().ToString ());
			Console.WriteLine (Ecdsa.Generate ().ToString ());
			Console.WriteLine (Ecdsa.Generate ().ToString ());


		}
	}
}
