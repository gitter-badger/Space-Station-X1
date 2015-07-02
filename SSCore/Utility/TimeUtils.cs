using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCyg.Core.Utility
{
	// Helper functions and constants for time related actions
	public static class TimeUtils
	{
		#region Constants
		// ========= Time Stamp Formats =========
		// Short version of the date (6/15/2009).
		public static readonly string SHORT_DATE;
		// Long version of the date (Monday, June 15, 2009)
		public static readonly string LONG_DATE;
		// Full date and time (Monday, June 15, 2009 1:45:30 PM).
		public static readonly string FULL_DATE_TIME;
		// General date and time (6/15/2009 1:45:30 PM).
		public static readonly string GENERAL_DATE_TIME;
		// Short time (1:45 PM).
		public static readonly string SHORT_TIME;
		// Long time (1:45:30 PM).
		public static readonly string LONG_TIME;
		// Universal date and time (Monday, June 15, 2009 8:45:30 PM)
		public static readonly string UNI_TIME;
		// Local 12 hour time (1:45).
		public static readonly string LOCAL_12HR;
		// Local 24 hour time (13:45).
		public static readonly string LOCAL_24HR;
		#endregion

		#region Functions
		/// Returns the current time as a timestamp with the given format and the default culture.
		public static string GetTimeStamp(string format)
		{
			return DateTime.Now.ToString(format);
		}
		#endregion

		static TimeUtils()
		{
			SHORT_DATE = "d";
			LONG_DATE = "D";
			FULL_DATE_TIME = "F";
			GENERAL_DATE_TIME = "G";
			SHORT_TIME = "t";
			LONG_TIME = "T";
			UNI_TIME = "U";
			LOCAL_12HR = "H:mm:ss";
			LOCAL_24HR = "h:mm:ss";
		}
	}
}