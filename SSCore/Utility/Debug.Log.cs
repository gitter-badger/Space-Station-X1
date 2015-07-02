using System;
using System.IO;
using System.Text;
using SSCyg.Core.Utility;

namespace SSCyg.Core
{
	// Used to categorize the relative importance of the logged message.
	public enum LogLevel :
		byte
	{
		// A non-important information message originating from inside of the library. User code should avoid using this level,
		// as it may cause confusion with engine messages.
		System = 0,
		// An information message coming from outside the library. Use this in the user code for non-important bits of information.
		Info = 1,
		// A message about an important event or possible error that does not noticably affect the application.
		Warning = 2,
		// A message about an error that affects for the application runs or plays, but can be / is recovered from.
		Error = 3,
		// A message about an error that cannot be recovered from, and stops either a large portion or the entire game.
		Fatal = 4
	}

	// Delegate for subscribing to the event that Debug fires whenever there is a new message logged.
	// lvl: The log level of the message.
	// timeStamp: The time stamp on the message.
	// message: The content of the message.
	public delegate void MessageLoggedHandler(LogLevel lvl, string timeStamp, string message);

	// This file implements the logging functionality for the Debug class
	public static partial class Debug
	{
		#region Members
		// If the debug is outputting logger lines to a log file.
		public static bool IsUsingFileLogging { get; private set; }
		// If the debug is outputting logger lines to the console.
		public static bool IsUsingConsoleLogging { get; private set; }
		// If the logger is threaded.
		public static bool IsUsingThreadedLogging { get { return _LogThread.IsThreaded; } }
		// If the logger is open.
		public static bool IsLoggerOpen { get; private set; }

		// The text for each of the logging levels
		private static readonly string[] levelText = { "System", "Info", "Warn", "ERROR", "FATAL" };
		// The event for messages logged
		private static event MessageLoggedHandler messageLogged;
		#endregion

		#region Logging Control Functions
		// Internal function used by AEApplication to open the logger. This will detect and open the console and
		// file logging as needed.
		// useConsole: if there should be console logging, note that explicit console creation is only supported on windows platforms, the 
		//			*nix platforms always have piping and terminals available, if needed
		// useFile: if there should be file logging
		// isThreaded: if the file logger should be multithreaded
		internal static void OpenLogger(bool useConsole, bool useFile, bool isThreaded)
		{
			if (IsLoggerOpen)
				return;

			IsUsingConsoleLogging = useConsole;
			IsUsingFileLogging = useFile;

			if (useFile)
			{
				_LogThread.OpenThread("SSX1Client.log", isThreaded);
			}

			IsLoggerOpen = true;
		}

		// Internal function used by AEApplication to close the logger. Also called automatically when the application
		// shuts down as a safety check against crashes.
		internal static void CloseLogger()
		{
			if (!IsLoggerOpen)
				return;

			Debug.LogSys("Closing down logger.");

			if (IsUsingFileLogging)
				_LogThread.Close(); // This will hang for at most 1/8 sec if threading is being used

			IsLoggerOpen = false;
		}
		#endregion

		#region Log Functions
		// Logs a message with LogLevel.Info.
		// str: The message to log.
		public static void Log(string str)
		{
			Log(LogLevel.Info, str);
		}

		// Logs a formatted message with LogLevel.Info.
		// str: The formatting string.
		// args: The additional objects to format.
		public static void Log(string str, params object[] args)
		{
			Log(LogLevel.Info, String.Format(str, args));
		}

		// Logs a message with a given level.
		// lvl: The LogLevel for the message.
		// str: The content of the message.
		public static void Log(LogLevel lvl, string str)
		{
			string ll = "[" + levelText[(int)lvl] + "]";
			string ts = "[" + TimeUtils.GetTimeStamp(TimeUtils.LOCAL_24HR) + "]";

			if (messageLogged != null)
				messageLogged(lvl, ts, str);

			if (IsLoggerOpen)
			{
				string log = ll + ts + ":\t" + str;
				if (IsUsingConsoleLogging)
					Console.WriteLine(log);
				if (IsUsingFileLogging)
					_LogThread.AddString(log);
			}
		}

		// Logs a formatted message with a given level.
		// lvl: The LogLevel for the message.
		// str: The content of the message.
		// args: The additional objects to format.
		public static void Log(LogLevel lvl, string str, params object[] args)
		{
			Log(lvl, String.Format(str, args));
		}

		// Shortcut to easily log a message with LogLevel.Warning.
		// s: The message to log.
		public static void LogWarning(string s) { Log(LogLevel.Warning, s); }
		// Shortcut to easily log a formatted message with LogLevel.Warning.
		// s: The message to log.
		// args: The additional objects to format.
		public static void LogWarning(string s, params object[] args) { Log(LogLevel.Warning, String.Format(s, args)); }
		// Shortcut to easily log a message with LogLevel.Error.
		// s: The message to log.
		public static void LogError(string s) { Log(LogLevel.Error, s); }
		// Shortcut to easily log a formatted message with LogLevel.Error.
		// s: The message to log.
		// args:The additional objects to format.
		public static void LogError(string s, params object[] args) { Log(LogLevel.Error, String.Format(s, args)); }

		// Convinience functions for this library
		internal static void LogSys(string s) { Log(LogLevel.System, s); }
		internal static void LogSys(string s, params object[] args) { Log(LogLevel.System, String.Format(s, args)); }

		// Provides a way to log information about an Exception in a neat and formatted fashion.
		// e: The exception to log.
		public static void LogException(Exception e)
		{
			string ts = "[" + TimeUtils.GetTimeStamp(TimeUtils.LOCAL_24HR) + "]";
			string mes =
				"\tType: \"" + e.GetType() + "\"\n" +
				"\tMessage: \"" + e.Message + "\"\n" +
				"\tStack Trace:\n" +
				"\t" + e.StackTrace.Replace("\n", "\n\t\t");
			string log = "[EXCEPTION]" + ts + ":\n" + mes;

			if (messageLogged != null)
				messageLogged(LogLevel.System, ts, mes);

			if (IsLoggerOpen)
			{
				if (IsUsingConsoleLogging)
					Console.WriteLine(log);
				if (IsUsingFileLogging)
					_LogThread.AddString(log);
			}
		}
		#endregion
	}
}
