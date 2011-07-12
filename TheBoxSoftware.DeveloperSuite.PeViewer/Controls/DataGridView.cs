using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Controls {
	/// <summary>
	/// Event arguments for the ColumnStateChanged event
	/// </summary>
	public class ColumnStateChangedEventArgs : EventArgs {
		private ColumnStateChangeAction action;

		/// <summary>
		/// The action for the event
		/// </summary>
		public ColumnStateChangeAction Action {
			get { return action; }
			set { action = value; }
		}

		private DataGridViewColumn columnChanged;

		/// <summary>
		/// The column that has changed
		/// </summary>
		public DataGridViewColumn ColumnChanged {
			get { return columnChanged; }
			set { columnChanged = value; }
		}

		/// <summary>
		/// default ctor
		/// </summary>
		/// <param name="columnChanged">The column that has changed</param>
		/// <param name="action">The action for the event</param>
		public ColumnStateChangedEventArgs(DataGridViewColumn columnChanged, ColumnStateChangeAction action) {
			this.action = action;
			this.columnChanged = columnChanged;
		}
	}

	/// <summary>
	/// DataGridView is a control that can display a list of objects
	/// The data grid view show the data in a grid style. It generates the columns from the properties of the object in the current context of data
	/// You can use the DataGridViewPropertyDescriptorAttribute attribute to make your class properties interact better with the data grid
	/// </summary>
	public class DataGridView : ListView {
		//delegate used for the change notification of the column
		private PropertyChangedEventHandler gridViewColumnPropertyChanged;

		//flag that marks explicitly tells the grid view to not generate the column by reflection
		private bool useDefaultView;

		/// <summary>
		/// Gets or set the use of a default view
		/// Set this property to true if you want to specify your own GridView
		/// </summary>
		public bool UseDefaultView {
			get { return useDefaultView; }
			set { useDefaultView = value; }
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public DataGridView() {
			gridViewColumnPropertyChanged = new PropertyChangedEventHandler(DataGridViewPropertyChanged);
		}

		//event handler for the property changed event for the GridViewColumn
		void DataGridViewPropertyChanged(object sender, PropertyChangedEventArgs e) {
			DataGridViewColumn column = (DataGridViewColumn)sender;
			if (DataGridViewColumn.IsEnabledPropertyChanged(e))//check if the property changed is the enabled
            {
				OnColumnStateChanged(new ColumnStateChangedEventArgs(
					column, DataGridViewColumn.GetActionFromPropertyChanged(column, e)
					));
			}
		}

		/// <summary>
		/// Returns true if there are multiple items selected 
		/// </summary>
		public bool MultipleItemsSelected {
			get { return (bool)GetValue(MultipleItemsSelectedProperty); }
		}

		/// <summary>
		/// Returns true if there are multiple items selected 
		/// </summary>
		public static readonly DependencyProperty MultipleItemsSelectedProperty =
			DependencyProperty.Register("MultipleItemsSelected", typeof(bool), typeof(DataGridView), new UIPropertyMetadata(false));

		/// <summary>
		/// override the selection changed to change the multiple select property
		/// </summary>
		/// <param name="e">The event arguments passed</param>
		protected override void OnSelectionChanged(SelectionChangedEventArgs e) {
			SetValue(MultipleItemsSelectedProperty, (SelectedItems.Count > 1));
			base.OnSelectionChanged(e);
		}

		/// <summary>
		/// event raised to notify listeners that a column state has changed
		/// </summary>
		public event EventHandler<ColumnStateChangedEventArgs> ColumnStateChanged;

		/// <summary>
		/// raises the ColumnStateChanged event
		/// </summary>
		/// <param name="e">The event arguments for the event</param>
		protected virtual void OnColumnStateChanged(ColumnStateChangedEventArgs e) {
			if (ColumnStateChanged != null)
				ColumnStateChanged(this, e);
		}

		//flag to mark the grid view columns as generated for the new item source
		private bool hasGridViewBeenSet;

		/// <summary>
		/// Check that the grid view columns where generated for this type if not create them
		/// This usually occurs if data comes in late after binding...
		/// </summary>
		/// <param name="e">The arguments for the notification change of the binded collection</param>
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e) {
			//if the columns were not created, create them
			if (!hasGridViewBeenSet)
				GenerateGridViewColumnsForNewDataType((IList)e.NewItems);

			base.OnItemsChanged(e);
		}

		/// <summary>
		/// Repopulate the columns for the listview when the item source changes
		/// </summary>
		/// <param name="oldValue">The old items</param>
		/// <param name="newValue">The new items</param>
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue) {
			IList nValue = (IList)newValue;
			GenerateGridViewColumnsForNewDataType(nValue);
			base.OnItemsSourceChanged(oldValue, newValue);
		}

		//generate the columns for the grid view
		private void GenerateGridViewColumnsForNewDataType(IList newValues) {
			if (useDefaultView) // if there is no need to generate the column we can use the default view
				return;

			//check if the data passed is valid
			if (newValues == null || newValues.Count == 0) {
				//mark the gridview columns as not 
				hasGridViewBeenSet = false;
				return;
			}

			hasGridViewBeenSet = true;

			//Generate the grid view columns if needed
			GenerateGridColumns(newValues[0].GetType());
		}

		//generates the column for the view
		private void GenerateGridColumns(Type type) {
			//unregister to the old columns notifications
			if (!UseDefaultView && View != null) {
				GridView oldView = View as GridView;
				if (oldView != null)
					foreach (INotifyPropertyChanged column in oldView.Columns)
						column.PropertyChanged -= gridViewColumnPropertyChanged;
			}
			GridView gridView = new GridView();//grid view to display the columns

			IList<DataGridViewColumn> columnsToAdd = PropertyInfoEngine.GetProperties<DataGridViewColumn>(
				type, delegate(PropertyInfo property) {
					DataGridViewColumn column = null;
					//get the attribute
					DataGridViewPropertyDescriptorAttribute[] attributes = (DataGridViewPropertyDescriptorAttribute[])
						property.GetCustomAttributes(typeof(DataGridViewPropertyDescriptorAttribute), true);
					if (attributes != null && attributes.Length > 0) {
						if (attributes[0].Exclude)
							return null; //return null so that we exclude this column
						column = attributes[0].GetDataGridViewColumn();
					}
					else {
						column = new DataGridViewColumn(property.Name, property.Name);
						column.DisplayMemberBinding = new Binding(property.Name);
					}

					//register to the notifications
					((INotifyPropertyChanged)column).PropertyChanged += gridViewColumnPropertyChanged;
					return column;
				});

			//add all the columns to the new grid view, and remove the excluded
			for (int i = 0; i < columnsToAdd.Count; i++) {
				DataGridViewColumn column = columnsToAdd[i];
				if (column != null) {
					gridView.Columns.Add(column);
				}
				else {
					columnsToAdd.RemoveAt(i);
					i--;
				}
			}

			//position the columns properly
			for (int i = 0; i < columnsToAdd.Count; i++) {
				DataGridViewColumn columnToMove = columnsToAdd[i];
				if (DataGridViewColumn.IsValidPosition(columnToMove.DefaultPosition))
					gridView.Columns.Move(i, (columnToMove.DefaultPosition >= columnsToAdd.Count ? (columnsToAdd.Count - 1) : columnToMove.DefaultPosition));
			}

			View = gridView;
		}
	}
}
