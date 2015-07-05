using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace SSCyg.Core
{
	// Static class that contains information about the runtime state of the application, as well
	// as functions for logging information to the console and to a file. Logging can be multithreaded
	// for performance gains.
	public static partial class Debug
	{
		#region Message Boxes
		// These return true if the user pressed OK, false if the user pressed Cancel
		// Pops up an information message box with the given title and message. Will suspend the application.
		public static bool ShowMessageBox(string title, string message)
		{
			DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
			return result == DialogResult.OK;
		}

		// Pops up a warning message box with the given title and message. Will suspend the application.
		public static bool ShowWarningBox(string title, string message)
		{
			DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
			return result == DialogResult.OK;
		}

		// Pops up an error message box with the given title and message. Will suspend the application.
		public static bool ShowErrorBox(string title, string message)
		{
			DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
			return result == DialogResult.OK;
		}
		#endregion

		#region Exception
		// Provides a way to log an exception and throw it in one fell swoop.
		public static void Throw(Exception e)
		{
			Debug.LogException(e);
			throw e;
		}
		#endregion

		#region Memory
		// Returns the current memory use for the program in kilobytes.
		public static long GetMemoryUsedKB()
		{
			return Process.GetCurrentProcess().PrivateMemorySize64 >> 10;
		}

		// Returns the current memory use in megabytes.
		public static double GetMemoryUsedMB()
		{
			return GetMemoryUsedKB() / 1024.0;
		}
		#endregion
	}
}
