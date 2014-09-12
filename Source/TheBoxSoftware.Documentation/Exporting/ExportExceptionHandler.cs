using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting {
	/// <summary>
	/// Event handler which can be used to consume ExportException events from the
	/// <see cref="Exporter"/> and derived classes.
	/// </summary>
	public delegate void ExportExceptionHandler(object sender, ExportExceptionEventArgs e);
}
