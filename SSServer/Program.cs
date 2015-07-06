using System;
using System.Collections.Generic;
using SSCyg.Core;

namespace SSCyg.Server
{
	class Program
	{

		public static void Main(string[] args)
		{
			List<string> commandLine = new List<string>(Environment.GetCommandLineArgs());
			commandLine.RemoveAt(0); // Remove program name

			SSCore.Initialize(commandLine, true);

			Server server = null;
			try
			{
				server = new Server();
				server.Start();

				server.MainLoop();
			}
			catch (Exception e)
			{
				Debug.Log(LogLevel.Fatal, "A runtime exception was caught and was unhandled.");
				Debug.LogException(e);
			}
			finally
			{
				server.Dispose();
			}

			SSCore.Shutdown();
		}

	}
}
