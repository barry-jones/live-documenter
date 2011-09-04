using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	using TheBoxSoftware.Diagnostics;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            Application.Current.DispatcherUnhandledException += 
                new DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);

            base.OnStartup(e);

			System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB", true);

			// setup traceing
			System.Diagnostics.BooleanSwitch ts = new System.Diagnostics.BooleanSwitch("TRACE", string.Empty);
			TraceHelper.IsTraceEnabled = ts.Enabled;
			TraceHelper.WriteLine(new string('#', 25));

			this.Arguments = e.Args;
        }

        /// <summary>
        /// Last chance exception handler. 
        /// </summary>
        /// <param name="sender">Calling object</param>
        /// <param name="e">Event arguments</param>
        void Current_DispatcherUnhandledException(object sender, 
            DispatcherUnhandledExceptionEventArgs e) {
            TheBoxSoftware.Diagnostics.Logging.Log(e.Exception);

			// [#87] close the main window so we dont get any activation errors (for ever repeating error dialogues)
			((MainWindow)App.Current.MainWindow).AllowFileRefreshing = false;

			e.Handled = true;

			Diagnostics.ErrorReporting errorReport = new Diagnostics.ErrorReporting();
			errorReport.SetException(e.Exception);
			errorReport.ShowDialog();
			App.Current.Shutdown();
        }

		/// <summary>
		/// A file specified on the command line for the application to load
		/// </summary>
		public string[] Arguments { get; set; }
    }
}
