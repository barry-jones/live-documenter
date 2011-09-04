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
	public class Activation : Control {
		/// <summary>
		/// Static constructor initialises the default styles associated with this control
		/// </summary>
		static Activation() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Activation), 
				new FrameworkPropertyMetadata(typeof(Activation)));
		}

		public Activation() : base() { }
		public Activation(Activation a) {
			this.ActualActivation = a;
		}

		public Activation ActualActivation {
			get;
			set;
		}
	}
}
