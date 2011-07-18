using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : Window {
		private EventHandler cancelled;
		private EventHandler accepted;

		/// <summary>
		/// Initialises a new instance of the Settings form.
		/// </summary>
		public Settings() {
			InitializeComponent();

			this.PrivacyFilters = new ObservableCollection<PrivacyFilter> {
				new PrivacyFilter("Document internal members", Reflection.Visibility.Internal),
				new PrivacyFilter("Document private members", Reflection.Visibility.Private),
				new PrivacyFilter("Document protected members", Reflection.Visibility.Protected),
				new PrivacyFilter("Document protected internal members", Reflection.Visibility.InternalProtected)
														  };

			this.DataContext = this;
		}

		private void Apply() {
			this.OnAccepted();
		}

		private void Cancel() {
			this.OnCancelled();
		}

		protected void OnCancelled() {
			if (this.cancelled != null) {
				this.cancelled(this, EventArgs.Empty);
			}
		}

		protected void OnAccepted() {
			if (this.accepted != null) {
				this.accepted(this, EventArgs.Empty);
			}
		}

		public event EventHandler Cancelled {
			add { cancelled += value; }
			remove { cancelled -= value; }
		}

		public event EventHandler Accepted {
			add { accepted += value; }
			remove { accepted -= value; }
		}

		public ObservableCollection<PrivacyFilter> PrivacyFilters { get; set; }

		#region Event Handlers
		private void button_Click(object sender, EventArgs e) {
			Button clickedButton = sender as Button;
			if (clickedButton == null)
				throw new InvalidOperationException();

			switch (clickedButton.Name) {
				case "apply":
					break;
				case "cancel":
					break;
			}
		}
		#endregion

		public class PrivacyFilter
		{
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
