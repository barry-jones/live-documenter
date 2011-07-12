using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting {
	/// <summary>
	/// Exception that indicates an issue with the export process.
	/// </summary>
	public class ExportException : Exception {
		/// <summary>
		/// Initializes a new instance of the <see cref="ExportException"/> class.
		/// </summary>
		public ExportException() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExportException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public ExportException(string message)
			: base(message) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExportException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public ExportException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
