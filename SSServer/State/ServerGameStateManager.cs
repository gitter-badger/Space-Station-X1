using System;
using SSCyg.Core.State;

namespace SSCyg.Server.State
{
	// Server side manager for game states
	public sealed class ServerGameStateManager : GameStateManager<ServerGameState>
	{
		// Update the current server game state
		public void Update(TimeSpan delta)
		{
			if (ActiveState != null)
				ActiveState.DoUpdate(delta);
		}
	}
}
