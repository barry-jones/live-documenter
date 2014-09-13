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
	/// <summary>
	/// Interaction logic for TypeParamSection.xaml
	/// </summary>
	public partial class TypeParamSection : Section {
		private Table exceptionTable;
		private TableRowGroup itemGroup;

		public TypeParamSection() {
			this.InitializeComponent();
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
			this.Blocks.Add(new Header3("Type Parameters"));

			exceptionTable = new Table();
			TableRowGroup headerGroup = new TableRowGroup();
			TableRow row = new TableRow();
			row.Cells.Add(new TableCell(new Paragraph(new Run("Name"))));
			row.Cells.Add(new TableCell(new Paragraph(new Run("Description"))));
			headerGroup.Rows.Add(row);

			this.exceptionTable.RowGroups.Add(headerGroup);

			itemGroup = new TableRowGroup();
			this.exceptionTable.RowGroups.Add(itemGroup);
			this.Blocks.Add(this.exceptionTable);
		}

		public void AddEntry(TypeParamEntry entry) {
			TableRow row = new TableRow();
			row.Cells.Add(new TableCell(new Paragraph(new Italic(new Run(entry.Param)))));
			row.Cells.Add(new TableCell(new Paragraph(new Run(entry.Description))));
			itemGroup.Rows.Add(row);
		}
	}
}
