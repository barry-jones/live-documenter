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
	/// Interaction logic for ExceptionList.xaml
	/// </summary>
	public partial class ExceptionList : Section {
		/// <summary>
		/// The table to contain the exceptions.
		/// </summary>
		private Table exceptionTable;
		/// <summary>
		/// The row group which is a container for the rows containing the
		/// exception details.
		/// </summary>
		private TableRowGroup itemGroup;
		/// <summary>
		/// The number of exceptions currently stored in this list.
		/// </summary>
		private int exceptionCount = 0;

		/// <summary>
		/// Initialises a new instance of the ExceptionList class.
		/// </summary>
		/// <remarks>
		/// This initialises the class and its basic structure, including
		/// the table and header that the exceptions will be contained
		/// within.
		/// </remarks>
		public ExceptionList() {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
			InitializeComponent();

			this.Blocks.Add(new Header2("Exceptions"));

			exceptionTable = new Table();
			TableRowGroup headerGroup = new TableRowGroup();
			TableRow row = new TableRow();
			row.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Exception")))));
			row.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Condition")))));
			headerGroup.Rows.Add(row);

			this.exceptionTable.RowGroups.Add(headerGroup);

			itemGroup = new TableRowGroup();
			this.exceptionTable.RowGroups.Add(itemGroup);
			this.Blocks.Add(this.exceptionTable);
		}

		/// <summary>
		/// Adds a range of exceptions to the exception list.
		/// </summary>
		/// <param name="exceptions">The collection of exceptions to add.</param>
		public void AddRange(List<ExceptionEntry> exceptions) {
			foreach (ExceptionEntry current in exceptions) {
				TableRow row = new TableRow();
				row.Cells.Add(new TableCell(new Paragraph(current.Link)));

				Section s = new Section();
				s.Blocks.AddRange(current.Description);

				row.Cells.Add(new TableCell(s));
				itemGroup.Rows.Add(row);
				this.exceptionCount++;
			}
		}

		/// <summary>
		/// Adds an individual exception to the exception list.
		/// </summary>
		/// <param name="exception">The individual exception to add.</param>
		public void Add(ExceptionEntry exception) {
			TableRow row = new TableRow();
			row.Cells.Add(new TableCell(new Paragraph(exception.Link)));

			Section s = new Section();
			s.Blocks.AddRange(exception.Description);

			row.Cells.Add(new TableCell(s));
			itemGroup.Rows.Add(row);
			this.exceptionCount++;
		}

		/// <summary>
		/// Gets the number of exceptions currently stored in this list.
		/// </summary>
		public int ExceptionCount {
			get { return this.exceptionCount; }
		}
	}
}
