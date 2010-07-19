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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Diagramming {
	/// <summary>
	/// Interaction logic for SequenceObject.xaml
	/// </summary>
	public partial class SequenceObject : UserControl {
		public SequenceObject() {
			InitializeComponent();
		}

		public SequenceObject(Model.Diagram.SequenceDiagram.Object o) {
			InitializeComponent();
			this.DataContext = o;
		}
	}
}
