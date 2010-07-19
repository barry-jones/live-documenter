using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Controls {
	/// <summary>
	/// The behevoiur to have for a collection
	/// </summary>
	public enum CollectionBehaviour {
		/// <summary>
		/// Does not use this field
		/// </summary>
		None,
		/// <summary>
		/// Gets ths last value added in the collection
		/// </summary>
		LastValue,
		/// <summary>
		/// Gets the count of the collection
		/// </summary>
		Count
	}

	/// <summary>
	/// Attribute to describe the data grid column fo a specific property
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class DataGridViewPropertyDescriptorAttribute : Attribute {
		private CollectionBehaviour collectionBehaviour = CollectionBehaviour.None;
		/// <summary>
		/// Gets or sets the collection Behaviour
		/// </summary>
		public CollectionBehaviour CollectionBehaviour {
			get {
				return this.collectionBehaviour;
			}
			set {
				this.collectionBehaviour = value;
			}
		}

		private bool exclude = false;
		/// <summary>
		/// gets or sets a flag that marks the property to be excluded
		/// </summary>
		public bool Exclude {
			get {
				return exclude;
			}
			set {
				exclude = value;
			}
		}

		int position = -1;
		/// <summary>
		/// Gets or sets the position(index) where to place the column
		/// </summary>
		public int Position {
			get {
				return position;
			}
			set {
				position = value;
			}
		}

		string displayName;
		/// <summary>
		/// Display Name used for the data grid view column
		/// </summary>
		public string DisplayName {
			get {
				return displayName;
			}
			set {
				displayName = value;
			}
		}

		Binding memeberBinding;
		/// <summary>
		/// Display Name used for the data grid view column
		/// </summary>
		internal Binding MemeberBinding {
			get {
				return memeberBinding;
			}
		}

		string sortName;
		/// <summary>
		/// Sort name used for sorting on the column
		/// </summary>
		public string SortName {
			get {
				return sortName;
			}
			set {
				displayName = value;
			}
		}

		/// <summary>
		/// Full constructor
		/// </summary>
		/// <param name="bindingPath">The binding path for the DisplayMemeberValue</param>
		/// <param name="displayName">The display name to use for the datagrid</param>
		/// <param name="sortName">The sort name to use to sort the data</param>
		public DataGridViewPropertyDescriptorAttribute(string bindingPath, string displayName, string sortName) {
			this.memeberBinding = new Binding(bindingPath);
			this.displayName = displayName;
			this.sortName = sortName;
		}

		/// <summary>
		/// Ctor used when one wants to mark a property to be excluded 
		/// from the auto generation of data grid viewl columns
		/// </summary>
		/// <param name="exclude">Set to true to exclude property</param>
		public DataGridViewPropertyDescriptorAttribute(bool exclude) {
			this.exclude = exclude;
		}

		/// <summary>
		/// returns a datagrid view column with all the settings applied
		/// </summary>
		/// <returns>datagrid view column</returns>
		/// <exception cref="NotSupportedException">Thrown when the exclude flag has been marked as true</exception>
		internal DataGridViewColumn GetDataGridViewColumn() {
			if (exclude)
				throw new NotSupportedException("Cannot call GetDataGridViewColumn if Exclude has been marked as true");

			DataGridViewColumn column = new DataGridViewColumn(DisplayName, SortName);

			//Collection behaviour
			IValueConverter converter = null; //TODO IMPLEMENT CUSTOM STRATEGY
			if (collectionBehaviour != CollectionBehaviour.None) {
				converter = CollectionNotificationManager.RegisterCollectionNotification(collectionBehaviour);
				memeberBinding.Converter = converter;
				column.CanSort = false;
			}

			column.DisplayMemberBinding = memeberBinding;
			column.DefaultPosition = position;
			return column;
		}
	}
}
