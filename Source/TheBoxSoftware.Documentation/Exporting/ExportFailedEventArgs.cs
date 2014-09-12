using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting {
	/// <summary>
	/// Details the failure of an an export run.
	/// </summary>
	/// <seealso cref="ExportFailedEventHandler"/>
	public class ExportFailedEventArgs : EventArgs {
		/// <summary>
		/// Initialises a new instance of the ExportFailedEvnetArgs class.
		/// </summary>
		/// <param name="message">The message describing the failure.</param>
		public ExportFailedEventArgs(string message) {
		}

		/// <summary>
		/// The message detailing the failure
		/// </summary>
		public string Message { get; set; }
	}
}
