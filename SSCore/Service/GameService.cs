using System;

namespace SSCyg.Core.Service
{
	// Implementation of the basic functionality of IGameService, for classes that do not require
	// a different base class and can therefore turn here for ease of implementation
	public abstract class GameService : IGameService
	{
		public bool IsDisposed { get; private set; }

		protected GameService()
		{
			IsDisposed = false;
		}
		~GameService()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Remember to always call base.Dispose(disposing) in child classes
		public virtual void Dispose(bool disposing)
		{
			IsDisposed = true;
		}
	}
}
