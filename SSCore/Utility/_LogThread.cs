using System;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using SSCyg.Core.Utility;

namespace SSCyg.Core
{
	// Maintains the logging thread. Messages can be posted to the log thread, and they will be flushed after
	// a set amount of time has passed. This minimizes the number of IO calls, as well as places IO calls into
	// a separate thread. Both actions will greatly increase the performance of the Logger and make it 
	// non-blocking. If threaded logging is not requested, then this class just passes the string directly into
	// the file.
	internal static class _LogThread
	{
		#region Members
		// If the thread should be closing. Call Close() to actually set this value
		public static bool ShouldClose { get; private set; }
		// If the thread is currently running
		public static bool IsRunning { get; private set; }
		// If threading is being used
		public static bool IsThreaded { get; private set; }
		// The name being used for the log file
		public static string LogFileName { get; private set; }

		// The thread-safe queue for messages
		private static ConcurrentQueue<string> messageQueue;
		// The stream writer to the file
		private static StreamWriter streamWriter;
		// The thread
		private static Thread logThread;
		#endregion

		// Opens the thread and log file for printing messages to
		// logName: name of the file to log to (needs extension)
		// threaded: if the log is threaded or should just go directly to the file
		public static void OpenThread(string logName, bool threaded)
		{
			if (IsRunning)
				return;

			if (logName == null)
				throw new ArgumentNullException("logName", "The name for the log file cannot be null.");
			if (String.IsNullOrEmpty(logName))
				throw new ArgumentException("The log file name cannot be empty.", "logName");

			ShouldClose = false;
			IsThreaded = threaded;
			LogFileName = logName;

			try // Initial check for log file validity
			{
				streamWriter = new StreamWriter(File.OpenWrite(LogFileName));
				streamWriter.AutoFlush = false;
				string stamp = TimeUtils.GetTimeStamp(TimeUtils.GENERAL_DATE_TIME);
				streamWriter.WriteLine(stamp);
				streamWriter.WriteLine("".PadLeft(stamp.Length, '='));
				streamWriter.WriteLine("\n");
				streamWriter.Flush();
			}
			catch (Exception e)
			{
				throw new ArgumentException("The provided log file threw an exception upon opening.", e);
			}

			if (IsThreaded) // If it is threaded, more to do
			{
				// These lines allow the streamWriter to be recycled to the log thread for proper ownership
				streamWriter.Close();
				streamWriter.Dispose();
				streamWriter = null;

				messageQueue = new ConcurrentQueue<string>();
				logThread = new Thread(thread_func);
				logThread.Start();
			}

			IsRunning = true;
		}

		// Informs the thread that it should flush remaining messages and close
		public static void Close()
		{
			if (!IsRunning)
				return;

			if (IsThreaded)
			{
				ShouldClose = true;
				logThread.Join(); // Join the thread and wait for it to close
			}
			else
			{
				streamWriter.Flush();
				streamWriter.BaseStream.Close();
				streamWriter.Close();
				streamWriter.Dispose();
			}

			IsRunning = false;
		}

		// Adds a new string to the log
		public static void AddString(string str)
		{
			if (IsThreaded)
				messageQueue.Enqueue(str);
			else
			{
				streamWriter.WriteLine(str);
				streamWriter.Flush();
			}
		}

		#region Threaded Stuff
		// The function that runs in the thread
		private static void thread_func()
		{
			streamWriter = new StreamWriter(File.OpenWrite(LogFileName)); // Checking if this file exists and can be opened is already done in OpenThread()
			streamWriter.AutoFlush = false;

		func_start:
			float currentTime = 0.0f;
			while (currentTime < 1.0f)
			{
				Thread.Sleep(125); // Sleep for 125 milliseconds
				if (ShouldClose)
				{
					do_flush();
					goto func_end;
				}
				currentTime += 0.125f;
			}

			do_flush();
			goto func_start;

		func_end:
			streamWriter.Close();
			streamWriter.Dispose();
		}

		// Actually performs the write to the file. Will only dequeue the number of messages available when this
		// method starts. If more messages are added as this is flushing, they will be included in the next flush.
		private static void do_flush()
		{
			int len = messageQueue.Count;
			for (int i = 0; i < len; ++i)
			{
				string value = null;
				if (!messageQueue.TryDequeue(out value))
					break; // TODO: this is the result of an error, maybe do something about this in the future
				else
					streamWriter.WriteLine(value);
			}
			if (len > 0)
				streamWriter.Flush();
		}
		#endregion
	}
}
