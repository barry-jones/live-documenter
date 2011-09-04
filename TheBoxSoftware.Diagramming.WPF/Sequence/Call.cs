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
	public class Call : Control {
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof(string), typeof(Call)
			);

		/// <summary>
		/// Static constructor initialises the default styles associated with this control
		/// </summary>
		static Call() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Call), 
				new FrameworkPropertyMetadata(typeof(Call)));
		}

		public Call() : base() { }
		public Call(Call c) {
			this.ActualCall = c;
			this.Text = c.Name;
		}

		public string Text {
			get { return (string)this.GetValue(TextProperty); }
			set { this.SetValue(TextProperty, value); }
		}

		public Call ActualCall {
			get;
			set;
		}
	}
}
