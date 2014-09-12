using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting {
	/// <summary>
	/// Arguments for <see cref="ExportExceptionHandler"/> events occurring during
	/// the export process.
	/// </summary>
	public sealed class ExportExceptionEventArgs : EventArgs {
		/// <summary>
		/// Initializes a new instance of the <see cref="ExportExceptionEventArgs"/> class.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public ExportExceptionEventArgs(ExportException exception) {
			this.Exception = exception;
		}

		/// <summary>
		/// The exception which occurred during the export process.
		/// </summary>
		/// <value>The exception.</value>
		public ExportException Exception { get; private set; }
	}
}
