using System;
using SSCyg.Core;
using SSCyg.Core.State;

namespace SSCyg.Server.State
{
	// Base class for game states on the server side
	public abstract class ServerGameState : GameState
	{
		protected ServerGameState()
		{

		}

		public sealed override void DoUpdate(TimeSpan delta)
		{
			PreUpdate(delta);
			Update(delta);
		}

		protected virtual void PreUpdate(TimeSpan delta) { }
		protected abstract void Update(TimeSpan delta);

		public override void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{
				if (disposing)
				{

				}
			}

			base.Dispose(disposing);
		}
	}
}
