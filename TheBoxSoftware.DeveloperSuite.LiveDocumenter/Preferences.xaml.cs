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

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Reflection.Syntax;
	using System.Collections.ObjectModel;

	/// <summary>
	/// Document settings allows the user to modify the settings for the current <see cref="LiveDocumentorFile"/>.
	/// </summary>
	public partial class Preferences : Window {
		private string[] buildConfigurations = new string[] { "Release", "Debug" };
		private string[] languages = new string[] { "CSharp", "VisualBasic" };

		/// <summary>
		/// Initialises a new instance of the Preferance Window.
		/// </summary>
		public Preferences() {
			this.InitializeComponent();

			this.buildConfiguration.ItemsSource = this.buildConfigurations;
			this.language.ItemsSource = this.languages;

			// Set the currently selected items
			this.buildConfiguration.SelectedIndex = this.buildConfiguration.Items.IndexOf(
				LiveDocumentorFile.Singleton.Configuration.ToString()
				);
			this.language.SelectedIndex = this.language.Items.IndexOf(
				LiveDocumentorFile.Singleton.Language.ToString()
				);

			this.PrivacyFilters = new PrivacyFilterCollection {
				new PrivacyFilter("Document internal members", Reflection.Visibility.Internal),
				new PrivacyFilter("Document private members", Reflection.Visibility.Private),
				new PrivacyFilter("Document protected members", Reflection.Visibility.Protected),
				new PrivacyFilter("Document protected internal members", Reflection.Visibility.InternalProtected)
				};

			// set the currently selected filters
			foreach(Reflection.Visibility filter in LiveDocumentorFile.Singleton.Filters) {
				PrivacyFilter p = this.PrivacyFilters.ToList().Find(c => c.Visibility == filter);
				if (p != null) {
					p.IsSelected = true;
				}
			}

			this.privacyFilters.ItemsSource = this.PrivacyFilters;
		}

		private void Apply(object sender, RoutedEventArgs e) {
			Languages selectedLanguage = (Languages)Enum.Parse(typeof(Languages), this.language.SelectedItem.ToString());
			List<Reflection.Visibility> filters = this.PrivacyFilters.GetFilters();
			// check if there have been changes
			bool changed = false;

			Reflection.Visibility[] original = LiveDocumentorFile.Singleton.Filters.ToArray();
			Reflection.Visibility[] selectedFilters = filters.ToArray();
			changed = !original.SequenceEqual(selectedFilters);


			LiveDocumentorFile.Singleton.Language = selectedLanguage;
			LiveDocumentorFile.Singleton.Configuration = (Model.BuildConfigurations)Enum.Parse(typeof(Model.BuildConfigurations), this.buildConfiguration.SelectedItem.ToString());
			LiveDocumentorFile.Singleton.Filters = filters;
			this.DialogResult = changed;
			this.Close();
		}

		private void Cancel(object sender, RoutedEventArgs e) {
			this.Close();
		}

		protected string[] BuildConfigurations {
			get { return this.buildConfigurations; }
		}

		protected string[] Languages {
			get { return this.languages; }
		}

		protected PrivacyFilterCollection PrivacyFilters { get; set; }

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

		protected class PrivacyFilterCollection : ObservableCollection<PrivacyFilter> {
			public override string ToString() {
				List<string> selectedNames = new List<string>();

				foreach (PrivacyFilter current in this) {
					if (current.IsSelected) {
						selectedNames.Add(current.Visibility.ToString());
					}
				}

				return selectedNames.Count > 0
					? selectedNames.Count == this.Count ? "Document all members" : string.Format("Document {0} members", string.Join(", ", selectedNames.ToArray()))
					: string.Empty;
			}

			public List<Reflection.Visibility> GetFilters() {
				List<Reflection.Visibility> filters = new List<Reflection.Visibility>();
				foreach (PrivacyFilter filter in this) {
					if (filter.IsSelected) filters.Add(filter.Visibility);
				}
				return filters;
			}
		}

		protected class PrivacyFilter {
			public PrivacyFilter(string title, TheBoxSoftware.Reflection.Visibility filter) {
				this.Title = title;
				this.Visibility = filter;
			}

			public string Title { get; set; }
			public TheBoxSoftware.Reflection.Visibility Visibility { get; set; }
			public bool IsSelected { get; set; }
		}
	}
}
