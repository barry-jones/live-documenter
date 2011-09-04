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
		private PrivacyFilterCollection originalFilters = new PrivacyFilterCollection();

		/// <summary>
		/// Initialises a new instance of the Settings form.
		/// </summary>
		public Settings() {
			InitializeComponent();

			this.PrivacyFilters = new PrivacyFilterCollection {
				new PrivacyFilter("Document internal members", Reflection.Visibility.Internal),
				new PrivacyFilter("Document private members", Reflection.Visibility.Private),
				new PrivacyFilter("Document protected members", Reflection.Visibility.Protected),
				new PrivacyFilter("Document protected internal members", Reflection.Visibility.InternalProtected)
														  };

			this.DataContext = this;
		}

		private void Apply() {
			this.OnAccepted();
			this.Hide();
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

		public PrivacyFilterCollection PrivacyFilters { get; set; }

		#region Event Handlers
		private void button_Click(object sender, EventArgs e) {
			Button clickedButton = sender as Button;
			if (clickedButton == null)
				throw new InvalidOperationException();

			switch (clickedButton.Name) {
				case "apply":
					this.Apply();
					break;
				case "cancel":
					this.Cancel();
					break;
			}
		}
		#endregion

		public class PrivacyFilterCollection : ObservableCollection<PrivacyFilter> {
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
		}

		public class PrivacyFilter {
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