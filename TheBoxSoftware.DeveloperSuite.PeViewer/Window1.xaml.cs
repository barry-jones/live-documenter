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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheBoxSoftware.DeveloperSuite.PEViewer {
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window {
		private Model.PEFile peFile;

		public Window1() {
			InitializeComponent();
		}

		/// <summary>
		/// Initilialises the window for the newly loaded PEFile.
		/// </summary>
		private void InitialiseForNewPEFile() {
			this.peViewMap.ItemsSource = peFile.Entries;
		}

		#region Event Handlers
		/// <summary>
		/// Handles the user wanting load an assembly.
		/// </summary>
		/// <param name="sender">Calling object</param>
		/// <param name="e">Event arguments</param>
		private void LoadAssembly_Click(object sender, RoutedEventArgs e) {
			System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				peFile = new Model.PEFile(new TheBoxSoftware.Reflection.Core.PeCoffFile(ofd.FileName));
				this.InitialiseForNewPEFile();
			}
		}

		private void ShowAbout(object sender, RoutedEventArgs e) {
			TheBoxSoftware.DeveloperSuite.LiveDocumenter.About about = new TheBoxSoftware.DeveloperSuite.LiveDocumenter.About();
			about.ShowDialog();
		}
		#endregion
	}
}
