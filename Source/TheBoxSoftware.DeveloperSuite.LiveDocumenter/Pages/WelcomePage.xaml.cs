using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages {
	/// <summary>
	/// Interaction logic for WelcomePage.xaml
	/// </summary>
	public partial class WelcomePage : Page {
        /// <summary>
        /// Constructor
        /// </summary>
		public WelcomePage() {
			InitializeComponent();

			// Add recent projects list
			Model.RecentFileList fileList = Model.UserApplicationStore.Store.RecentFiles;
			if (fileList.Count > 0) {
				foreach (Model.RecentFile file in fileList) {
					ListItem item = new ListItem();
					Paragraph p = new Paragraph();
					item.Blocks.Add(p);

					// Build up a hyperlink
					Hyperlink project = new Hyperlink();
					project.Click += new RoutedEventHandler(RecentProjectLoader);
					project.Inlines.Add(new Run(file.DisplayName));
					project.DataContext = file;
					p.Inlines.Add(project);

					this.recentProjectList.ListItems.Add(item);
				}
			}
			else {
				// Hide that information from display
				this.Blocks.Remove(this.pRecentProjects);
				this.Blocks.Remove(this.recentProjectList);
			}
		}

		/// <summary>
		/// Loads the selected project in to the main application.
		/// </summary>
		/// <param name="sender">The calling object</param>
		/// <param name="e">Event arguments</param>
		void RecentProjectLoader(object sender, RoutedEventArgs e) {
			Hyperlink link = (Hyperlink)sender;

			((MainWindow)((FlowDocumentReader)this.Parent).FindName("mainWindow")).LoadRecentFile(
				(Model.RecentFile)link.DataContext
				);
		}
	}
}
