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
    using System.Reflection;
    using System.Diagnostics;
    using System.IO;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		internal Model.UserViewingHistory userViewingHistory = new Model.UserViewingHistory();
		private System.Timers.Timer searchEntryTimer = new System.Timers.Timer(1000);
		/// <summary>
		/// Store for the currently selected element, this is used when the view is updated
		/// from an external build so we can reselect the users selection.searchBox_SelectionChanged
		/// </summary>
		private Entry currentSelection;
		private Entry currentSelectionParent;
		private bool allowFileRefreshing = true;

        /// <summary>
        /// Checks the license and informs the user if there is an issue.
        /// </summary>
        /// <returns>True if the application can run else false.</returns>
        private bool CheckLicense()
        {
            string file = "livedocumenter.lic";
            Licensing.License license;

            if (!File.Exists(file))
            {
                MessageBox.Show(
                    string.Format("No license was located. Please add your license file '{0}' to the same directory as this executable and restart the application.\n\n", file),
                    "Live Documenter - License Issue"
                    );
                return false;
            }

            try
            {
                license = Licensing.License.Decrypt(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "There was an error reading your license file. Please make sure it is correct. If this issue continues please contact support@theboxsoftware.com\n\n",
                    "Live Documenter - License Issue"
                    );
                return false;
            }
            finally { }

            // validate the license.
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            Licensing.License.ValidationInfo info = license.Validate("ld-desktop", fvi.ProductVersion);
            if (info.HasExpired)
            {
                MessageBox.Show(
                    "Thank you for trying out our software. You can purchase a full copy from http://livedocumenter.com\n\n",
                    "Live Documenter - License Issue"
                    );
                return false;
            }
            if (!info.IsComponentValid)
            {
                MessageBox.Show(
                    "Your license does not cover this application. You can purchase a full copy from http://livedocumenter.com\n\n",
                    "Live Documenter - License Issue"
                    );
                return false;
            }
            if (info.IsVersionInvalid)
            {
                MessageBox.Show(
                    string.Format("Unfortuntely your license does not cover this version {0} of the software. Please upgrade or install an earlier version.\n\n",
                        fvi.ProductVersion
                        ),
                    "Live Documenter - License Issue"
                    );
                return false;
            }

            return true;
        }

		/// <summary>
		/// Initialises a new instance of the MainWindow class.
		/// </summary>
		public MainWindow() {
			InitializeComponent();

			Model.UserApplicationStore.Load();

			// restore the user preferences
			Size lastSize = Model.UserApplicationStore.Store.LastWindowSize;
			Point lastPosition = Model.UserApplicationStore.Store.LastWindowPosition;
			FlowDocumentReaderViewingMode mode = Model.UserApplicationStore.Store.ViewingMode;
			if (lastSize != null && !lastSize.IsEmpty && lastSize != new Size(0, 0)) {
				this.Height = lastSize.Height;
				this.Width = lastSize.Width;
			}
			if (lastPosition != null && !(lastPosition.X == 0 && lastPosition.Y == 0)) {
				this.Top = lastPosition.X;
				this.Left = lastPosition.Y;
			}
			this.pageViewer.ViewingMode = mode;
			if (Model.UserApplicationStore.Store.IsMaximized) {
				this.WindowState = System.Windows.WindowState.Maximized;
			}

			this.DataContext = this;
			this.forward.DataContext = this.back.DataContext = this.userViewingHistory;
			this.recentFiles.DataContext = Model.UserApplicationStore.Store.RecentFiles;

			this.searchEntryTimer.AutoReset = true;
			this.searchEntryTimer.Elapsed += new System.Timers.ElapsedEventHandler(PerformSearch);

			this.InitialiseStartScreen();

            if (!this.CheckLicense())
            {
                Application.Current.Shutdown();
            }

			string[] args = ((App)App.Current).Arguments;
			if (args != null && args.Length > 0) {
				// validate this is a file we are looking for
				if (System.IO.File.Exists(args[0])) {
					// we only allow ldproj files to open here
					if (System.IO.Path.GetExtension(args[0]) == ".ldproj") {
						LiveDocumentorFile.Load(args[0]);
						this.startpage.Visibility = System.Windows.Visibility.Hidden;
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
						this.startpage.Visibility = System.Windows.Visibility.Hidden;
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
					this.pageViewer.Focus(); // [#98] need to reset focus or commands are all greyed out
				}
				finally {
					this.Cursor = null;
                    GC.Collect();
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

            GC.Collect();
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
					LiveDocumentorFile.Singleton.UnerlyingProject.GetAssemblies().Count + LiveDocumentorFile.Singleton.UnerlyingProject.RemovedAssemblies.Count	> 1;
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
				ProjectManager manager = new ProjectManager();
				manager.Owner = this;
				bool? result = manager.ShowDialog();
				if (result.HasValue && result.Value) {
					SelectionStateManager state = new SelectionStateManager();
					state.Save(this.currentSelection);
					this.UpdateView();
					state.Restore();
				}
			}
			else if (e.Command == Commands.DocumentSettings) {
				Preferences p = new Preferences();
				p.Owner = this;

				bool? result = p.ShowDialog();

				if (result.HasValue && result.Value) {
					SelectionStateManager state = new SelectionStateManager();
					state.Save(this.currentSelection);
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
				GC.Collect();
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
			//throw new ArgumentException();
		}

		/// <summary>
		/// Make sure the state of the application is restored when the application is loaded.
		/// </summary>
		/// <param name="sender">Calling object</param>
		/// <param name="e">Event arguments</param>
		private void mainWindow_Loaded(object sender, RoutedEventArgs e) {
			this.Cursor = Cursors.AppStarting;

			this.recentFiles.DataContext = Model.UserApplicationStore.Store.RecentFiles;
			this.Cursor = null;
			this.Opacity = 1;
			this.Visibility = System.Windows.Visibility.Visible;
		}

		/// <summary>
		/// Make sure the state of the application is persisted and other tasks before application
		/// closes.
		/// </summary>
		/// <param name="sender">The calling object</param>
		/// <param name="e">The event arguments</param>
		private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			// store the users current selections
			Model.UserApplicationStore.Store.LastWindowSize = new Size(this.Width, this.Height);
			Model.UserApplicationStore.Store.LastWindowPosition = new Point(this.Top, this.Left);
			Model.UserApplicationStore.Store.ViewingMode = this.pageViewer.ViewingMode;
			Model.UserApplicationStore.Store.IsMaximized = this.WindowState == System.Windows.WindowState.Maximized;

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

                if (this.startpage.Visibility == Visibility.Visible) {
                    this.startpage.Visibility = Visibility.Hidden;
                    this.documentpage.Visibility = Visibility.Visible;
                }
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
			export = null;
			GC.Collect();
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
						results.Sort();
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

		/// <summary>
		/// Configures the start up screen.
		/// </summary>
		private void InitialiseStartScreen() {
			this.start_recentFileList.ItemsSource = Model.UserApplicationStore.Store.RecentFiles;

			if (Model.UserApplicationStore.Store.RecentFiles.Count == 0) {
				this.start_recentFiles.Visibility = Visibility.Hidden;
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
			private List<Entry> modifiedEntries = new List<Entry>();

			/// <summary>
			/// Saves the state.
			/// </summary>
			/// <param name="current">The users curretnly selected entry.</param>
			public void Save(Entry current) {
				if(current != null) {
					this.previousEntry = current;
					this.isExpanded = current.IsExpanded;
				}

				// recursively iterate through the entries and record each that has been 
				// changed
				this.RecordTreeState(LiveDocumentorFile.Singleton.LiveDocument.Map);
			}

			/// <summary>
			/// Restores the users state.
			/// </summary>
			public void Restore() {
				Entry selected = this.Find(this.previousEntry);
				List<Entry> otherChangedEntries = new List<Entry>();

				foreach(Entry current in this.modifiedEntries) {
					Entry found = this.Find(current);
					if(found != null) {
						found.IsExpanded = current.IsExpanded;
					}
				}

				if(selected != null) {
					selected.IsSelected = true;
					selected.IsExpanded = true;
					selected.IsExpanded = this.isExpanded;
				}
			}

			/// <summary>
			/// Finds the <paramref name="entry"/> in the new DocumentMap.
			/// </summary>
			/// <param name="entry"></param>
			/// <returns></returns>
			private Entry Find(Entry entry) {
				Entry found = null;

				if (entry != null) { // only if something was previously selected
					Entry parent = entry.Parent;

					if (entry.Item is Reflection.ReflectedMember) {
						Reflection.Comments.CRefPath path = Reflection.Comments.CRefPath.Create(
							entry.Item as Reflection.ReflectedMember
							);
						found = LiveDocumentorFile.Singleton.LiveDocument.Find(path);
					}
					else if (parent != null && parent.Item is Reflection.ReflectedMember) {
						Reflection.Comments.CRefPath path = Reflection.Comments.CRefPath.Create(
							parent.Item as Reflection.ReflectedMember
							);
						found = LiveDocumentorFile.Singleton.LiveDocument.Find(path);

						// test method/field member collection pages in a type
						for (int i = 0; i < found.Children.Count; i++) {
							if (found.Children[i].Name == entry.Name) {
								found = found.Children[i];
								break;
							}
						}
					}
					else {
						// now its a namespace or namespace collection page and we will ignore
						// it for now
						bool isNamespaceContainer = entry.Item is EntryTypes 
							&& ((EntryTypes)entry.Item) == EntryTypes.NamespaceContainer;
						List<Entry> childrenToSearch = new List<Entry>();

						// get children that can be searched for
						if(isNamespaceContainer) {
							for(int i = 0; i < entry.Children.Count; i++){
								childrenToSearch.AddRange(entry.Children[i].Children);
							}
						}
						else {
							childrenToSearch.AddRange(entry.Children);
						}

						// iterate over each item until one is found
						Entry foundChildItem = null;
						for(int i = 0; i < childrenToSearch.Count; i++) {
							Reflection.ReflectedMember item = childrenToSearch[i].Item as Reflection.ReflectedMember;
							if(item != null) {
								Reflection.Comments.CRefPath path = Reflection.Comments.CRefPath.Create(item);
								foundChildItem = LiveDocumentorFile.Singleton.LiveDocument.Find(path);
								break;
							}
						}

						if(foundChildItem != null) {
							if(isNamespaceContainer) {
								foundChildItem.Parent.Parent.IsExpanded = entry.IsExpanded;
							}
							else {
								foundChildItem.Parent.IsExpanded = entry.IsExpanded;
							}
						}
					}
				}

				return found;
			}

			private void RecordTreeState(IEnumerable<Entry> entries) {
				foreach(Entry current in entries) {
					if(current.IsExpanded) {
						modifiedEntries.Add(current);
						this.RecordTreeState(current.Children);
					}
				}
			}
		}
		#endregion

		private void Hyperlink_RequestNavigate(object sender, EventArgs e) {
			Button b = sender as Button;
			this.LoadRecentFile((Model.RecentFile)b.Tag);

			this.startpage.Visibility = Visibility.Hidden;
			this.documentpage.Visibility = Visibility.Visible;
		}

		private void start_recentFileList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (this.start_recentFileList.SelectedItem != null) {
				this.LoadRecentFile((Model.RecentFile)this.start_recentFileList.SelectedItem);

				this.startpage.Visibility = Visibility.Hidden;
				this.documentpage.Visibility = Visibility.Visible;
			}
		}
	}
}
