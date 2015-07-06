using System;
using SSCyg.Core;
using SSCyg.Core.State;

namespace SSCyg.Client.State
{
	// Base class for game states on the client side
	public abstract class ClientGameState : GameState
	{
		protected ClientGameState()
		{

		}

		public override void DoUpdate(TimeSpan delta)
		{
			PreUpdate(delta);
			Update(delta);
		}

		public void DoDraw(TimeSpan delta)
		{
			PreDraw(delta);
			Draw(delta);
		}

		protected virtual void PreUpdate(TimeSpan delta) { }
		protected abstract void Update(TimeSpan delta);
		protected virtual void PreDraw(TimeSpan delta) { }
		protected abstract void Draw(TimeSpan delta);

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