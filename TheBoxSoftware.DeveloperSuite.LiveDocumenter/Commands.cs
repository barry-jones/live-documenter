using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	/// <summary>
	/// Application commands used to correctly manage activities in the user interface.
	/// </summary>
	internal static class Commands {
		/// <summary>
		/// Command for exporting documentation in the <see cref="MainWindow"/>.
		/// </summary>
		public static readonly RoutedUICommand Export = new RoutedUICommand("Export", "export", typeof(MainWindow));
	}
}
