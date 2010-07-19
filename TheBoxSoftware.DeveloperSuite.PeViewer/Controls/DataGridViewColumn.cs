using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Controls {
	/// <summary>
	/// Enum to state the action for the Coulmn State
	/// </summary>
	public enum ColumnStateChangeAction {
		/// <summary>
		/// columns is enabled
		/// </summary>
		Enabled,
		/// <summary>
		/// columns is disabled
		/// </summary>
		Disabled
	}

	/// <summary>
	/// Grid view column that can sort
	/// </summary>
	public class SortableGridViewColumn : GridViewColumn {
		/// <summary>
		/// default ctor
		/// </summary>
		public SortableGridViewColumn() {
			this.sortDirection = ListSortDirection.Ascending;
		}

		private ListSortDirection sortDirection;

		/// <summary>
		/// Gets or sets the current sort direction
		/// </summary>
		public ListSortDirection SortDirection {
			get { return sortDirection; }
		}

		/// <summary>
		/// Switches the sort direction
		/// </summary>
		public void SetSortDirection() {
			this.sortDirection = this.sortDirection == ListSortDirection.Ascending ?
				ListSortDirection.Descending : ListSortDirection.Ascending;
		}

		/// <summary>
		/// The property name of the column to be sorted
		/// </summary>
		public string SortPropertyName {
			get { return (string)GetValue(SortPropertyNameProperty); }
			set { SetValue(SortPropertyNameProperty, value); }
		}

		bool canSort = true;//this is used to not sort collections mainly

		/// <summary>
		/// Gets or sets a flag indicating if the column can sort
		/// </summary>
		public bool CanSort {
			get { return canSort; }
			set { canSort = value; }
		}

		/// <summary>
		/// The property name of the column to be sorted
		/// </summary>
		public static readonly DependencyProperty SortPropertyNameProperty =
			DependencyProperty.Register("SortPropertyName", typeof(string), typeof(DataGridViewColumn), new UIPropertyMetadata(""));
	}

	/// <summary>
	/// Extends the GridViewColumn to include some extra features
	/// </summary>
	public class DataGridViewColumn : SortableGridViewColumn {
		double lastWidth;
		const double defaultWidth = 50;

		/// <summary>
		/// Default ctor
		/// </summary>
		public DataGridViewColumn() {
			SetLastWidth();
		}

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="name">The header name to set</param>
		/// <param name="sortName">The sort property name to set</param>
		public DataGridViewColumn(string name, string sortName)
			: this() {
			this.Header = name;
			this.SortPropertyName = sortName;
		}

		int defaultPosition = -1;
		/// <summary>
		/// Gets or sets the default position(index) where to place the column
		/// </summary>
		public int DefaultPosition {
			get {
				return defaultPosition;
			}
			set {
				defaultPosition = value;
			}
		}

		//sets the last width 
		private void SetLastWidth() {
			this.lastWidth = (double.IsNaN(Width) || Width == 0.0)
				? (ActualWidth == 0.0 ? defaultWidth : ActualWidth) : Width;
		}

		private object tag;

		/// <summary>
		/// Gets or sets the tag for this object
		/// </summary>
		public object Tag {
			get { return tag; }
			set { tag = value; }
		}

		/// <summary>
		/// a dependancy property to know whether to display the column in the ListView
		/// </summary>
		public static readonly DependencyProperty IsEnabledProperty =
			DependencyProperty.Register("IsEnabled", typeof(bool), typeof(DataGridViewColumn), new PropertyMetadata(true, new PropertyChangedCallback(propertyChanged)));

		//raises the property changed event
		static void propertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			DataGridViewColumn column = d as DataGridViewColumn;
			column.HideShowColumn((bool)e.NewValue);
			column.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(e.Property.Name));
		}

		/// <summary>
		/// Dependancy property to know whether to display the column in the ListView
		/// </summary>
		public bool IsEnabled {
			get { return (bool)GetValue(IsEnabledProperty); }
			set {
				//don't set the value if it is the same. this is important since on startup it could set the width as 0 for the columns
				if (value == IsEnabled)
					return;
				SetValue(IsEnabledProperty, value);
				HideShowColumn(value);
				OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("IsEnabled"));
			}
		}

		//hides / shows the gridview column
		private void HideShowColumn(bool enabled) {
			if (enabled)
				this.Width = lastWidth;
			else {
				SetLastWidth();
				this.Width = 0;
			}
		}

		/// <summary>
		/// Checks if the property changed is the IsEnabled propety
		/// </summary>
		/// <param name="e">The event arguments to verify</param>
		/// <returns>Returns true if the property is the IsEnabled</returns>
		public static bool IsEnabledPropertyChanged(PropertyChangedEventArgs e) {
			if (e == null)
				return false;

			return e.PropertyName == "IsEnabled";
		}

		/// <summary>
		/// Returns a ColumnStateChangeAction that describes the property changed event
		/// </summary>
		/// <param name="sender">The DataGridViewColumn that has raised the event</param>
		/// <param name="e">The event argmunets to parse</param>
		/// <returns>Returns a ColumnStateChangeAction to represent the change of state</returns>
		/// <exception cref="ArgumentNullException">Throws ArgumentNullException if the sender or the arguments are not valid</exception>
		public static ColumnStateChangeAction GetActionFromPropertyChanged(DataGridViewColumn sender, PropertyChangedEventArgs e) {
			if (sender != null && IsEnabledPropertyChanged(e))
				return sender.IsEnabled ? ColumnStateChangeAction.Enabled : ColumnStateChangeAction.Disabled;

			throw new ArgumentNullException("e");
		}

		/// <summary>
		/// Gets the identifier for a specific column
		/// </summary>
		/// <param name="column">The column to get the identifier from</param>
		/// <returns>Returns an identifier for the column</returns>
		public static string GetColumnIdentifier(GridViewColumn column) {
			return column == null ? "" : column.Header.ToString();
		}

		/// <summary>
		/// Gets a flag indicating if the position of the column is valid
		/// </summary>
		/// <param name="position">The position to check</param>
		/// <returns>Returns true if position is valid</returns>
		public static bool IsValidPosition(int position) {
			return position > -1;
		}
	}
}
