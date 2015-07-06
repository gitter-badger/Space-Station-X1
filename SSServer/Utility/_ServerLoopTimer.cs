using System;
using Timer = SSCyg.Core.Utility.HighResTimer;
using AutoResetEvent = System.Threading.AutoResetEvent;

namespace SSCyg.Server.Utility
{
	// Maintains the timer and event handling for smooth updating
	internal class _ServerLoopTimer
	{
		public delegate void MainServerLoop();

		private Timer _timer;
		private AutoResetEvent _are;

		public _ServerLoopTimer(float period)
		{
			_timer = new Timer(period);
			_timer.Elapsed += () => { Set(); };
			_are = new AutoResetEvent(true);
			_timer.Enabled = true;
		}

		// Sets the AutoResetEvent
		public void Set()
		{
			_are.Set();
		}

		// Waits the AutoResetEvent
		public void WaitOne()
		{
			_are.WaitOne(-1);
		}
	}
}
