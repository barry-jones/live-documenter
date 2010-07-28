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

		/// <summary>
		/// Initialises a new instance of the MainWindow class.
		/// </summary>
		public MainWindow() {
			InitializeComponent();
			this.forward.DataContext = this.back.DataContext = this.userViewingHistory;
			this.recentFiles.DataContext = Model.UserApplicationStore.Store.RecentFiles;

			this.searchEntryTimer.AutoReset = true;
			this.searchEntryTimer.Elapsed += new System.Timers.ElapsedEventHandler(PerformSearch);
		}

		#region Menu Actions
		/// <summary>
		/// Adds a documentation file to the current live document.
		/// </summary>
		/// <param name="sender">Calling object.</param>
		/// <param name="e">Event arguments</param>
		private void AddDocumentationFile(object sender, RoutedEventArgs e) {
			System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
			string[] filters = new string[] {
				"All Files (.sln, .csproj, .vbproj, .vcproj, .dll, .exe)|*.sln;*.csproj;*.vbproj;*.vcproj;*.dll;*.exe",
				"VS.NET Solution (.sln)|*.sln",
				"All VS Project Files (.csproj, .vbproj, .vcproj)|*.csproj;*.vbproj;*.vcproj",
				".NET Libraries and Executables (.dll, .exe)|*.dll;*.exe"
				};
			ofd.Filter = string.Join("|", filters);
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				this.Cursor = Cursors.Wait;
				LiveDocumentorFile.Singleton.Files.Clear();

				try {
					List<DocumentedAssembly> readFiles = DocumentationFileReader.Read(ofd.FileName);
					if (readFiles != null) {
						LiveDocumentorFile.Singleton.Add(readFiles, ofd.FileName);

						this.UpdateView();

						this.userViewingHistory.ClearHistory();
						Model.UserApplicationStore.Store.RecentFiles.AddFile(new TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model.RecentFile(
							ofd.FileName, System.IO.Path.GetFileName(ofd.FileName)
							));
					}
					this.Cursor = null;
				}
				catch (TheBoxSoftware.Reflection.Core.NotAManagedLibraryException) {
					LiveDocumentorFile.Singleton.Files.Clear();	// Clear it again, we already did that before loading
					MessageBox.Show(
						string.Format(ResourcesExceptionText.NOT_A_MANAGED_LIBRARY, ofd.FileName),
						"Unsupported File Type",
						MessageBoxButton.OK,
						MessageBoxImage.Information
						);
				}
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
			else if (e.Command == ApplicationCommands.Print) {
				e.CanExecute = LiveDocumentorFile.Singleton.LiveDocument.DocumentedFiles != null 
					&& LiveDocumentorFile.Singleton.LiveDocument.DocumentedFiles.Count > 0;
			}
			else if (e.Command == ApplicationCommands.Find) {
				e.CanExecute = true;
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
				this.AddDocumentationFile(sender, e);
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
		}
		#endregion

		/// <summary>
		/// Updates the view of the live document. This is just the document map in the
		/// tree view.
		/// </summary>
		public void UpdateView() {
			LiveDocument document = LiveDocumentorFile.Singleton.Update();
			this.documentMap.ItemsSource = document.DocumentMap;
			if (document.DocumentMap.Count > 0) {
				this.pageViewer.Document = document.DocumentMap[0].Page;
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
            if (e.NewValue != null && !(e.NewValue is EmptyEntry)) {
                this.Cursor = Cursors.Wait;
                TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Page page = ((Entry)e.NewValue).Page;
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
			//if (sender == this.TestException) {
				throw new ApplicationException("Its just a test halt!");
			//}
			//else {
				//this.Cursor = Cursors.AppStarting;
				//Model.DocumentationExporter exporter = new TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model.DocumentationExporter();
				//exporter.Export();
				//this.Cursor = null;
			//}

			System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
			xdoc.LoadXml(System.Windows.Markup.XamlWriter.Save(((Entry)this.documentMap.SelectedItem).Page));
			xdoc.Save(@"test.xml");
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

		/// <summary>
		/// Loads up the recent file
		/// </summary>
		/// <param name="file"></param>
		internal void LoadRecentFile(Model.RecentFile file) {
			if (System.IO.File.Exists(file.Filename)) {
				this.Cursor = Cursors.Wait;
				LiveDocumentorFile.Singleton.Files.Clear();
				LiveDocumentorFile.Singleton.Add(DocumentationFileReader.Read(file.Filename), file.Filename);
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

		private void mainWindow_Activated(object sender, EventArgs e) {
			this.Cursor = Cursors.Wait;
			Entry preUpdateSelection = this.currentSelection;
			Entry preUpdateSelectionParent = this.currentSelectionParent;
            bool wasExpanded = preUpdateSelection == null ? false : preUpdateSelection.IsExpanded;
			bool hasBeenReloaded = false;

			foreach (DocumentedAssembly current in LiveDocumentorFile.Singleton.Files) {
				if (current.HasAssemblyBeenModified()) {
					hasBeenReloaded = true;

					// The assembly has been modified, find the existing node
					// and generate the new one
					Entry existingEntry = null;
					int entryAtIndex = -1;
					for (int i = 0; i < LiveDocumentorFile.Singleton.LiveDocument.DocumentMap.Count; i++) {
						Entry currentEntry = LiveDocumentorFile.Singleton.LiveDocument.DocumentMap[i];
						if (currentEntry.Name == System.IO.Path.GetFileName(current.FileName)) {
							existingEntry = currentEntry;
							entryAtIndex = i;
							break;
						}
					}

					// Remove the old entry, we need to do this now so any searches performed in the
					// generate method do not return values from here.
					LiveDocumentorFile.Singleton.LiveDocument.DocumentMap.RemoveAt(entryAtIndex);

					int fileCounter = ((TheBoxSoftware.Reflection.AssemblyDef)existingEntry.Item).UniqueId;
					Entry assemblyEntry = LiveDocumentorFile.Singleton.LiveDocument.GenerateDocumentForAssembly(current, ref fileCounter);

					// Insert the newly generated entry in the same location as the old one
					LiveDocumentorFile.Singleton.LiveDocument.DocumentMap.Insert(entryAtIndex, assemblyEntry);
				}
			}

			// Tries to reselect a node in the tree that was selected before the window was
			// activated. That is before we tried to reload the project.
			if (hasBeenReloaded && (preUpdateSelectionParent != null && preUpdateSelection != null)) {
				// We need something in the document map to search on
				if (LiveDocumentorFile.Singleton.LiveDocument.DocumentMap.Count >= 1) {
					Entry foundParent = LiveDocumentorFile.Singleton.LiveDocument.DocumentMap.First<Entry>(
						entry => entry.Key == preUpdateSelectionParent.Key
						);
					Entry foundEntry = foundParent.FindByKey(preUpdateSelection.Key, preUpdateSelection.SubKey);
					if (foundEntry != null) {
						foundEntry.IsSelected = true;
                        foundEntry.IsExpanded = true;
						foundEntry.IsExpanded = wasExpanded;
					}
				}
			}

			this.Cursor = null;
		}

		private void ShowPreferences(object sender, RoutedEventArgs e) {
			Preferences p = new Preferences();
			p.Owner = this;
			p.ShowDialog();
		}

		private void ShowAbout(object sender, RoutedEventArgs e) {
			About about = new About();
			about.Owner = this;
			about.ShowDialog();
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
	}
}
