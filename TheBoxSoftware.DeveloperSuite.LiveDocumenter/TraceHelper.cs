using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	/// <summary>
	/// 
	/// </summary>
	public static class TraceHelper {
		/// <summary>
		/// Indicates if Traceing has been switched on for this application.
		/// </summary>
		public static bool IsTraceEnabled { get; set; }

		public static void Indent() {
			System.Diagnostics.Trace.Indent();
		}

		public static void Unindent() {
			System.Diagnostics.Trace.Unindent();
		}

		public static void WriteLine(object value) {
			System.Diagnostics.Trace.WriteLine(value);
		}

		public static void WriteLine(string message) {
			System.Diagnostics.Trace.WriteLine(message);
		}

		public static void WriteLine(string format, object arg1) {
			System.Diagnostics.Trace.WriteLine(string.Format(format, arg1));
		}

		public static void WriteLine(string format, object arg1, object arg2) {
			System.Diagnostics.Trace.WriteLine(string.Format(format, arg1, arg2));
		}

		public static void WriteLine(string format, object arg1, object arg2, object arg3) {
			System.Diagnostics.Trace.WriteLine(string.Format(format, arg1, arg2, arg3));
		}

		public static void WriteLineIf(bool condition, object value) {
			System.Diagnostics.Trace.WriteLineIf(TraceHelper.IsTraceEnabled && condition, value);
		}

		public static void WriteLineIf(bool condition, string message) {
			System.Diagnostics.Trace.WriteLineIf(TraceHelper.IsTraceEnabled && condition, message);
		}

		public static void WriteLineIf(bool condition, string format, object arg1) {
			System.Diagnostics.Trace.WriteLineIf(condition, string.Format(format, arg1));
		}

		public static void WriteLineIf(bool condition, string format, object arg1, object arg2) {
			System.Diagnostics.Trace.WriteLineIf(condition, string.Format(format, arg1, arg2));
		}

		public static void WriteLineIf(bool condition, string format, object arg1, object arg2, object arg3) {
			System.Diagnostics.Trace.WriteLineIf(condition, string.Format(format, arg1, arg2, arg3));
		}
	}
}
