using System;
using System.Collections.Generic;
using SSCyg.Core;

namespace SSCyg.Client
{
	class Program
	{

		static void Main()
		{
			List<string> commandLine = new List<string>(Environment.GetCommandLineArgs());
			commandLine.RemoveAt(0); // Remove program name

			SSCore.Initialize(commandLine, false);

			Client client = null;
			try
			{
				client = new Client();
				client.Run();
			}
			catch (Exception e)
			{
				Debug.Log(LogLevel.Fatal, "A runtime exception was caught and was unhandled.");
				Debug.LogException(e);
				bool ok = Debug.ShowErrorBox("Unhandled Exception", "\"" + e.Message + "\"\n" + e.StackTrace.Replace("\n", "\n\t"));
				Debug.Log("User pressed " + (ok ? "OK" : "CANCEL") + " for the error window.");
			}
			finally
			{
				client.Dispose();
			}

			SSCore.Shutdown();
		}

	}
}
