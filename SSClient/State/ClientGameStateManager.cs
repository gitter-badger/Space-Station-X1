using System;
using SSCyg.Core.State;

namespace SSCyg.Client.State
{
	// Client side manager for game states
	public sealed class ClientGameStateManager : GameStateManager<ClientGameState>
	{
		// Update the current client game state
		public void Update(TimeSpan delta)
		{
			if (ActiveState != null)
				ActiveState.DoUpdate(delta);
		}

		// Draw the current client game state
		public void Draw(TimeSpan delta)
		{
			if (ActiveState != null)
				ActiveState.DoDraw(delta);
		}
	}
}