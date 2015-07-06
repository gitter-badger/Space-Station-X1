using System;
using LoopTimer = SSCyg.Server.Utility._ServerLoopTimer;

namespace SSCyg.Server
{
	public class Server
	{
		#region Members
		// If the server is active
		public bool IsActive { get; private set; }

		// The timer that fires the main loop
		private LoopTimer _loopTimer;
		#endregion

		// Initializes all of the server services
		public Server()
		{
			IsActive = false;
		}

		// Loads in the server settings and opens the server for connections
		public void Start()
		{
			IsActive = true;
		}

		#region Server Loop
		// Launches the server into its main loop, but is not the main loop itself
		public void MainLoop()
		{
			// Try to run the loop every 1 msec
			_loopTimer = new LoopTimer(() => { RunLoop(); }, 1);

			// Wait for the event to be signaled and try to run the loop
			while (IsActive)
			{
				_loopTimer.WaitOne();

				mainLoopStuff();
			}
		}

		// Sets the event to cause the loop to run
		public void RunLoop()
		{
			_loopTimer.Set();
		}
		
		// The actual main loop
		private void mainLoopStuff()
		{

		}
		#endregion
	}
}
