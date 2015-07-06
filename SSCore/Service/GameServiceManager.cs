using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace SSCyg.Core.Service
{
	// Manages the initialization, storage, and disposal of GameServices.
	// Use this class to access the GameServices from anywhere in the code.
	public static class GameServiceManager
	{
		// The game service instances
		private static readonly Dictionary<Type, IGameService> registeredServices = new Dictionary<Type, IGameService>();

		// Attempt to get the instance of the provided type. If the provided type exists, then
		// the instance is returned, otherwise the type is created using the default no-args
		// constructor. This new type is managed by the ServiceManager and is returned.
		public static T Resolve<T>()
			where T : IGameService
		{
			Type type = typeof(T);
			if (!registeredServices.ContainsKey(type))
			{
				ConstructorInfo cinfo = type.GetConstructors().FirstOrDefault(); // Try to get a public no-args constructor
				if (cinfo == null)
					Debug.Throw(new NoPublicConstructorException(type));

				ParameterInfo[] pinfo = cinfo.GetParameters();
				if (pinfo.Any()) // Check that the type has no parameters
					Debug.Throw(new NoValidConstructorException(type, "void", "GameService classes must have an empty public constructor."));

				IGameService inst = Activator.CreateInstance<T>();
				registeredServices.Add(type, inst);
			}

			return (T)registeredServices[type];
		}

		// Check to see if the GameService with the provided type currently has an active
		// instance being managed by the ServiceManager.
		public static bool HasService<T>()
			where T : IGameService
		{
			return registeredServices.ContainsKey(typeof(T));
		}

		// Unregisters a service from the ServiceManager. Will stop managing that instance and will
		// dispose of the instance. Use this carefully when not in the shutdown phase, as any other
		// code that references a specific service may lose that usable instance if it is unregistered.
		public static bool UnregisterService<T>()
			where T : IGameService
		{
			Type type = typeof(T);
			if (!registeredServices.ContainsKey(type))
				return false;

			T inst = (T)registeredServices[type];
			registeredServices.Remove(type);
			inst.Dispose();
			return true;
		}

		// Unregisters and disposes of all managed services
		public static void UnregisterAll()
		{
			foreach (IGameService service in registeredServices.Values)
				service.Dispose();

			registeredServices.Clear();
		}
	}

	// Exception that gets called when there is no invokable public constructors for a type.
	public sealed class NoPublicConstructorException : Exception
	{
		// The type that the exception was called for.
		public readonly Type Type;

		public override string Message
		{
			get
			{
				return "The type " + Type + " does not have any publicly visible constructors.";
			}
		}

		public NoPublicConstructorException(Type t)
		{
			Type = t;
		}
	}

	// Exception for types that do not have a public constructor with the required argument list.
	public sealed class NoValidConstructorException : Exception
	{
		// The type that the exception was called for.
		public readonly Type Type;
		// The list of argument types that was expected.
		public readonly string Arguments;

		public override string Message
		{
			get
			{
				string s = "The type " + Type + " does not have a constructor with the proper argument list: (" + Arguments + ").";
				if (base.Message.Length > 0)
					s += " Message: \"" + base.Message + "\".";
				return s;
			}
		}

		public NoValidConstructorException(Type t, string args, string message = "")
			: base(message)
		{
			Type = t;
			Arguments = args;
		}
	}
}
