using System;

namespace SSCyg.Core.Service
{
	// Base interface for GameServices, that are treated like singletons, but are managed
	// through GameServerManager, and therefore more predictable.
	public interface IGameService : IDisposable
	{
		// If the service has been disposed
		bool IsDisposed { get; }

		// Implements the disposal functionality, disposing is true when the disposal is 
		// user induced, or false otherwise
		void Dispose(bool disposing);
	}
}
