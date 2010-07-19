using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            Application.Current.DispatcherUnhandledException += 
                new DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);

            base.OnStartup(e);
        }

        /// <summary>
        /// Last chance exception handler. 
        /// </summary>
        /// <param name="sender">Calling object</param>
        /// <param name="e">Event arguments</param>
        void Current_DispatcherUnhandledException(object sender, 
            DispatcherUnhandledExceptionEventArgs e) {
            TheBoxSoftware.Diagnostics.Logging.Log(e.Exception);

			e.Handled = true;

			Diagnostics.ErrorReporting errorReport = new Diagnostics.ErrorReporting();
			errorReport.SetException(e.Exception);
			errorReport.ShowDialog();
			Application.Current.Shutdown();
        }
    }
}
