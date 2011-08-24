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
	/// Interaction logic for List.xaml
	/// </summary>
	public partial class List : Section {
		private System.Windows.Documents.List internalList = new System.Windows.Documents.List();

		#region Constructors
		public List() : this(ListTypes.None) {
			this.Initialise();
			this.Blocks.Add(internalList);
		}

		public List(Block title) : this(title, ListTypes.None) {
			this.Initialise();
			this.Blocks.Add(title);
			this.Blocks.Add(internalList);
		}

		private void Initialise() {
			this.InitializeComponent();
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
		}

		public List(ListTypes type) {
			switch (type) {
				case ListTypes.None:
					internalList = new System.Windows.Documents.List();
					break;
				case ListTypes.Unordered:
					internalList = new UnorderedList();
					break;
			}

			this.InitializeComponent();
			this.Blocks.Add(internalList);
		}

		public List(Block title, ListTypes type) {
			switch (type) {
				case ListTypes.None:
					internalList = new System.Windows.Documents.List();
					break;
				case ListTypes.Unordered:
					internalList = new UnorderedList();
					break;
			}

			this.InitializeComponent();
			this.Blocks.Add(title);
			this.Blocks.Add(internalList);
		}
		#endregion

		/// <summary>
		/// Adds a title to the list. If the list already has items then the title
		/// is added to the very beginning.
		/// </summary>
		/// <param name="title">The title to add</param>
		public void AddTitle(Block title) {
			if (this.Blocks.FirstBlock != null) {
				this.Blocks.InsertBefore(this.Blocks.FirstBlock, title);
			}
			else {
				this.Blocks.Add(title);
			}
		}

		public void AddListItem(string item) {
			this.internalList.ListItems.Add(new ListItem(new Paragraph(new Run(item))));
		}

		public void AddListItem(Hyperlink item) {
			this.internalList.ListItems.Add(new ListItem(new Paragraph(item)));
		}

		public List AddChildList(List child) {
			ListItem item = new ListItem();
			child.internalList.Style = (Style)this.FindResource("ChildList");
			item.Blocks.Add(child);
			this.internalList.ListItems.Add(item);
			return child;
		}

		internal System.Windows.Documents.List InternalList {
			get { return this.internalList; }
			set { this.internalList = value; }
		}
	}
}
