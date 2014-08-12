using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model {
	using TheBoxSoftware.Documentation;

	/// <summary>
	/// Class that manages the history of the users viewing activities. This will
	/// record any page that the user visits and allow them to navigate their history
	/// when it suits them.
	/// </summary>
	internal sealed class UserViewingHistory : INotifyPropertyChanged {
		private const int MaxSize = 10;
		private List<Entry> viewedEntries = new List<Entry>();
		private bool canMoveBackward = false;
		private bool canMoveForward = false;
		private int currentPositionInHistory;

		/// <summary>
		/// Adds a new entry to the viewing history.
		/// </summary>
		/// <param name="viewedEntry">The entry to retain in the history.</param>
		public void Add(Entry viewedEntry) {
			bool currentIsViewed = this.Current != null && this.Current == viewedEntry;

			if (this.Current != null && !currentIsViewed) {
				if (viewedEntries[viewedEntries.Count - 1] != this.Current) {
					// Clear the list in front of the current element so we can continue
					// the viewers history without it getting complicated with branches
					int indexInfront = this.currentPositionInHistory + 1;
					Entry nextEntry = this.viewedEntries[indexInfront];
					while (nextEntry != null) {
						this.viewedEntries.RemoveAt(indexInfront);
						if (indexInfront < this.viewedEntries.Count) {
							nextEntry = this.viewedEntries[indexInfront];
						}
						else {
							nextEntry = null;
						}
					}
				}
			}
			if (!currentIsViewed) {
				// Check the history is not too long
				if (this.viewedEntries.Count >= MaxSize) {
					this.viewedEntries.RemoveAt(0);
				}
				this.viewedEntries.Add(viewedEntry);
				this.currentPositionInHistory = this.viewedEntries.Count - 1;	// Move the cursor to the end of the list
				this.Current = viewedEntry;
			}
			this.CanMoveForward = this.IndexOfCurrent() < this.viewedEntries.Count - 1;
			this.CanMoveBackward = this.IndexOfCurrent() > 0;
		}

		/// <summary>
		/// Clears the current history for the current document. This should be called
		/// when the element being documented has been changed.
		/// </summary>
		public void ClearHistory() {
			this.currentPositionInHistory = 0;
			this.viewedEntries.Clear();
			this.Current = null;
			this.CanMoveBackward = false;
			this.CanMoveForward = false;
		}

		/// <summary>
		/// Moves the user to the next page in their viewing history
		/// </summary>
		public void MoveForward() {
			if (this.CanMoveForward) {
				if ((this.currentPositionInHistory + 1) >= this.viewedEntries.Count) {
					throw new InvalidOperationException("Can not move any further forward, no history available");
				}
				this.Current = this.viewedEntries[this.currentPositionInHistory + 1];
				this.currentPositionInHistory++; // Move to the next item so we can check capability
				this.CanMoveBackward = this.IndexOfCurrent() > 0;
				this.CanMoveForward = this.IndexOfCurrent() < (this.viewedEntries.Count - 1);

				this.Current.IsSelected = true;
				this.Current.IsExpanded = true;
			}
		}

		/// <summary>
		/// Moves the user to the last page in their viewing history.
		/// </summary>
		public void MoveBack() {
			if (this.CanMoveBackward) {
				if ((this.currentPositionInHistory - 1) < 0) {
					throw new InvalidOperationException("Can not move any further back, no history available");
				}
				this.Current = this.viewedEntries[this.currentPositionInHistory - 1];
				this.currentPositionInHistory--;	// Move to the previous item so we can chcekc capability
				this.CanMoveBackward = this.IndexOfCurrent() > 0;
				this.CanMoveForward = this.IndexOfCurrent() < (this.viewedEntries.Count - 1);

				this.Current.IsSelected = true;
				this.Current.IsExpanded = true;
			}
		}

		/// <summary>
		/// Obtains the index in the history of the users current position
		/// </summary>
		/// <returns>An integer representing the current position of the currently selected entry.</returns>
		private int IndexOfCurrent() {
			return this.currentPositionInHistory;
		}

		/// <summary>
		/// The current position in the history for the user.
		/// </summary>
		/// <remarks>
		/// When the current position is not the last element in the que
		/// any newly added elements will be inserted after this and any others
		/// in front will dissapear.
		/// </remarks>
		public Entry Current { 
			get; 
			set; 
		}

		/// <summary>
		/// Indicates if the user has enough history information to be able
		/// to travel forwards.
		/// </summary>
		public bool CanMoveForward {
			get { return this.canMoveForward; }
			set {
				if (this.canMoveForward != value) {
					this.canMoveForward = value;
					this.OnPropertyChanged("CanMoveForward");
				}
			}
		}

		/// <summary>
		/// Indicates if the user has enough history information to be able
		/// to travel backwards.
		/// </summary>
		public bool CanMoveBackward {
			get {
				return this.canMoveBackward;
			}
			set {
				if (this.canMoveBackward != value) {
					this.canMoveBackward = value;
					this.OnPropertyChanged("CanMoveBackward");
				}
			}
		}


		#region INotifyPropertyChanged Members
		/// <summary>
		/// PropertyChanged event handler. Fires when an interesting property in this
		/// class has been changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Notification event for interesting property changes in this class,
		/// helps to link the model to the view.
		/// </summary>
		/// <param name="propertyName">The name of the property that has changed.</param>
		private void OnPropertyChanged(string propertyName) {
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion

		public bool CanExecuteCommand(object sender, CanExecuteRoutedEventArgs e) {
			if (e.Command == NavigationCommands.BrowseForward) {
				e.CanExecute = this.CanMoveForward;
			}
			else if (e.Command == NavigationCommands.BrowseBack) {
				e.CanExecute = this.CanMoveBackward;
			}
			else {
				e.CanExecute = false;
			}

			return e.CanExecute;
		}

		public void ExecuteCommand(object sender, ExecutedRoutedEventArgs e) {
			if (e.Command == NavigationCommands.BrowseBack) {
				this.MoveBack();
			}
			else if (e.Command == NavigationCommands.BrowseForward) {
				this.MoveForward();
			}
		}
	}
}
