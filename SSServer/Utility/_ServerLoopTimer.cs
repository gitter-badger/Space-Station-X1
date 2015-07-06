using System;
using Timer = System.Timers.Timer;
using ARE = System.Threading.AutoResetEvent;

namespace SSCyg.Server.Utility
{
	// Maintains the timer and event handling for smooth updating
	internal class _ServerLoopTimer
	{
		private delegate void MainServerLoop();

		private Timer _timer;
		private ARE _are;

		public _ServerLoopTimer(MainServerLoop loop, uint period)
		{
			_timer = new Timer(period);
			_timer.Elapsed += (sender, e) => { loop(); };
			_are = new ARE(true);
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
