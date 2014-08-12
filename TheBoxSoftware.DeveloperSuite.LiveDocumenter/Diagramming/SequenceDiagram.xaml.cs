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

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Diagramming {
	using d = TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model.Diagram.SequenceDiagram;
	/// <summary>
	/// Interaction logic for SequenceDiagram.xaml
	/// </summary>
	public partial class SequenceDiagram : UserControl {
		internal SequenceDiagram() {
			InitializeComponent();
		}

		internal SequenceDiagram(d.SequenceDiagram diagram) {
			InitializeComponent();
			int lastPos = 1;
			foreach (d.Object current in diagram.GetObjects()) {
				SequenceObject newObject = new SequenceObject(current);
				Canvas.SetLeft(newObject, lastPos);
				lastPos += 100;
				this.canvas.Children.Add(newObject);
			}
		}
	}
}
