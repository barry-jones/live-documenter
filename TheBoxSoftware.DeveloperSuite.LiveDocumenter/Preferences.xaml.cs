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

	/// <summary>
	/// Interaction logic for Preferences.xaml
	/// </summary>
	public partial class Preferences : Window {
		private string[] buildConfigurations = new string[] { "Release", "Debug" };
		private string[] languages = new string[] { "CSharp", "VisualBasic" };

		public Preferences() {
			this.InitializeComponent();

			// Set the currently selected items
			this.buildConfiguration.SelectedIndex = this.buildConfiguration.Items.IndexOf(
				Model.UserApplicationStore.Store.Preferences.BuildConfiguration.ToString()
				);
			this.language.SelectedIndex = this.language.Items.IndexOf(
				Model.UserApplicationStore.Store.Preferences.Language.ToString()
				);
		}

		private void Apply(object sender, RoutedEventArgs e) {
			Model.UserApplicationStore.Store.Preferences.Language = (Languages)Enum.Parse(typeof(Languages), this.language.SelectedItem.ToString());
			Model.UserApplicationStore.Store.Preferences.BuildConfiguration = (Model.BuildConfigurations)Enum.Parse(typeof(Model.BuildConfigurations), this.buildConfiguration.SelectedItem.ToString());
			this.Close();
		}

		private void Cancel(object sender, RoutedEventArgs e) {
			this.Close();
		}

		public string[] BuildConfigurations {
			get { return this.buildConfigurations; }
		}

		public string[] Languages {
			get { return this.languages; }
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
	}
}
