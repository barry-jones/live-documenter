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
using System.Windows.Controls.Primitives;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Controls {
	/// <summary>
	/// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
	///
	/// Step 1a) Using this custom control in a XAML file that exists in the current project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:TheBoxSoftware.DeveloperSuite.LiveDocumenter.Controls"
	///
	///
	/// Step 1b) Using this custom control in a XAML file that exists in a different project.
	/// Add this XmlNamespace attribute to the root element of the markup file where it is 
	/// to be used:
	///
	///     xmlns:MyNamespace="clr-namespace:TheBoxSoftware.DeveloperSuite.LiveDocumenter.Controls;assembly=TheBoxSoftware.DeveloperSuite.LiveDocumenter.Controls"
	///
	/// You will also need to add a project reference from the project where the XAML file lives
	/// to this project and Rebuild to avoid compilation errors:
	///
	///     Right click on the target project in the Solution Explorer and
	///     "Add Reference"->"Projects"->[Browse to and select this project]
	///
	///
	/// Step 2)
	/// Go ahead and use your control in the XAML file.
	///
	///     <MyNamespace:DropDownButton/>
	///
	/// </summary>
	public class DropDownButton : Button {
		#region Dependency Properties

		public static readonly DependencyProperty DropDownContextMenuProperty = DependencyProperty.Register("DropDownContextMenu", typeof(ContextMenu), typeof(DropDownButton), new UIPropertyMetadata(null));
		public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(DropDownButton));
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(DropDownButton));
		public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(UIElement), typeof(DropDownButton));
		public static readonly DependencyProperty DropDownButtonCommandProperty = DependencyProperty.Register("DropDownButtonCommand", typeof(ICommand), typeof(DropDownButton), new FrameworkPropertyMetadata(null));
		public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsCheckedProperty", typeof(bool), typeof(DropDownButton), new FrameworkPropertyMetadata(null));

		#endregion

		#region Constructors

		public DropDownButton() {
			// Bind the ToogleButton.IsChecked property to the drop-down's IsOpen property 
			var binding = new Binding("DropDownContextMenu.IsOpen") { Source = this };
			SetBinding(IsCheckedProperty, binding);
		}

		#endregion

		#region Properties

		public ContextMenu DropDownContextMenu {
			get { return GetValue(DropDownContextMenuProperty) as ContextMenu; }
			set { SetValue(DropDownContextMenuProperty, value); }
		}

		public ImageSource Image {
			get { return GetValue(ImageProperty) as ImageSource; }
			set { SetValue(ImageProperty, value); }
		}

		public string Text {
			get { return GetValue(TextProperty) as string; }
			set { SetValue(TextProperty, value); }
		}

		public UIElement Target {
			get { return GetValue(TargetProperty) as UIElement; }
			set { SetValue(TargetProperty, value); }
		}

		public ICommand DropDownButtonCommand {
			get { return GetValue(DropDownButtonCommandProperty) as ICommand; }
			set { SetValue(DropDownButtonCommandProperty, value); }
		}

		public bool IsChecked {
			get { return (bool)this.GetValue(IsCheckedProperty); }
			set { this.SetValue(IsCheckedProperty, value); }
		}

		#endregion

		#region Protected Override Methods

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
			base.OnPropertyChanged(e);

			if (e.Property == DropDownButtonCommandProperty)
				Command = DropDownButtonCommand;
		}

		protected override void OnClick() {
			if (DropDownContextMenu == null) return;

			if (DropDownButtonCommand != null) DropDownButtonCommand.Execute(null);

			// If there is a drop-down assigned to this button, then position and display it 
			DropDownContextMenu.PlacementTarget = this;
			DropDownContextMenu.Placement = PlacementMode.Bottom;
			DropDownContextMenu.IsOpen = !DropDownContextMenu.IsOpen;
		}

		#endregion
	}
}
