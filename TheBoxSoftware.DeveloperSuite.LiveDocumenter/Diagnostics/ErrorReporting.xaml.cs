﻿using System;
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
using System.Net;
using System.Net.Mail;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Diagnostics {
	/// <summary>
	/// Interaction logic for ErrorReporting.xaml
	/// </summary>
	public partial class ErrorReporting : Window {
		private Exception currentException;
		private const string template = "StandardErrorReport.xml";

		/// <summary>
		/// Constructor
		/// </summary>
		public ErrorReporting() {
			InitializeComponent();
		}

		/// <summary>
		/// Sets the exception which has halted the application
		/// </summary>
		/// <param name="ex">The exception</param>
		public void SetException(Exception ex) {
			this.currentException = ex;
			this.txtExceptionDetails.Text = ex.ToString();
		}

		/// <summary>
		/// Sends the error report
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void sendErrorReport_Click(object sender, RoutedEventArgs e) {
			try {
				this.Cursor = Cursors.AppStarting;

				BoxSoftwareServices.ErrorReport errorReport = new BoxSoftwareServices.ErrorReport();
				errorReport.ProductName = About.ProductName;
				errorReport.ProductVersion = About.ProductVersion;
				errorReport.DateOccurred = DateTime.Now.ToUniversalTime();
				errorReport.UserActivity = this.txtUserDescription.Text;

				string osVersion = Environment.OSVersion.ToString();
				string framework = Environment.Version.ToString();
				string emial = this.email.Text;
				errorReport.Environment = new TheBoxSoftware.DeveloperSuite.LiveDocumenter.BoxSoftwareServices.EnvironmentInformation();
				errorReport.Environment.OperatingSystem = osVersion;
				errorReport.Environment.FrameworkVersion = framework;
				errorReport.Email = emial;

				// write out all of the exceptions
				Exception current = this.currentException;
				List<BoxSoftwareServices.ExceptionReport> exceptions = new List<BoxSoftwareServices.ExceptionReport>();
				do {
					BoxSoftwareServices.ExceptionReport exceptionReport = new BoxSoftwareServices.ExceptionReport();
					exceptionReport.ExceptionType = current.GetType().ToString();
					exceptionReport.Message = current.Message;
					exceptionReport.StackTrace = current.StackTrace;
					exceptionReport.Data = this.WriteDictionary(current.Data);
					exceptions.Add(exceptionReport);
					current = current.InnerException;

				} while (current != null);
				errorReport.Exceptions = exceptions.ToArray();

				// get the referenced assemblies and the details

				BoxSoftwareServices.ErrorReportingSoapClient client = new BoxSoftwareServices.ErrorReportingSoapClient();
				client.ReportAnError(errorReport);
			}
			finally {
				this.Cursor = null;
			}
			this.Close();
		}

		/// <summary>
		/// Reads the "embedded resource" from the diagnostics folder with the
		/// specified filename.
		/// </summary>
		/// <returns>A string builder containing the loaded templates content.</returns>
		private StringBuilder ReadErrorReportTemplate() {
			StringBuilder content = new StringBuilder();
			try {
				object o = Application.Current.Resources;
				System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
				System.IO.StreamReader reader = new System.IO.StreamReader(
					a.GetManifestResourceStream(string.Format("TheBoxSoftware.DeveloperSuite.LiveDocumenter.Diagnostics.{0}", ErrorReporting.template))
					);
				content.Append(reader.ReadToEnd());
				reader.Close();
			}
			finally { }
			return content;
		}

		private string WriteDictionary(System.Collections.IDictionary dictionary) {
			StringBuilder sb = new StringBuilder();
			if (dictionary != null && dictionary.Count > 0) {
				foreach (System.Collections.DictionaryEntry entry in dictionary) {
					sb.AppendFormat("{0} - {1}|", entry.Key, entry.Value);
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// Simply closes the form returning control to the application error handler.
		/// </summary>
		/// <param name="sender">Calling object</param>
		/// <param name="e">Event arguments</param>
		private void cancel_Click(object sender, RoutedEventArgs e) {
			this.Close();
		}
	}
}