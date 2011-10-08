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
using System.Windows.Shapes;
using TheBoxSoftware.Documentation;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	/// <summary>
	/// Interaction logic for ProjectManager.xaml
	/// </summary>
	public partial class ProjectManager : Window {
		private List<string> originalFiles = new List<string>();
		private List<string> originalRemovedAssemblies = new List<string>();

		/// <summary>
		/// Initialises a new instance of the ProjectManager window.
		/// </summary>
		public ProjectManager() {
			this.Entries = new ObservableCollection<ProjectEntry>();

			this.InitializeComponent();
			this.InitialiseFromProject();

			this.DataContext = this;
		}

		private void InitialiseFromProject() {
			Project project = LiveDocumentorFile.Singleton.UnerlyingProject;
			ObservableCollection<ProjectEntry> entries = new ObservableCollection<ProjectEntry>();

			// store original list so we can check for changes.
			originalFiles.AddRange(project.Files);
			originalRemovedAssemblies.AddRange(project.RemovedAssemblies);

			// build the entry list
			foreach (string file in project.Files) {
				ProjectEntry parent = new ProjectEntry();
				parent.Tag = file;
				parent.Name = System.IO.Path.GetFileName(file);
				parent.Type = "parent";

				string extension = System.IO.Path.GetExtension(file);
				switch (extension) {
					case ".sln":
						parent.Icon = "solution.png";
						break;
					case ".dll":
						parent.Icon = "library.png";
						parent.Children = null;
						break;
					default:
						parent.Icon = "project.png";
						break;
				}

				if (parent.Children != null) {
					List<DocumentedAssembly> libraries = InputFileReader.Read(file, project.Configuration);
					foreach (DocumentedAssembly assembly in libraries) {
						ProjectEntry child = new ProjectEntry();
						child.Name = assembly.Name;
						child.Tag = string.Format("{0}\\{1}", parent.Name, child.Name);
						child.Hidden = project.RemovedAssemblies.Contains(child.Tag);
						child.Type = "child";
						parent.Children.Add(child);
					}
				}

				entries.Add(parent);
			}

			this.Entries = entries;
			this.manager.ItemsSource = this.Entries;
		}	

		/// <summary>
		/// The entries to manager
		/// </summary>
		protected ObservableCollection<ProjectEntry> Entries { get; set; }

		#region Internals
		protected class ProjectEntry : INotifyPropertyChanged {
			private bool hidden;

			public ProjectEntry() {
				this.Children = new List<ProjectEntry>();
			}

			public string Type { get; set; }
			public string Icon { get; set; }
			public string Name { get; set; }
			public string Tag { get; set; }
			public bool Hidden {
				get { return this.hidden; }
				set {
					this.hidden = value;
					this.OnPropertyChanged("Hidden");
				}
			}
			public List<ProjectEntry> Children { get; set; }

			#region INotifyPropertyChanged Members

			protected void OnPropertyChanged(string propertyName) {
				if (PropertyChanged != null) {
					this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

			#endregion
		}
		#endregion

		#region Event Handlers
		private void Apply(object sender, RoutedEventArgs e) {
			bool? changed = false;
			List<string> newFiles = new List<string>();
			List<string> newRemovedAssemblies = new List<string>();

			for (int i = 0; i < this.Entries.Count; i++) {
				newFiles.Add(this.Entries[i].Tag);

				if (this.Entries[i].Children != null && this.Entries[i].Children.Count > 0) {
					for (int j = 0; j < this.Entries[i].Children.Count; j++) {
						if (this.Entries[i].Children[j].Hidden) {
							newRemovedAssemblies.Add(this.Entries[i].Children[j].Tag);
						}
					}
				}
			}

			newFiles.Sort();
			newRemovedAssemblies.Sort();

			Project underlyingProject = LiveDocumentorFile.Singleton.UnerlyingProject;
			if (!newFiles.SequenceEqual(underlyingProject.Files) || !newRemovedAssemblies.SequenceEqual(underlyingProject.RemovedAssemblies)) {
				LiveDocumentorFile.Singleton.UnerlyingProject.Files = newFiles;
				LiveDocumentorFile.Singleton.UnerlyingProject.RemovedAssemblies = newRemovedAssemblies;
				changed = true;
				LiveDocumentorFile.Singleton.HasChanged = true;
			}

			this.DialogResult = changed;
			this.Close();
		}

		/// <summary>
		/// Hides or removes the currently selected item based on its position in list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HideAndRemove(object sender, RoutedEventArgs e) {
			ProjectEntry entry = (ProjectEntry)this.manager.SelectedItem;
			if (entry.Type == "child") {
				if (entry.Hidden) {
					entry.Hidden = false;
					this.hideRemoveContent.Text = "Don't document";
				}
				else {
					entry.Hidden = true;
					this.hideRemoveContent.Text = "Document";
				}
			}
			else {
				this.Entries.Remove(entry);
				this.hideRemoveContent.Text = "Remove";
			}
		}

		private void Cancel(object sender, RoutedEventArgs e) {
			this.Close();
		}

		/// <summary>
		/// Command binding event handler, checks if a command executed by the user can be
		/// handled by the application in its current state.
		/// </summary>
		/// <param name="sender">Calling object</param>
		/// <param name="e">Event arguments</param>
		private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			if (e.Command == ApplicationCommands.Close) {
				e.CanExecute = true;
			}
		}

		/// <summary>
		/// Command binding event handler, executes the command executed by the user.
		/// </summary>
		/// <param name="sender">Calling object</param>
		/// <param name="e">Event arguments</param>
		private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) {
			if (e.Command == ApplicationCommands.Close) {
				this.Cancel(sender, e);
			}
		}

		private void manager_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
			this.hideRemove.IsEnabled = this.Entries.Count > 0;
			ProjectEntry entry = (ProjectEntry)this.manager.SelectedItem;
			if (entry != null) {
				if (entry.Type == "child") {
					if (entry.Hidden) {
						this.hideRemoveContent.Text = "Document";
					}
					else {
						this.hideRemoveContent.Text = "Don't document";
					}
				}
				else {
					this.hideRemoveContent.Text = "Remove";
				}
			}
		}
		#endregion
	}
}
