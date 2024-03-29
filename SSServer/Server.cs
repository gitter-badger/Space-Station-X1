﻿using System;
using System.Collections.Generic;
using System.Linq;
using Stopwatch = System.Diagnostics.Stopwatch;
using SSCyg.Core;
using LoopTimer = SSCyg.Server.Utility._ServerLoopTimer;
using SSCyg.Core.Utility;
using SSCyg.Core.Service;
using SSCyg.Server.State;

namespace SSCyg.Server
{
	public class Server : IDisposable
	{
		// Instance of the server
		private static Server _theServer = null;
		public static Server TheServer
		{
			get
			{
				if (_theServer == null)
					Debug.Throw(new NullReferenceException("Server singleton has not been initialized."));
				return _theServer;
			}
		}

		#region Members
		// If the server is active
		public bool IsActive { get; private set; }
		// The number of frames per second
		public float TickRate { get; private set; }
		// The number of milliseconds per tick
		public float ServerRate { get; private set; }
		// The total uptime of the server
		public float ServerClock { get; private set; }
		// The time at the beginning of the last update
		public DateTime Time { get; private set; }

		// The timer that fires the main loop
		private LoopTimer _loopTimer;
		// Tracks the elapsed time to see if the server should update
		private Stopwatch _stopwatch = new Stopwatch();
		// The list of the last second of frame times
		private readonly List<float> _frameTimes = new List<float>();
		// Last time the console title was updated
		private DateTime _lastConsoleUpdate;
		// Convinience reference to the state manager
		private ServerGameStateManager _stateManager;
		#endregion

		// Initializes all of the server services
		public Server()
		{
			Debug.BeginBlock("ServerConstructor");
			_theServer = this;

			IsActive = false;

			_lastConsoleUpdate = DateTime.Now;

			// TODO: Eventually load this from a settings file
			TickRate = 20.0f;
			ServerRate = 1000.0f / TickRate;

			// Log timing warning
			if (!HighResTimer.IsHighResolution)
				Debug.LogWarning("Your system does not support high resolution timing. The server may run at a lower TPS than anticipated.");

			Debug.EndBlock();
		}
		~Server()
		{
			dispose(false);
		}

		// Loads in the server settings and opens the server for connections
		public void Start()
		{
			Debug.BeginBlock("ServerStart");

			_stateManager = GameServiceManager.Resolve<ServerGameStateManager>();
			// TODO: Starting stuff

			IsActive = true;
			Debug.EndBlock();
		}

		#region Server Loop
		// Launches the server into its main loop, but is not the main loop itself
		public void MainLoop()
		{
			// Try to run the loop every 1 msec
			_loopTimer = new LoopTimer(1.0f);
			_stopwatch.Start();

			// Wait for the event to be signaled and try to run the loop
			while (IsActive)
			{
				_loopTimer.WaitOne();

				mainLoopStuff();
			}
		}

		// The actual main loop
		private void mainLoopStuff()
		{
			Debug.BeginBlock("ServerMainLoop");

			float elapsedTime, elapsedMilliseconds;
			elapsedTime = (_stopwatch.ElapsedTicks / (float)Stopwatch.Frequency);
			elapsedMilliseconds = elapsedTime * 1000;

			// Return out if we are not ready for another update
			if (elapsedMilliseconds < ServerRate && (ServerRate - elapsedMilliseconds) >= 0.5f)
			{
				Debug.EndBlock(); // "ServerMainLoop"
				return;
			}

			Debug.BeginFrame();
			Debug.BeginBlock("ConsoleUpdate");
			_stopwatch.Restart();

			ServerClock += elapsedTime;

			// Update the server time
			Time = DateTime.Now;
			if (_frameTimes.Count >= TickRate)
				_frameTimes.RemoveAt(0);
			float rate = 1.0f / elapsedTime;
			_frameTimes.Add(rate);

			// Update the console title if its been at least a second
			if ((Time - _lastConsoleUpdate).TotalMilliseconds >= 1000)
			{
				double tps = Math.Round(averageFrameTime(), 2);
				long mem = Debug.GetMemoryUsedKB();
				Console.Title = String.Format("TPS: {0:N2} | Net: (TODO) | Memory: {1:N0} KiB", tps, mem);
				_lastConsoleUpdate = DateTime.Now;
			}
			Debug.EndBlock(); // "ConsoleUpdate"

			Debug.BeginBlock("BaseUpdate");

			_stateManager.Update(TimeSpan.FromMilliseconds(elapsedMilliseconds));

			Debug.EndBlock(); // "BaseUpdate"

			Debug.EndFrame();
			Debug.EndBlock(); // "ServerMainLoop"
		}
		#endregion

		#region Stats functions
		private float averageFrameTime()
		{
			if (_frameTimes.Count == 0)
				return 0.0f;

			return _frameTimes.Average(p => p);
		}
		#endregion

		#region Disposal
		public void Dispose()
		{
			dispose(true);
		}

		private void dispose(bool disposing)
		{
			if (_theServer == null)
				return; // Already disposed

			// TODO: Disposal stuff

			_theServer = null;
		}
		#endregion
	}
}
