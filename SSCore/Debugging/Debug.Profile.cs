using System;
using SSCyg.Core.Debugging;
using SSCyg.Core.Utility;
using System.IO;

namespace SSCyg.Core
{
	// This file implements the profiling portion of the debug class
	public static partial class Debug
	{
		#region Members
		// If the profiler will make a dump to a file when the application exits.
		public static bool UseProfilerFileDump { get; internal set; }
		// The total number of frames that has run in this game.
		public static int TotalFrames { get; private set; }

		private static ProfileBlock rootBlock;
		private static ProfileBlock currentBlock;
		#endregion

		#region Timing Functions
		// Begins timing the block with the given name.
		public static void BeginBlock(string name)
		{
			currentBlock = currentBlock.GetChild(name);
			currentBlock.Begin();
		}

		// Ends the current active profiling block.
		public static void EndBlock()
		{
			if (rootBlock != currentBlock)
			{
				currentBlock.End();
				currentBlock = currentBlock.Parent;
			}
		}

		internal static void BeginFrame()
		{
			EndFrame();
			BeginBlock("BaseFrame");
		}
		internal static void EndFrame()
		{
			if (currentBlock != rootBlock)
			{
				EndBlock();
				++TotalFrames;
				if (TotalFrames == 0)
					++TotalFrames;
				rootBlock.EndFrame();
				currentBlock = rootBlock;
			}
		}
		#endregion

		internal static void DumpProfilerToFile()
		{
			using (FileStream fs = File.Open(SSCore.IsServer ? "perfdumpserver.log" : "perfdumpclient.log", FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (StreamWriter writer = new StreamWriter(fs))
				{
					string header = "Profiler Dump File Created " + TimeUtils.GetTimeStamp("H:mm:ss tt zz");
					writer.WriteLine(header);
					writer.WriteLine("".PadLeft(header.Length, '='));
					writer.Write(GetProfilerString());
				}
			}
		}

		public static string GetProfilerString()
		{
			return "TODO: Debug.GetProfilerString()";
		}
	}
}
