using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model;
using TheBoxSoftware.Reflection.Syntax;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
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
			this.PrivacyFilters.SetFilters(LiveDocumentorFile.Singleton.Filters);

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

		internal PrivacyFilterCollection PrivacyFilters { get; set; }

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
	}
}
