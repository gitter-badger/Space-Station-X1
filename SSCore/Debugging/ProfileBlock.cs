using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SSCyg.Core.Debugging
{
	// Contains a single profiling block instance for the profiler. Keeps a list of child
	// blocks (from the tree structure), and it's profile information will always contain
	// at least the sum of the children's information.
	public sealed class ProfileBlock
	{
		private static int count = 0; // For giving names to unnamed blocks

		#region Members
		// Name of the profile block.
		public string Name { get; private set; }
		// Time in the current frame.
		public TimeSpan Time { get; private set; }
		// Maximum time on the current frame.
		public TimeSpan MaxTime { get; private set; }
		// Number of times this block has executed in this frame.
		public int Count { get; private set; }
		// Time in the last frame.
		public TimeSpan LastTime { get; private set; }
		// Maximum time in the last frame.
		public TimeSpan LastMaxTime { get; private set; }
		// Number of times this block executed in the last frame.
		public int LastCount { get; private set; }
		// Total accumulated time.
		public TimeSpan TotalTime { get; private set; }
		// Total maximum time.
		public TimeSpan TotalMaxTime { get; private set; }
		// Total accumulated execution times.
		public int TotalCount { get; private set; }
		// The parent block for this one. Null for root block.
		public ProfileBlock Parent { get; private set; }
		// The children blocks to this block.
		public List<ProfileBlock> Children { get; private set; }

		private Stopwatch _timer; // Internal timer for getting real time information
		#endregion

		#region Functions
		internal ProfileBlock(ProfileBlock p, string name)
		{
			_timer = new Stopwatch();
			Parent = p;
			Children = new List<ProfileBlock>();
			Time = MaxTime = LastTime = LastMaxTime = TotalTime = TotalMaxTime = TimeSpan.Zero;
			Count = LastCount = TotalCount = 0;

			int num = count++;
			if (String.IsNullOrEmpty(name))
				Name = "ProfileBlock" + num;
			else
				Name = name;
		}

		internal void Begin()
		{
			_timer.Restart();
			++Count;
		}
		internal void End()
		{
			TimeSpan t = _timer.Elapsed;
			if (t > MaxTime)
				MaxTime = t;
			Time += t;
		}

		internal void EndFrame()
		{
			LastTime = Time;
			LastMaxTime = MaxTime;
			LastCount = Count;
			TotalTime += Time;
			if (MaxTime > TotalMaxTime)
				TotalMaxTime = MaxTime;
			TotalCount += Count;
			Time = TimeSpan.Zero;
			MaxTime = TimeSpan.Zero;
			Count = 0;

			foreach (ProfileBlock p in Children)
				p.EndFrame();
		}

		// Gets a child block with the given name, or creates a new child if none exists.
		public ProfileBlock GetChild(string name)
		{
			foreach (ProfileBlock p in Children)
				if (p.Name == name)
					return p;

			ProfileBlock np = new ProfileBlock(this, name);
			Children.Add(np);
			return np;
		}
		#endregion
	}
}
