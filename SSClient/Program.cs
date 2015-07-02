using System;
using System.Collections.Generic;

namespace SSCyg.Client
{
	class Program
	{

		static void Main()
		{
			List<string> commandLine = new List<string>(Environment.GetCommandLineArgs());
			commandLine.RemoveAt(0); // Remove program name

			// TODO: Process all command line args for the client and the core library, open logger

			Client client = null;
			try
			{
				client = new Client();
				client.Run();
			}
			catch (Exception e)
			{
				Console.WriteLine("Runtime Exception: " + e);
			}
			finally
			{
				client.Dispose();
			}

			// TODO: Core library shutdown if needed
		}

	}
}
