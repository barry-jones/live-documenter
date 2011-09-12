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
	/// <summary>
	/// Interaction logic for ExceptionsMessageBox.xaml
	/// </summary>
	public partial class ExceptionsMessageBox : Window {

		/// <summary>
		/// Initialises a new instance of the ExceptoinMessageBox window.
		/// </summary>
		/// <param name="exception">The exceptoin to the report</param>
		public ExceptionsMessageBox(Exception exception) 
			: this(new List<Exception>() { exception }) {
		}

		/// <summary>
		/// Initialises a new instance of the ExceptionsMessagesbox window.
		/// </summary>
		/// <param name="exceptions">The exceptions to display</param>
		public ExceptionsMessageBox(List<Exception> exceptions) {
			this.InitializeComponent();

			this.Exceptions = exceptions;

			foreach (Exception ex in this.Exceptions) {
				if(ex is IExtendedException) {
					string extendedDetails = string.Format("{0}\n\n", ((IExtendedException)ex).GetExtendedInformation());
					if (string.IsNullOrEmpty(extendedDetails)) {
						this.errorDetails.Text += string.Format("{0}\n\n", ex.Message);
					}
					else {
						this.errorDetails.Text += extendedDetails;
					}
				}
				else {
					this.errorDetails.Text += string.Format("{0}\n\n", ex.Message);
				}
			}
		}

		/// <summary>
		/// The exceptions being reported.
		/// </summary>
		protected List<Exception> Exceptions { get; set; }

		/// <summary>
		/// Handles the Click event of the button control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void button_Click(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			switch (b.Name) {
				case "cancel":
					this.Close();
					break;

				case "report":
					Diagnostics.ErrorReporting reporting = new Diagnostics.ErrorReporting();
					reporting.SetExceptions(this.Exceptions);
					reporting.ShowDialog();
					this.Close();
					break;

				default:
					throw new NotImplementedException(b.Name);
			}
		}
	}
}
