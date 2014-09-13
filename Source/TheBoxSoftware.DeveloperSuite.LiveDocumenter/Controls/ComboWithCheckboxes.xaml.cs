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

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Controls {
	/// <summary>
	/// Interaction logic for ComboWithCheckboxes.xaml
	/// </summary>
	public partial class ComboWithCheckboxes : UserControl {
		public static readonly DependencyProperty DefaultTextProperty;
		public static readonly DependencyProperty ItemsSourceProperty;
		public static readonly DependencyProperty TextProperty;

		static ComboWithCheckboxes() {
			ComboWithCheckboxes.DefaultTextProperty = DependencyProperty.Register(
				"DefaultText", typeof(string), typeof(ComboWithCheckboxes), new FrameworkPropertyMetadata(string.Empty)
				);
			ComboWithCheckboxes.ItemsSourceProperty = ItemsSourceProperty = DependencyProperty.Register(
				"ItemsSource", typeof(object), typeof(ComboWithCheckboxes), new FrameworkPropertyMetadata(null)
				);
			ComboWithCheckboxes.TextProperty = DependencyProperty.Register(
				"Text", typeof(string), typeof(ComboWithCheckboxes), new FrameworkPropertyMetadata(string.Empty)
				);
		}

		public ComboWithCheckboxes() {
			this.InitializeComponent();
			this.SetText();
		}

		#region Dependency Properties
		/// <summary> 
		///Gets or sets a collection used to generate the content of the ComboBox 
		/// </summary> 
		public object ItemsSource {
			get { return (object)GetValue(ComboWithCheckboxes.ItemsSourceProperty); }
			set {
				this.SetValue(ComboWithCheckboxes.ItemsSourceProperty, value);
				this.SetText();
			}
		}

		/// <summary> 
		///Gets or sets the text displayed in the ComboBox 
		/// </summary> 
		public string Text {
			get { return (string)GetValue(ComboWithCheckboxes.TextProperty); }
			set { SetValue(ComboWithCheckboxes.TextProperty, value); }
		}


		/// <summary> 
		///Gets or sets the text displayed in the ComboBox if there are no selected items 
		/// </summary> 
		public string DefaultText {
			get { return (string)this.GetValue(ComboWithCheckboxes.DefaultTextProperty); }
			set { this.SetValue(ComboWithCheckboxes.DefaultTextProperty, value); }
		}
		#endregion

		/// <summary> 
		///Whenever a CheckBox is checked, change the text displayed 
		/// </summary> 
		/// <param name="sender"></param> 
		/// <param name="e"></param> 
		private void CheckBox_Click(object sender, RoutedEventArgs e) {
			this.SetText();
		}

		/// <summary> 
		///Set the text property of this control (bound to the ContentPresenter of the ComboBox) 
		/// </summary> 
		private void SetText() {
			this.Text = (this.ItemsSource != null) ? this.ItemsSource.ToString() : this.DefaultText;

			// set DefaultText if nothing else selected 
			if (string.IsNullOrEmpty(this.Text)) {
				this.Text = "Document Public members";
			}
		}
	}
}
