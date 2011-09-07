﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using TheBoxSoftware.Documentation.Exporting;
using TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	/// <summary>
	/// Window that allows the user to export the documentation. Allows for selection of
	/// export to and settings controlling the way it is exported.
	/// </summary>
	public partial class Export : Window {
		private ManualResetEvent resetEvent = null;
		protected List<ExportConfigFile> exportFiles = new List<ExportConfigFile>();
		private Settings settingsWindow = new Settings();
		private Exporter threadedExporter;
		private bool exportComplete = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="Export"/> class.
		/// </summary>
		public Export() {
			InitializeComponent();

			this.PrivacyFilters = new PrivacyFilterCollection {
				new PrivacyFilter("Document internal members", Reflection.Visibility.Internal),
				new PrivacyFilter("Document private members", Reflection.Visibility.Private),
				new PrivacyFilter("Document protected members", Reflection.Visibility.Protected),
				new PrivacyFilter("Document protected internal members", Reflection.Visibility.InternalProtected)
														  };
			this.PrivacyFilters.SetFilters(LiveDocumentorFile.Singleton.Filters); // set defaults
			this.visibility.ItemsSource = this.PrivacyFilters;
			this.publishTo.Text = LiveDocumentorFile.Singleton.OutputLocation;

			this.DataContext = this;
			this.LoadConfigFiles();
		}

		/// <summary>
		/// Loads all of teh ldec files and updates the view with the names of the files.
		/// </summary>
		private void LoadConfigFiles() {
			string appFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			foreach (string file in System.IO.Directory.GetFiles(appFolder + @"/ApplicationData/", "*.ldec")) {
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

			//this.exportSelection.Visibility = Visibility.Hidden;
			this.settings.Visibility = Visibility.Collapsed;
			this.export.Visibility = Visibility.Collapsed;
			this.finish.Visibility = Visibility.Visible;
			System.Windows.Forms.Application.DoEvents();

			LiveDocumentorFile.Singleton.OutputLocation = this.publishTo.Text; // store the users output selection
			
			this.exportSelection.BeginAnimation(OpacityProperty, (AnimationTimeline)this.FindResource("OptionsHide"));
			this.BeginAnimation(HeightProperty, (AnimationTimeline)this.FindResource("ShrinkWindow"));
			this.exportProgress.BeginAnimation(OpacityProperty, (AnimationTimeline)this.FindResource("OptionsShow"));
			System.Windows.Forms.Application.DoEvents();

			Documentation.Exporting.Exporter exporter = null;
			ExportConfigFile config = (ExportConfigFile)this.outputSelection.SelectedItem;
			this.exportDescription.Text = config.Description;

			ExportSettings settings = new ExportSettings();
			settings.PublishDirectory = this.publishTo.Text.EndsWith("\\") ? this.publishTo.Text : this.publishTo.Text + "\\";
			settings.Settings = new Documentation.DocumentSettings();
			foreach (PrivacyFilter filter in this.PrivacyFilters) {
				if (filter.IsSelected) {
					settings.Settings.VisibilityFilters.Add(filter.Visibility);
				}
			}

			TheBoxSoftware.Documentation.Document document = new Documentation.Document(LiveDocumenter.LiveDocumentorFile.Singleton.LiveDocument.Assemblies);
			document.Settings = settings.Settings;
			document.UpdateDocumentMap();

			exporter = Documentation.Exporting.Exporter.Create(document, settings, config);
			
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

		private void ThreadedExport(object state) {
			this.threadedExporter = state as Documentation.Exporting.Exporter;
			if (this.threadedExporter!= null) {
				this.threadedExporter.Export();
				GC.Collect();
				this.exportComplete = true;
			}
		}

		/// <summary>
		/// Shows the settings dialoge and updates the settings for the export.
		/// </summary>
		private void ShowSettings() {
			this.settingsWindow.ShowDialog();
			this.settingsWindow.Owner = this;
		}

		/// <summary>
		/// Cancels the export operation and closes the Export window.
		/// </summary>
		private void Cancel() {
			this.Cursor = Cursors.Wait;
			if (this.threadedExporter != null) {
				this.threadedExporter.Cancel();
				while (!this.exportComplete) {
					Thread.Sleep(60);
				}
			}
			this.Cursor = null;
			this.Close();
		}

		#region Properties
		internal PrivacyFilterCollection PrivacyFilters { get; set; }
		#endregion

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

		private void outputSelection_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			ExportConfigFile o = (ExportConfigFile)this.outputSelection.SelectedItem;
			this.exportDescription.Text = o.Description;
			this.exportVersion.Text = "v " + o.Version;

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
				case Exporters.HelpViewer1:
					this.exportType.Text = "MS Help Viewer 1";
					break;
				case Exporters.XML:
					this.exportType.Text = "XML";
					break;
			}

			if (o.HasScreenshot) {
				System.Windows.Media.Imaging.BitmapImage image = new System.Windows.Media.Imaging.BitmapImage();
				image.BeginInit();
				image.StreamSource = o.GetScreenshot();
				image.EndInit();
				this.exportImage.Source = image;
			}
			else {
				this.exportImage.Source = null;
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

		private void fileDialogOpen_Click(object sender, System.Windows.RoutedEventArgs e) {
			System.Windows.Forms.FolderBrowserDialog ofd = new System.Windows.Forms.FolderBrowserDialog();
			string[] filters = new string[] {
				"All Files (.sln, .csproj, .vbproj, .vcproj, .dll, .exe)|*.sln;*.csproj;*.vbproj;*.vcproj;*.dll;*.exe",
				"VS.NET Solution (.sln)|*.sln",
				"All VS Project Files (.csproj, .vbproj, .vcproj)|*.csproj;*.vbproj;*.vcproj",
				".NET Libraries and Executables (.dll, .exe)|*.dll;*.exe"
				};
			ofd.ShowNewFolderButton = true;
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				this.publishTo.Text = ofd.SelectedPath;
			}
		}
		#endregion
	}
}
