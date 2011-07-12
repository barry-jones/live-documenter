using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Model {
	class TableData {
		public TableData(object data) {
		}

		public GridViewColumnCollection Columns {
			get {
				GridViewColumnCollection columns = new GridViewColumnCollection();
				GridViewColumn test = new GridViewColumn();
				test.Header = "This is a test header";
				columns.Add(test);
				return columns;
			}
		}
	}
}
