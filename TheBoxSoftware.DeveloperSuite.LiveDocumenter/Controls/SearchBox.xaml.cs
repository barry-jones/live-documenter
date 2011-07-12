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

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Controls {
	/// <summary>
	/// Interaction logic for SearchBox.xaml
	/// </summary>
	public partial class SearchBox :UserControl {
		private System.Timers.Timer searchEntryTimer = new System.Timers.Timer(1000);

		public SearchBox() {
			InitializeComponent();

			this.searchEntryTimer.AutoReset = true;
			this.searchEntryTimer.Elapsed += new System.Timers.ElapsedEventHandler(PerformSearch);
		}

		/// <summary>
		/// Handles the Text Changed event for the search box.
		/// </summary>
		/// <param name="sender">Calling object</param>
		/// <param name="e">Event arguments</param>
		private void Search_TextChanged(object sender, TextChangedEventArgs e) {
			TextBox textBox = e.Source as TextBox;
			if (textBox != null) {
				this.searchEntryTimer.Start();
			}
		}

		/// <summary>
		/// Performs a search based on the users entered search text, or clears the
		/// search results if the search box has been emptied.
		/// </summary>
		/// <param name="sender">Calling timer.</param>
		/// <param name="e">The event arguments.</param>
		/// <remarks>
		/// This event is fired after the user has not entered another character in
		/// the search field for a determined amount of time. This is to reduce
		/// unecessary searches.
		/// </remarks>
		private void PerformSearch(object sender, System.Timers.ElapsedEventArgs e) {
			this.Dispatcher.Invoke((System.Threading.ThreadStart)delegate() {
				if (string.IsNullOrEmpty(this.searchBox.Text)) {
					this.searchResultsPopup.IsOpen = false;
				}
				else {
					Model.SearchResultCollection results = new Model.SearchResultCollection();
					results.AddEntriesToResults(LiveDocumentorFile.Singleton.LiveDocument.Search(this.searchBox.Text));
					this.searchResults.ItemsSource = results;
					this.searchResultsPopup.IsOpen = results.Count > 0;
				}
				this.searchEntryTimer.Stop();
			});
		}

		public TextBox InternalTextBox {
			get { return this.searchBox; }
		}

		private void searchResults_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			Model.SearchResult result = (Model.SearchResult)this.searchResults.SelectedItem;
			result.RelatedEntry.IsSelected = true;
			result.RelatedEntry.IsExpanded = true;
		}
	}
}
