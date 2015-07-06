using System;

namespace SSCyg.Core.State
{
	// This is how functionality is customized without making a massive loop in the main class.
	// There can only be one state active at a time, and that state is automatically updated every
	// frame of the game. It can run on both the client and the server, and the manager is smart
	// enough to not call the draw functions on the server-side states. To implement Client states,
	// extend the ClientGameState class in SSCyg.Client.State, and extend the ServerGameState class
	// in SSCyg.Server.State for server states.
	public interface IGameState : IDisposable
	{
		// The simple name of the scene. Should generally be the class name minus the "State" at the end
		string Name { get; }
		// If this scene is disposed and is no longer valid.
		bool IsDisposed { get; }
		// If the scene has been initialized yet.
		bool IsInitialized { get; }

		// Should implement the initialization functionality
		void Initialize();
		// Should implement a single run the client/server specific update functionality
		void DoUpdate(TimeSpan delta);
		// Should implement functionality for when the state stops being the active state
		void OnRemove();
		// Implements the functionality of Dispose(), disposing == true if user disposal, false otherwise
		void Dispose(bool disposing);
	}
}
