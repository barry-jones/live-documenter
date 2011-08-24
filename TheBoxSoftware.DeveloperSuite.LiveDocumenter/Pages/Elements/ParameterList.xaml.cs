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
	/// Interaction logic for ParameterList.xaml
	/// </summary>
	public partial class ParameterList : Section {
		private Table exceptionTable;
		private TableRowGroup itemGroup;

		/// <summary>
		/// Initialises a new instance of a ParameterList class.
		/// </summary>
		public ParameterList() {
			this.InitializeComponent();
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);

			this.Blocks.Add(new Header3("Parameters"));

			exceptionTable = new Table();
			TableRowGroup headerGroup = new TableRowGroup();
			TableRow row = new TableRow();
			row.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Name")))));
			row.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Type")))));
			row.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Description")))));
			headerGroup.Rows.Add(row);

			this.exceptionTable.RowGroups.Add(headerGroup);

			itemGroup = new TableRowGroup();
			this.exceptionTable.RowGroups.Add(itemGroup);
			this.Blocks.Add(this.exceptionTable);
		}

		/// <summary>
		/// Adds another exception entry to the list of exceptions displayed in this exception
		/// list.
		/// </summary>
		/// <param name="exceptionNode">The node containing the details of the exception.</param>
		public void Add(string name, string typeName, AssemblyDef assembly, EntryKey typeKey, List<Block> description) {
			Inline type = null;
			if (typeKey != null) {
				type = new Hyperlink(new Run(typeName));
				((Hyperlink)type).Tag = typeKey;
				((Hyperlink)type).Click += new System.Windows.RoutedEventHandler(LinkHelper.Resolve);
			}
			else {
				type = new Run(typeName);
			}

			TableRow row = new TableRow();
			row.Cells.Add(new TableCell(new Paragraph(new Italic(new Run(name)))));
			row.Cells.Add(new TableCell(new Paragraph(type)));
			Section descSection = new Section();
			descSection.Blocks.AddRange(description);
			row.Cells.Add(new TableCell(descSection));
			itemGroup.Rows.Add(row);
		}
	}
}
