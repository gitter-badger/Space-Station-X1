using System;
using System.Diagnostics;
using System.Threading;

namespace SSCyg.Core.Utility
{
	// Timer that uses Stopwatch to get resolutions higher than 15 ms, which is the default
	// resolution for the built in timer class. If Stopwatch.IsHighResolution is true, then
	// the resolution of this timer is ~500 ns.
	public class HighResTimer : IDisposable
	{
		// If the underlying timer is high resolution
		public static readonly bool IsHighResolution = Stopwatch.IsHighResolution;
		// Callback function for elapsed
		public delegate void HighResTimerCallback();

		#region Members
		// If the requested time may not be accurately tracked, i.e. < 0.1 ms
		// TODO: Testing to get an accurate value on this
		public readonly bool IsAccurate;
		// The callback for when the timer is elapsed
		public event HighResTimerCallback Elapsed;
		// The time between events being fired, in milliseconds
		public float TargetTime { get; private set; }
		// If the timer is counting
		public bool Enabled;

		// The thread that the timer is running on
		private Thread _thread;
		// If the thread should be running
		private bool _enabled;
		// The Stopwatch used to track time
		private Stopwatch _stopwatch;
		#endregion

		// Create a new high res timer with the given timespan to time
		public HighResTimer(float milliseconds)
		{
			TargetTime = milliseconds;
			Enabled = false;
			IsAccurate = (IsHighResolution ? (milliseconds < 0.1f) : (milliseconds < 15.2f));

			_thread = new Thread(thread_func);
			_stopwatch = new Stopwatch();
			_thread.Start();
		}
		~HighResTimer()
		{
			Dispose();
		}

		private void thread_func()
		{
			_stopwatch.Start();
			_enabled = true;
			while (_enabled)
			{
				// Elapsed time in microseconds, and milliseconds
				float elapsedTime = (_stopwatch.ElapsedTicks / (float)Stopwatch.Frequency);
				float elapsedMillis = elapsedTime * 1000.0f;
				if (elapsedMillis >= TargetTime && Enabled)
				{
					callback();
					_stopwatch.Restart();
				}
			}
		}

		#region Callbacks
		private void callback()
		{
			if (Elapsed != null)
				Elapsed();
		}
		#endregion

		#region Disposal
		public void Dispose()
		{
			if (_enabled)
			{
				_enabled = false;
				_thread.Join();
			}
		}
		#endregion
	}
}
