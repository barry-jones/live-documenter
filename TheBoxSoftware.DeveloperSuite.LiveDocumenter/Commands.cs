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

		/// <summary>
		/// Command for adding new files to the LiveDocumenterFile
		/// </summary>
		public static readonly RoutedUICommand Add = new RoutedUICommand("Add", "add", typeof(MainWindow));

		/// <summary>
		/// Command for removing files from the LiveDocumenterFile
		/// </summary>
		public static readonly RoutedUICommand Remove = new RoutedUICommand("Remove", "remove", typeof(MainWindow));

		/// <summary>
		/// Command for viewing/opening the document settings dialogue
		/// </summary>
		public static readonly RoutedUICommand DocumentSettings = new RoutedUICommand("DocumentSettings", "documentsettings", typeof(MainWindow));
	}
}
