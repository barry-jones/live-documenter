using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using TheBoxSoftware.Reflection.Comments;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// Manages the display of the permission elements in the documentation.
	/// </summary>
	internal class PermissionList : Section {
		private Table permissionTable;
		private TableRowGroup itemGroup;
		private int count = 0;

		/// <summary>
		/// Initialises a new PermissionList class.
		/// </summary>
		public PermissionList() {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);

			this.Blocks.Add(new Header2("Permissions"));

			this.permissionTable = new Table();
			TableRowGroup headerGroup = new TableRowGroup();
			headerGroup.Style = (Style)this.FindResource("TableHeader");
			TableRow row = new TableRow();
			row.Cells.Add(new TableCell(new Paragraph(new Run("Permission"))));
			row.Cells.Add(new TableCell(new Paragraph(new Run("Description"))));
			headerGroup.Rows.Add(row);

			this.permissionTable.RowGroups.Add(headerGroup);

			itemGroup = new TableRowGroup();
			this.permissionTable.RowGroups.Add(itemGroup);
			this.Blocks.Add(this.permissionTable);
		}

		/// <summary>
		/// Adds an individual permission to the permission list.
		/// </summary>
		/// <param name="permission">The individual permission to add.</param>
		public void Add(PermissionEntry permission) {
			TableRow row = new TableRow();
			row.Cells.Add(new TableCell(new Paragraph(permission.DisplayName)));

			Section s = new Section();
			s.Blocks.AddRange(permission.Description);

			row.Cells.Add(new TableCell(s));
			itemGroup.Rows.Add(row);
			this.count++;
		}

		/// <summary>
		/// Gets the number of permissions currently stored in this list.
		/// </summary>
		public int Count {
			get { return this.count; }
		}
	}
}
