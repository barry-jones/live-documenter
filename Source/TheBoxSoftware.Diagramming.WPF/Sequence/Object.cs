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

namespace TheBoxSoftware.Diagramming.WPF.Sequence {
	public class Object : UserControl {
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof(string), typeof(Object)
			);

		/// <summary>
		/// Static constructor initialises the default styles associated with this control
		/// </summary>
		static Object() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Object), 
				new FrameworkPropertyMetadata(typeof(Object)));
		}

		public Object() : base() { }
		public Object(Object o)
			: base() {
			this.ActualObject = o;
			this.Text = o.Name;
		}

		public string Text {
			get { return (string)this.GetValue(TextProperty); }
			set { this.SetValue(TextProperty, value); }
		}

		public Object ActualObject {
			get;
			set;
		}
	}
}
