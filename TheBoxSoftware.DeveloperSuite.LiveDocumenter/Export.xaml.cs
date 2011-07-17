using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using System.Windows.Threading;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Documentation.Exporting;

	/// <summary>
	/// Window that allows the user to export the documentation. Allows for selection of
	/// export to and settings controlling the way it is exported.
	/// </summary>
	public partial class Export : Window {
		private ManualResetEvent resetEvent = null;
		protected List<ExportConfigFile> exportFiles = new List<ExportConfigFile>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Export"/> class.
		/// </summary>
		public Export() {
			InitializeComponent();
			this.LoadConfigFiles();
		}

		/// <summary>
		/// Loads all of teh ldec files and updates the view with the names of the files.
		/// </summary>
		private void LoadConfigFiles() {
			//Uri applocation = new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location);
			foreach (string file in System.IO.Directory.GetFiles(@"ApplicationData/", "*.ldec")) {
				exportFiles.Add(ExportConfigFile.Create(file));
			}
			exportFiles.Sort((f1, f2) => f1.Name.CompareTo(f2.Name));
			this.outputSelection.Items.Clear();
			this.outputSelection.ItemsSource = exportFiles;
		}

		/// <summary>
		/// Starts the export operation using the selected settings.s
		/// </summary>
		private void ExportDocumentation() {
			this.Cursor = Cursors.AppStarting;

			this.exportSelection.Visibility = Visibility.Hidden;
			this.exportProgress.Visibility = Visibility.Visible;
			this.settings.Visibility = Visibility.Collapsed;
			this.export.Visibility = Visibility.Collapsed;
			this.finish.Visibility = Visibility.Visible;
			System.Windows.Forms.Application.DoEvents();

			Documentation.Exporting.Exporter exporter = null;
			ExportConfigFile o = (ExportConfigFile)this.outputSelection.SelectedItem;
			this.exportDescription.Text = o.Description;

			switch (o.Exporter) {
				case Exporters.Website:
					exporter = new Documentation.Exporting.WebsiteExporter(
						LiveDocumentorFile.Singleton.Files,
						new TheBoxSoftware.Documentation.Exporting.ExportSettings(),
						o
						);
					this.exportType.Text = "Website exporter";
					break;
				case Exporters.Html1:
					exporter = new Documentation.Exporting.HtmlHelp1Exporter(
						LiveDocumentorFile.Singleton.Files,
						new TheBoxSoftware.Documentation.Exporting.ExportSettings(),
						o
						);
					this.exportType.Text = "HTML Help 1 exporter";
					break;
				case Exporters.Html2:
					exporter = new Documentation.Exporting.HtmlHelp2Exporter(
						LiveDocumentorFile.Singleton.Files,
						new TheBoxSoftware.Documentation.Exporting.ExportSettings(),
						o
						);
					break;
			}

			exporter.ExportCalculated += new ExportCalculatedEventHandler(exporter_ExportCalculated);
			exporter.ExportStep += new ExportStepEventHandler(exporter_ExportStep);
			exporter.ExportException += new ExportExceptionHandler(exporter_ExportException);

			if (exporter != null) {
				this.resetEvent = new ManualResetEvent(false);
				Exception e = null;	// holder for exceptions that occur in the thread

				ThreadPool.QueueUserWorkItem(state => {
					DateTime start = DateTime.Now;
					this.ThreadedExport(exporter);

					// set cursor back to normal
					DateTime end = DateTime.Now;
					TimeSpan duration = end.Subtract(start);
					DispatcherOperation op = this.Dispatcher.BeginInvoke(
						DispatcherPriority.Normal,
						new Action<ExportStepEventArgs>(
							p => {
								this.progressIndicator.Value = this.progressIndicator.Maximum;
								this.progressText.Text = string.Format("Complete in {0}:{1}s", (int)duration.TotalMinutes, duration.Seconds);
								this.finish.IsEnabled = true;
								this.Cursor = null;
							}),
						e);

					this.resetEvent.Set();
				});
			}
		}

		/// <summary>
		/// Handles the ExportException event of the exporter control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="TheBoxSoftware.Documentation.Exporting.ExportExceptionEventArgs"/> instance containing the event data.</param>
		void exporter_ExportException(object sender, ExportExceptionEventArgs e) {
			// this.resetEvent.WaitOne();
			throw e.Exception;
		}

		/// <summary>
		/// Handles the ExportStep event of the exporter.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="TheBoxSoftware.Documentation.Exporting.ExportStepEventArgs"/> instance containing the event data.</param>
		private void exporter_ExportStep(object sender, ExportStepEventArgs e) {
			DispatcherOperation op = this.Dispatcher.BeginInvoke(
				DispatcherPriority.Normal,
				new Action<ExportStepEventArgs>(
					p => {
						this.progressIndicator.Value = p.Step;
						this.progressText.Text = p.Description;
					}),
				e);
		}

		/// <summary>
		/// Handles the ExportCalculated event of the exporter control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="TheBoxSoftware.Documentation.Exporting.ExportCalculatedEventArgs"/> instance containing the event data.</param>
		private void exporter_ExportCalculated(object sender, ExportCalculatedEventArgs e) {
			DispatcherOperation op = this.Dispatcher.BeginInvoke(
				DispatcherPriority.Normal,
				new Action<ExportCalculatedEventArgs>(
					p => {
						this.progressIndicator.Value = 0;
						this.progressIndicator.Minimum = 0;
						this.progressIndicator.Maximum = p.NumberOfSteps;
						this.progressText.Text = "Started export";
					}),
				e);
		}

		private void ThreadedExport(object state) {
			Documentation.Exporting.Exporter exporter = state as Documentation.Exporting.Exporter;
			if (exporter != null) {
				exporter.Export();
			}
		}

		/// <summary>
		/// Shows the settings dialoge and updates the settings for the export.
		/// </summary>
		private void ShowSettings() {
		}

		/// <summary>
		/// Cancels the export operation and closes the Export window.
		/// </summary>
		private void Cancel() {
			this.Close();
		}

		#region Event Handlers
		/// <summary>
		/// Handles the Click event of the button control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void button_Click(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			switch (b.Name) {
				case "settings":
					this.ShowSettings();
					break;

				case "cancel":
				case "finish":
					this.Cancel();
					break;

				case "export":
					this.ExportDocumentation();
					break;

				default:
					throw new NotImplementedException(b.Name);
			}
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
				this.Cancel();
			}
		}
		#endregion

		private void outputSelection_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			ExportConfigFile o = (ExportConfigFile)this.outputSelection.SelectedItem;
			this.exportDescription.Text = o.Description;

			switch (o.Exporter) {
				case Exporters.Website:
					this.exportType.Text = "Website exporter";
					break;
				case Exporters.Html1:
					this.exportType.Text = "HTML Help 1 exporter";
					break;
				case Exporters.Html2:
					this.exportType.Text = "HTML Help 2 exporter";
					break;
			}
		}
	}
}
