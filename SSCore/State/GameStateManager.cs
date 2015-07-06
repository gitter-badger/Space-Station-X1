using System;
using System.Reflection;
using System.Linq;
using SSCyg.Core.Service;

namespace SSCyg.Core.State
{
	// Manages the active game state and automatically updates the state as needed.
	// These should be implemented in the client and server by extending this class,
	// and passing either ClientGameState or ServerGameState as the generic argument.
	public abstract class GameStateManager<T> : GameService
		where T : class, IGameState
	{
		#region Members
		private Type _typeQueued; // The type we are queued to change to
		private bool _changeQueued { get { return _typeQueued != null; } }

		// The currently active state
		public T ActiveState { get; private set; }
		// If there is an active state
		public bool HasState { get { return ActiveState != null; } }
		#endregion

		public GameStateManager()
		{
			_typeQueued = null;
			ActiveState = null;
		}

		#region State Management
		// Queue a new state to be used as the active state. The switch will be made at the end of
		// the draw command on the current frame.
		public void SwitchTo<U>()
			where U : T
		{
			Type type = typeof(U);

			if (ActiveState != null && ActiveState.GetType() == type)
				Debug.Throw(new ArgumentException("Cannot switch from a GameState to a new state of the same type."));

			_typeQueued = type;
		}

		private void doSwitch()
		{
			if (!_changeQueued)
				Debug.Throw(new Exception("Could not do the state switch. Invalid arguments for the next state."));

			Debug.BeginBlock("StateSwitch");

			string last = "None";
			if (ActiveState != null)
			{
				ActiveState.OnRemove();
				ActiveState.Dispose();
				last = ActiveState.Name;
			}

			ActiveState = null;

			ConstructorInfo cinfo = _typeQueued.GetConstructors().FirstOrDefault();
			if (cinfo == null)
				Debug.Throw(new NoPublicConstructorException(_typeQueued));
			ParameterInfo[] pinfo = cinfo.GetParameters();
			if (pinfo.Any())
				Debug.Throw(new NoValidConstructorException(_typeQueued, "void", "Subclasses of GameState must have a no-args public constructor."));

			Debug.BeginBlock("StateInitialize(" + _typeQueued + ")");
			T ns = (T)Activator.CreateInstance(_typeQueued);
			ns.Initialize();
			Debug.EndBlock(); // "StateInitialize(" + _typeQueued + ")"

			ActiveState = ns;

			Debug.LogSys("Changed the active GameState from " + last + " to " + ActiveState.Name + ".");
			_typeQueued = null;

			Debug.EndBlock(); // "StateSwitch"
		}
		#endregion

		#region Disposal
		public override void Dispose(bool disposing)
		{
			if (disposing && !IsDisposed)
			{
				if (HasState)
				{
					ActiveState.OnRemove();
					ActiveState.Dispose();
					ActiveState = null;
				}
			}

			base.Dispose(disposing);
		}
		#endregion
	}
}
