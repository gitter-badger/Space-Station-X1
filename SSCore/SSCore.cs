using System;
using System.Collections.Generic;

namespace SSCyg.Core
{
	// Class that initializes and shuts down all of the core library components.
	public static class SSCore
	{
		// If the core library has been initialized.
		private static bool initialized = false;

		#region Members
		// Provided command line arguments
		private static List<string> commandLineArgs;
		public static string[] CommandLineArguments
		{
			get { return commandLineArgs.ToArray(); }
		}

		// If the code is currently running on the server
		public static bool IsServer { get; private set; }
		#endregion

		// Initializes the core components
		public static void Initialize(List<string> commandLine, bool isServer)
		{
			if (initialized)
				Debug.Throw(new InvalidOperationException("Cannot initialize the core library twice."));
			if (commandLine == null)
				Debug.Throw(new ArgumentNullException("commandLine", "Command line argument list cannot be null."));

			commandLineArgs = commandLine;
			IsServer = isServer;

			List<string> unknown = new List<string>();
			bool useFile = true, useThread = true;
			for (int i = 0; i < commandLine.Count; ++i)
			{
				string command = commandLine[i];
				if (!command.StartsWith("-"))
					continue;

				switch (command)
				{
					case "-nlf":
					case "--no-log-file":
						{
							useFile = false;
						}
						break;
					case "-nlt":
					case "--no-log-thread":
						{
							useThread = false;
						}
						break;
					case "-npf":
					case "--no-profile-file":
						{
							Debug.UseProfilerFileDump = false;
						}
						break;
					default:
						unknown.Add(command);
						break;
				}
			}

			Debug.OpenLogger(useFile, useThread);
			Debug.Log("Logger is open.");
			if (unknown.Count > 0)
			{
				string log = "There were " + unknown.Count + " unknown command line arguments.";
				foreach (string command in unknown)
					log += ("\n\t" + command);
				Debug.LogWarning(log);
			}

			initialized = true;
		}

		// Shuts down all of the core components
		public static void Shutdown()
		{
			if (!initialized)
				Debug.Throw(new InvalidOperationException("Cannot shutdown the core library more than once."));

			if (Debug.UseProfilerFileDump)
				Debug.DumpProfilerToFile();

			Debug.CloseLogger();

			initialized = false;
		}
	}
}
