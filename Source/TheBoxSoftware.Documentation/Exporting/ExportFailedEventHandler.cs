using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting {
	/// <summary>
	/// Event that describes an expected non-terminal failure during an export run.
	/// </summary>
	/// <param name="e">The arguments that describe the failure.</param>
	public delegate void ExportFailedEventHandler(ExportFailedEventArgs e);
}
