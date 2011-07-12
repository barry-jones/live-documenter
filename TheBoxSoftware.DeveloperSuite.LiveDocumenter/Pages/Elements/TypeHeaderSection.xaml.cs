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

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	using TheBoxSoftware.Reflection;

	/// <summary>
	/// Interaction logic for TypeHeaderSection.xaml
	/// </summary>
	public partial class TypeHeaderSection : Section {
		public TypeHeaderSection() {
			InitializeComponent();
		}

		RoutedEventHandler linkResolver = new RoutedEventHandler(LinkHelper.Resolve);

		/// <summary>
		/// Initialise a new instance of the TypeHeaderSection class.
		/// </summary>
		/// <param name="type">The type to create the header for.</param>
		/// <remarks>
		/// The TypeHeaderSection uses a table to layout evenly all of the links
		/// in a row. It does not show links for elements that are not present in
		/// the type. Further any links it does not show will be replaced with an
		/// empty cell.
		/// </remarks>
		public TypeHeaderSection(TypeDef type) {
			InitializeComponent();

			Table viewingTable = new Table();
			this.Blocks.Add(viewingTable);
			TableRowGroup rowGroup = new TableRowGroup();
			TableRow row = new TableRow();
			rowGroup.Rows.Add(row);
			viewingTable.RowGroups.Add(rowGroup);

			viewingTable.Columns.Add(new TableColumn());
			viewingTable.Columns.Add(new TableColumn());
			viewingTable.Columns.Add(new TableColumn());
			viewingTable.Columns.Add(new TableColumn());
			viewingTable.Columns.Add(new TableColumn());
			viewingTable.Columns.Add(new TableColumn());
			viewingTable.Columns.Add(new TableColumn());
			viewingTable.Columns.Add(new TableColumn());

			long key = Helper.GetUniqueKey(type.Assembly, type);
			int numberOfBlankColumns = 0;

			row.Cells.Add(this.CreateColumn(new EntryKey(key, string.Empty), "Class"));
			if (type.GetConstructors().Count > 0) {
				row.Cells.Add(this.CreateColumn(new EntryKey(key, "Constructors"), "Constructors"));
			}
			else { numberOfBlankColumns++; }
			if (type.GetFields().Count > 0) {
				row.Cells.Add(this.CreateColumn(new EntryKey(key, "Fields"), "Fields"));
			}
			else { numberOfBlankColumns++; }
			if (true) {
				row.Cells.Add(this.CreateColumn(new EntryKey(key, "Members"), "Members"));
			}
			if (type.GetMethods().Count > 0) {
				row.Cells.Add(this.CreateColumn(new EntryKey(key, "Methods"), "Methods"));
			}
			else { numberOfBlankColumns++; }
			if (type.GetOperators().Count > 0) {
				row.Cells.Add(this.CreateColumn(new EntryKey(key, "Operators"), "Operators"));
			}
			else { numberOfBlankColumns++; }
			if (type.GetProperties().Count > 0) {
				row.Cells.Add(this.CreateColumn(new EntryKey(key, "Properties"), "Properties"));
			}
			else { numberOfBlankColumns++; }
			if (type.GetEvents().Count > 0) {
				row.Cells.Add(this.CreateColumn(new EntryKey(key, "Events"), "Events"));
			}
			else { numberOfBlankColumns++; }

			for (int i = 0; i < numberOfBlankColumns; i++) {
				row.Cells.Add(this.CreateBlankColumn());
			}
		}

		/// <summary>
		/// Creates a new column, populating it with a hyperlink correctly linked to the
		/// relevant page.
		/// </summary>
		/// <param name="entryKey">The entry key to allow pages to be linked.</param>
		/// <param name="displayName">The textual name to display for the link.</param>
		/// <returns>The created TableCell instance.</returns>
		private TableCell CreateColumn(EntryKey entryKey, string displayName) {
			Paragraph p = new Paragraph();
			Hyperlink link = new Hyperlink(new Run(displayName));
			link.Tag = entryKey;
			link.Click += this.linkResolver;
			p.Inlines.Add(link);
			return new TableCell(p);
		}

		/// <summary>
		/// Initialises a blank column.
		/// </summary>
		/// <returns>The blank column.</returns>
		private TableCell CreateBlankColumn() {
			Paragraph p = new Paragraph();
			return new TableCell(p);
		}
	}
}
