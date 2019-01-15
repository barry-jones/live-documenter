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
using System.Reflection;
using System.Diagnostics;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
	public partial class About : Window {
		private static string productDescription;
		private static string productVersion;
		private static string productTitle;
		private static string productName;

		/// <summary>
		/// Initialises a new instance of the About class.
		/// </summary>
		public About() {
			InitializeComponent();

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

			this.assemblyName.Text = About.ProductName;
			this.version.Text = "v " + fvi.ProductVersion;
		}

		/// <summary>
		/// Gets the AssemblyInfo defined description of the product
		/// </summary>
		public static string ProductDescription {
			get {
				Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
				if (assembly != null) {
					object[] customAttributes = assembly.GetCustomAttributes
							(typeof(AssemblyDescriptionAttribute), false);
					if ((customAttributes != null) && (customAttributes.Length > 0)) {
						productDescription =
						  ((AssemblyDescriptionAttribute)customAttributes[0]).Description;
					}
					if (string.IsNullOrEmpty(productDescription)) {
						productDescription = string.Empty;
					}
				}
				return productDescription;
			}
		}

		/// <summary>
		/// Gets the assembly info defined product title
		/// </summary>
		public static string ProductTitle {
			get {
				Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
				if (assembly != null) {
					object[] customAttributes = assembly.GetCustomAttributes
							(typeof(AssemblyTitleAttribute), false);
					if ((customAttributes != null) && (customAttributes.Length > 0)) {
						productTitle =
							((AssemblyTitleAttribute)customAttributes[0]).Title;
					}
					if (string.IsNullOrEmpty(productTitle)) {
						productTitle = string.Empty;
					}
				}
				return productTitle;
			}
		}

		/// <summary>
		/// Gets the assembly info defined product name
		/// </summary>
		public static string ProductName {
			get {
				Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
				if (assembly != null) {
					object[] customAttributes = assembly.GetCustomAttributes
						(typeof(AssemblyProductAttribute), false);
					if ((customAttributes != null) && (customAttributes.Length > 0)) {
						productName =
							((AssemblyProductAttribute)customAttributes[0]).Product;
					}
					if (string.IsNullOrEmpty(productName)) {
						productName = string.Empty;
					}
				}
				return productName;
			}
		}

		/// <summary>
		/// Gets the assembly info defined product version
		/// </summary>
		public static string ProductVersion {
			get {
				Assembly assembly = Assembly.GetExecutingAssembly();
				if (assembly != null) {                    
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                    productVersion = fvi.ProductVersion;
					if (string.IsNullOrEmpty(productVersion)) {
						productVersion = string.Empty;
					}
				}
				return productVersion;
			}
		}

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
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
				this.Close();
			}
		}
	}
}
