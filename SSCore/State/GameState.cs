using System;

namespace SSCyg.Core.State
{
	// Implements common functionality of the IGameState interface, and can be used by classes
	// for easier implementation if they do not require a different base class.
	public abstract class GameState : IGameState
	{
		#region Members
		public abstract string Name { get; }

		public bool IsDisposed { get; private set; }

		private bool _isInitialized;
		public bool IsInitialized
		{
			get
			{
				if (IsDisposed)
					Debug.Throw(new ObjectDisposedException("GameState"));
				return _isInitialized;
			}
		}
		#endregion

		public GameState()
		{
			IsDisposed = false;
			_isInitialized = false;
		}
		~GameState()
		{
			Dispose(false);
		}

		public abstract void Initialize();
		public abstract void DoUpdate(TimeSpan delta);
		public abstract void OnRemove();

		#region Disposal
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Dispose(bool disposing)
		{
			if (disposing)
			{

			}

			IsDisposed = true;
		}
		#endregion
	}
}
