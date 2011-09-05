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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Documentation;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public Model.UserViewingHistory userViewingHistory = new Model.UserViewingHistory();
		private System.Timers.Timer searchEntryTimer = new System.Timers.Timer(1000);
		/// <summary>
		/// Store for the currently selected element, this is used when the view is updated
		/// from an external build so we can reselect the users selection.searchBox_SelectionChanged
		/// </summary>
		private Entry currentSelection;
		private Entry currentSelectionParent;
		private bool allowFileRefreshing = true;

		/// <summary>
		/// Initialises a new instance of the MainWindow class.
		/// </summary>
		public MainWindow() {
			InitializeComponent();
			this.DataContext = this;
			this.forward.DataContext = this.back.DataContext = this.userViewingHistory;
			this.recentFiles.DataContext = Model.UserApplicationStore.Store.RecentFiles;
			this.removeAssemblies.DataContext = LiveDocumentorFile.Singleton;

			this.searchEntryTimer.AutoReset = true;
			this.searchEntryTimer.Elapsed += new System.Timers.ElapsedEventHandler(PerformSearch);

			string[] args = ((App)App.Current).Arguments;
			if (args != null && args.Length > 0) {
				// validate this is a file we are looking for
				if (System.IO.File.Exists(args[0])) {
					// we only allow ldproj files to open here
					if (System.IO.Path.GetExtension(args[0]) == ".ldproj") {
						LiveDocumentorFile.Load(args[0]);
						this.UpdateView();
					}
				}
			}
		}

		#region Commands
		private void OpenDocumentationFile() {
			System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
			string[] filters = new string[] {
				"All Files (.ldproj, .sln, .csproj, .vbproj, .vcproj, .dll, .exe)|*.ldproj;*.sln;*.csproj;*.vbproj;*.vcproj;*.dll;*.exe",
				"Live Documenter Project (.ldproj)|*.ldproj",
				"VS.NET Solution (.sln)|*.sln",
				"All VS Project Files (.csproj, .vbproj, .vcproj)|*.csproj;*.vbproj;*.vcproj",
				".NET Libraries and Executables (.dll, .exe)|*.dll;*.exe"
				};
			ofd.Filter = string.Join("|", filters);
			ofd.AutoUpgradeEnabled = true;
			ofd.Multiselect = false;
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				this.Cursor = Cursors.Wait;
				try {
					if(!string.IsNullOrEmpty(ofd.FileName)) {
						if(System.IO.Path.GetExtension(ofd.FileName) == ".ldproj") {
							LiveDocumentorFile.Load(ofd.FileName);
						}
						else {
							LiveDocumentorFile.Singleton.Open(ofd.FileName);
						}

						this.UpdateView();

						this.userViewingHistory.ClearHistory();
						if(ofd.FileNames.Length == 1) { // only add histories for named files
							Model.UserApplicationStore.Store.RecentFiles.AddFile(new TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model.RecentFile(
								ofd.FileName, System.IO.Path.GetFileName(ofd.FileName)
								));
						}
					}
				}
				catch (TheBoxSoftware.Reflection.Core.NotAManagedLibraryException) {
					LiveDocumentorFile.Singleton.Clear();	// Clear it again, we already did that before loading
					MessageBox.Show(
						string.Format(ResourcesExceptionText.NOT_A_MANAGED_LIBRARY, ofd.FileName),
						"Unsupported File Type",
						MessageBoxButton.OK,
						MessageBoxImage.Information
						);
					this.pageViewer.Document = new Pages.WelcomePage();
					this.pageViewer.Focus(); // [#98] need to reset focus or commands are all greyed out
				}
				finally {
					this.Cursor = null;
				}
			}
		}

		private void AddDocumentationFile() {
			System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
			string[] filters = new string[] {
				"All Files (.sln, .csproj, .vbproj, .vcproj, .dll, .exe)|*.sln;*.csproj;*.vbproj;*.vcproj;*.dll;*.exe",
				"VS.NET Solution (.sln)|*.sln",
				"All VS Project Files (.csproj, .vbproj, .vcproj)|*.csproj;*.vbproj;*.vcproj",
				".NET Libraries and Executables (.dll, .exe)|*.dll;*.exe"
				};
			ofd.Filter = string.Join("|", filters);
			ofd.AutoUpgradeEnabled = true;
			ofd.Multiselect = true;
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				this.Cursor = Cursors.Wait;

				try {
					if(ofd.FileNames != null && ofd.FileNames.Length > 0) {
						LiveDocumentorFile.Singleton.Add(ofd.FileNames);

						this.UpdateView();
					}
				}
				catch (TheBoxSoftware.Reflection.Core.NotAManagedLibraryException) {
					LiveDocumentorFile.Singleton.Clear();	// Clear it again, we already did that before loading
					MessageBox.Show(
						string.Format(ResourcesExceptionText.NOT_A_MANAGED_LIBRARY, ofd.FileName),
						"Unsupported File Type",
						MessageBoxButton.OK,
						MessageBoxImage.Information
						);
					this.pageViewer.Document = new Pages.WelcomePage();
					this.pageViewer.Focus(); // [#98] need to reset focus or commands are all greyed out
				}
				finally {
					this.Cursor = null;
				}
			}
		}

		private void Save(bool saveAs) {
			this.Cursor = Cursors.Wait;
			try {
				if(!saveAs && !string.IsNullOrEmpty(LiveDocumentorFile.Singleton.Filename)) {
					// save the changes to the existing file
					LiveDocumentorFile.Singleton.Save();
				}
				else {
					System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
					sfd.AddExtension = true;
					sfd.AutoUpgradeEnabled = true;
					sfd.CreatePrompt = false;
					sfd.DefaultExt = "ldproj";
					sfd.Filter = "Live Documenter Project (.ldproj)|*.ldproj";
					sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
					if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
						if(!string.IsNullOrEmpty(sfd.FileName)) {
							LiveDocumentorFile.Singleton.SavaAs(sfd.FileName);
						}
					}
				}
			}
			finally {
				this.Cursor = null;
			}
		}

		/// <summary>
		/// Remove the assembly with the <paramref name="assemblyUniqueId"/> from
		/// the document and update.
		/// </summary>
		/// <param name="assemblyUniqueId">The unqiue id of the assembly.</param>
		private void RemoveAssembly(long assemblyUniqueId) {
			SelectionStateManager state = new SelectionStateManager();
			state.Save(this.currentSelection);

			LiveDocumentorFile.Singleton.Remove(assemblyUniqueId);
			this.UpdateView();

			state.Restore();
		}

		/// <summary>
		/// Loads up the recent file
		/// </summary>
		/// <param name="file">The recent file information to load.</param>
		internal void LoadRecentFile(Model.RecentFile file) {
			if (System.IO.File.Exists(file.Filename)) {
				this.Cursor = Cursors.Wait;
				
				if(System.IO.Path.GetExtension(file.Filename) == ".ldproj") {
					LiveDocumentorFile.Load(file.Filename);
				}
				else {
					LiveDocumentorFile.Singleton.Open(file.Filename);
				}

				Model.UserApplicationStore.Store.RecentFiles.AddFile(file);
				this.UpdateView();
				this.userViewingHistory.ClearHistory();

				this.Cursor = null;
			}
			else {
				MessageBox.Show(string.Format(
					"The {0} does not exist at the specified location.", System.IO.Path.GetFileName(file.Filename)),
					"File Not Found"
					);
			}
		}

		/// <summary>
		/// Command handler. Checks if commands can be executed by the application when the user or application checks
		/// the state of the application.
		/// </summary>
		/// <param name="sender">The calling object</param>
		/// <param name="e">Event arguments</param>
		public void CanExecuteCommand(object sender, CanExecuteRoutedEventArgs e) {
			if (e.Command == NavigationCommands.BrowseForward || e.Command == NavigationCommands.BrowseBack) {
				this.userViewingHistory.CanExecuteCommand(sender, e);
			}
			else if (e.Command == ApplicationCommands.Open || e.Command == ApplicationCommands.Close) {
				e.CanExecute = true;
			}
			else if (e.Command == ApplicationCommands.SaveAs) {
				e.CanExecute = LiveDocumentorFile.Singleton.LiveDocument != null &&
					LiveDocumentorFile.Singleton.LiveDocument.HasFiles;
			}
			else if (e.Command == ApplicationCommands.Save) {
				e.CanExecute = LiveDocumentorFile.Singleton.LiveDocument != null &&
					LiveDocumentorFile.Singleton.LiveDocument.HasFiles &&
					LiveDocumentorFile.Singleton.HasChanged;
			}
			else if (e.Command == ApplicationCommands.Print) {
				e.CanExecute = LiveDocumentorFile.Singleton.LiveDocument != null &&
					LiveDocumentorFile.Singleton.LiveDocument.HasFiles;
			}
			else if (e.Command == ApplicationCommands.Find) {
				e.CanExecute = LiveDocumentorFile.Singleton.LiveDocument != null &&
					LiveDocumentorFile.Singleton.LiveDocument.HasFiles;
			}
			else if (e.Command == Commands.Export) {
				e.CanExecute = LiveDocumentorFile.Singleton.LiveDocument != null && 
					LiveDocumentorFile.Singleton.LiveDocument.HasFiles;
			}
			else if (e.Command == Commands.Add) {
				e.CanExecute = LiveDocumentorFile.Singleton.LiveDocument != null;
			}
			else if (e.Command == Commands.Remove) {
				e.CanExecute = LiveDocumentorFile.Singleton.LiveDocument != null &&
					LiveDocumentorFile.Singleton.LiveDocument.HasFiles && 
					LiveDocumentorFile.Singleton.LiveDocument.Assemblies.Count > 1;
			}
			else if (e.Command == Commands.DocumentSettings) {
				e.CanExecute = LiveDocumentorFile.Singleton.LiveDocument != null &&
					LiveDocumentorFile.Singleton.LiveDocument.HasFiles;
			}
		}

		/// <summary>
		/// Executes commands actioned by the user of the appliction which are implemented by the
		/// interface.
		/// </summary>
		/// <param name="sender">Calling object</param>
		/// <param name="e">Event arguments</param>
		public void ExecuteCommand(object sender, ExecutedRoutedEventArgs e) {
			if (e.Command == NavigationCommands.BrowseForward || e.Command == NavigationCommands.BrowseBack) {
				this.userViewingHistory.ExecuteCommand(sender, e);
			}
			else if (e.Command == ApplicationCommands.Open) {
				this.OpenDocumentationFile();
			}
			else if (e.Command == Commands.Add) {
				this.AddDocumentationFile();
			}
			else if (e.Command == ApplicationCommands.Close) {
				this.CloseApplication(sender, e);
			}
			else if (e.Command == ApplicationCommands.Print) {
				this.Button_Click(sender, e);
			}
			else if (e.Command == ApplicationCommands.Find) {
				this.searchBox.Focus();
			}
			else if (e.Command == ApplicationCommands.Save) {
				this.Save(false);
			}
			else if(e.Command == ApplicationCommands.SaveAs) {
				this.Save(true);
			}
			else if (e.Command == Commands.Export) {
				this.exportClick(sender, e);
			}
			else if (e.Command == Commands.Remove) {
				this.RemoveAssembly((long)e.Parameter);
			}
			else if (e.Command == Commands.DocumentSettings) {
				Preferences p = new Preferences();
				p.Owner = this;
				SelectionStateManager state = new SelectionStateManager();
				state.Save(this.currentSelection);

				bool? result = p.ShowDialog();

				if (result.HasValue && result.Value) {
					this.UpdateView();
					state.Restore();
				}
			}
		}
		#endregion

		/// <summary>
		/// Updates the view of the live document. This is just the document map in the
		/// tree view.
		/// </summary>
		public void UpdateView() {
			LiveDocument document = LiveDocumentorFile.Singleton.Update();
			this.documentMap.ItemsSource = document.Map;
			this.removeAssemblies.DataContext = null;
			this.removeAssemblies.DataContext = LiveDocumentorFile.Singleton;

			if (document.Map.Count > 0) {
				this.pageViewer.Document = ((LiveDocumenterEntry)document.Map[0]).Page;
			}
			else {
                // There are no entries in this project/solution
                this.documentMap.ItemsSource = new Entry[] {
                    new EmptyEntry("There are no libraries.")
                };
				this.pageViewer.Document = new Pages.Page();
			}
		}

		/// <summary>
		/// Action handler. Handles the user selecting a new page to view.
		/// </summary>
		/// <param name="sender">Calling object</param>
		/// <param name="e">Event arguments</param>
        private void documentMap_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            if (e.NewValue != null && !(e.NewValue is EmptyEntry) && e.NewValue is LiveDocumenterEntry) {
                this.Cursor = Cursors.Wait;
				TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Page page = ((LiveDocumenterEntry)e.NewValue).Page;
                this.pageViewer.Document = page;
                this.userViewingHistory.Add((Entry)e.NewValue);
                this.currentSelection = (Entry)e.NewValue;
                this.currentSelectionParent = this.currentSelection.Parent != null ? this.currentSelection.Parent : this.currentSelection;
                while (this.currentSelectionParent == null || this.currentSelectionParent.Parent != null) {
                    this.currentSelectionParent = this.currentSelectionParent.Parent;
                };
                this.Cursor = null;
            }
        }

		private void Button_Click(object sender, RoutedEventArgs e) {
			this.pageViewer.Print();
		}

		private void MoveForward(object sender, RoutedEventArgs e) {
			this.userViewingHistory.MoveForward();
		}

		private void MoveBackward(object sender, RoutedEventArgs e) {
			this.userViewingHistory.MoveBack();
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e) {
			//// test
			//Documentation.Exporting.WebsiteExporter exp = new TheBoxSoftware.Documentation.Exporting.WebsiteExporter(
			//    LiveDocumentorFile.Singleton.Files,
			//    new TheBoxSoftware.Documentation.Exporting.ExportSettings(),
			//    Documentation.Exporting.ExportConfigFile.Create(@"ApplicationData/web-msdn.ldec")
			//    );
			//exp.Export();
		}

		/// <summary>
		/// Make sure the state of the application is restored when the application is loaded.
		/// </summary>
		/// <param name="sender">Calling object</param>
		/// <param name="e">Event arguments</param>
		private void mainWindow_Loaded(object sender, RoutedEventArgs e) {
			this.Cursor = Cursors.AppStarting;
			Model.UserApplicationStore.Load();
			this.recentFiles.DataContext = Model.UserApplicationStore.Store.RecentFiles;
            this.pageViewer.Document = new Pages.WelcomePage();
			this.Cursor = null;
		}

		/// <summary>
		/// Make sure the state of the application is persisted and other tasks before application
		/// closes.
		/// </summary>
		/// <param name="sender">The calling object</param>
		/// <param name="e">The event arguments</param>
		private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			Model.UserApplicationStore.Save();
			App.Current.Shutdown();
		}

		/// <summary>
		/// The user has actioned the close application menu item.
		/// </summary>
		/// <param name="sender">The caller.</param>
		/// <param name="e">The event arguments.</param>
		private void CloseApplication(object sender, RoutedEventArgs e) {
			App.Current.Shutdown();
		}

		/// <summary>
		/// Event handler for all recent file menu items, handled by the parent menu item. When we recieve
		/// this event we need to load the selected recent file for the user.
		/// </summary>
		/// <param name="sender">The calling object</param>
		/// <param name="e">The event arguments.</param>
		private void recentFiles_Click(object sender, RoutedEventArgs e) {
			MenuItem s = (MenuItem)e.OriginalSource;

            if (s == this.recentFiles)
                return; // causes exceptions when we process top level with no children

			if (s.DataContext != null) {
				Model.RecentFile f = (Model.RecentFile)s.DataContext;
				this.LoadRecentFile(f);
			}
		}

		private void mainWindow_Activated(object sender, EventArgs e) {
			if (this.AllowFileRefreshing && 
				LiveDocumentorFile.Singleton != null && 
				LiveDocumentorFile.Singleton.LiveDocument != null && 
				LiveDocumentorFile.Singleton.LiveDocument.Assemblies != null) {

				this.Cursor = Cursors.Wait;
				SelectionStateManager state = new SelectionStateManager();
				state.Save(this.currentSelection);
				bool hasBeenReloaded = false;

				foreach (DocumentedAssembly current in LiveDocumentorFile.Singleton.LiveDocument.Assemblies) {
					if (current.HasAssemblyBeenModified()) {
						hasBeenReloaded = true;

						LiveDocumentorFile.Singleton.LiveDocument.RefreshAssembly(current);
					}
				}

				// update the view if we are refreshing, especially since libraries may have been newly
				// compiled.
				if (hasBeenReloaded) {
					this.UpdateView();
					state.Restore();
				}

				this.Cursor = null;
			}
		}

		private void ShowAbout(object sender, RoutedEventArgs e) {
			About about = new About();
			about.Owner = this;
			about.ShowDialog();
		}

		/// <summary>
		/// Shows the export dialog.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void exportClick(object sender, RoutedEventArgs e) {
			Export export = new Export();
			export.Owner = this;
			export.ShowDialog();
		}

		/// <summary>
		/// Starts the time of for the search box. After a period of time has elapsed
		/// without the user entering more keys the search will complete.
		/// </summary>
		/// <param name="sender">The calling object</param>
		/// <param name="e">Event arguments</param>
		private void searchBox_Populating(object sender, PopulatingEventArgs e) {
			AutoCompleteBox textBox = e.Source as AutoCompleteBox;
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
			if (LiveDocumentorFile.Singleton.LiveDocument != null &&
					LiveDocumentorFile.Singleton.LiveDocument.HasFiles) {
				this.Dispatcher.Invoke((System.Threading.ThreadStart)delegate() {
					if (string.IsNullOrEmpty(this.searchBox.Text)) {
						this.searchBox.PopulateComplete();
					}
					else {
						Model.SearchResultCollection results = new Model.SearchResultCollection();
						results.AddEntriesToResults(LiveDocumentorFile.Singleton.LiveDocument.Search(this.searchBox.Text));
						this.searchBox.ItemsSource = results;
						this.searchBox.PopulateComplete();
					}
					this.searchEntryTimer.Stop();
				});
			}
		}

		/// <summary>
		/// Fired when the timer elapses. Allows us to reduce the amount of searches
		/// performed by the search box.
		/// </summary>
		/// <param name="sender">Calling object</param>
		/// <param name="e">Event arguments</param>
		/// <remarks>
		/// Will unselect and unexpand the current user selection and select and expand
		/// the new item.
		/// </remarks>
		private void searchBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (this.searchBox.SelectedItem != null) {
				if (this.documentMap.SelectedItem != null) {
					Entry entry = (Entry)this.documentMap.SelectedItem;
					do {
						entry.IsExpanded = false;
						entry = entry.Parent;
					}
					while (entry != null);
				}
				((Model.SearchResult)this.searchBox.SelectedItem).RelatedEntry.IsSelected = true;
				((Model.SearchResult)this.searchBox.SelectedItem).RelatedEntry.IsExpanded = true;

				System.Diagnostics.Debug.WriteLine(((Model.SearchResult)this.searchBox.SelectedItem).RelatedEntry.Item);
				System.Diagnostics.Debug.WriteLine(new System.Diagnostics.StackTrace());
				this.searchBox.SelectedItem = null;
			}
			e.Handled = true;
		}

		/// <summary>
		/// Handles all of the input binding and command bindings at the window level
		/// </summary>
		/// <param name="sender">Calling object</param>
		/// <param name="e">Event arguments</param>
		private void mainWindow_PreviewKeyDown(object sender, KeyEventArgs e) {
			// make sure that all our input bindings are taken care of, this fixes
			// an issue where the ctrl+f binding is ignored in some parts of the app.
			foreach (InputBinding inputBinding in this.InputBindings) {
				KeyGesture keyGesture = inputBinding.Gesture as KeyGesture;
				if (keyGesture != null && keyGesture.Key == e.Key && keyGesture.Modifiers == e.KeyboardDevice.Modifiers) {
					if (inputBinding.Command != null) {
						inputBinding.Command.Execute(0);
						e.Handled = true;
					}
				}
			}
		}

		#region Properties
		/// <summary>
		/// Informs the window if it is allowed to refresh the file list.
		/// </summary>
		internal bool AllowFileRefreshing {
			get { return this.allowFileRefreshing; }
			set { this.allowFileRefreshing = value; }
		}

		protected List<DocumentedAssembly> Assemblies {
			get { 

				return LiveDocumentorFile.Singleton.LiveDocument.Assemblies; 
			}
		}
		#endregion

		#region Internals
		/// <summary>
		/// Manages the current user selection state in the UI.
		/// </summary>
		/// <remarks>
		/// This allows the current entry to be recorded and restored around
		/// updates to the document map.
		/// <para>
		/// This does nor currently work with namespace and namespace container
		/// entries.
		/// </para>
		/// </remarks>
		private class SelectionStateManager {
			private Entry previousEntry;
			private bool isExpanded;

			/// <summary>
			/// Saves the state.
			/// </summary>
			/// <param name="current">The users curretnly selected entry.</param>
			public void Save(Entry current) {
				if(current != null) {
					this.previousEntry = current;
					this.isExpanded = current.IsExpanded;
				}
			}

			/// <summary>
			/// Restores the users state.
			/// </summary>
			public void Restore() {
				Entry found = null;
				Entry parent = this.previousEntry.Parent;
				
				if(this.previousEntry.Item is Reflection.ReflectedMember) {
					Reflection.Comments.CRefPath path = Reflection.Comments.CRefPath.Create(
						this.previousEntry.Item as Reflection.ReflectedMember
						);
					found = LiveDocumentorFile.Singleton.LiveDocument.Find(path);
				}
				else if(parent != null && parent.Item is Reflection.ReflectedMember) {
					Reflection.Comments.CRefPath path = Reflection.Comments.CRefPath.Create(
						parent.Item as Reflection.ReflectedMember
						);
					found = LiveDocumentorFile.Singleton.LiveDocument.Find(path);

					// test method/field member collection pages in a type
					for(int i = 0; i < found.Children.Count; i++) {
						if(found.Children[i].Name == this.previousEntry.Name) {
							found = found.Children[i];
							break;
						}
					}
				}
				else {
					// now its a namespace or namespace collection page and we will ignore
					// it for now
				}

				if(found != null) {
					found.IsSelected = true;
					found.IsExpanded = true;
					found.IsExpanded = this.isExpanded;
				}
			}
		}
		#endregion
	}
}
