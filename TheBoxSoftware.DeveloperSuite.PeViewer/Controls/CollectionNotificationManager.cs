using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Collections.Specialized;
using System.Collections;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Controls {
	/// <summary>
	/// Strategy interface to get data
	/// </summary>
	public interface ICollectionNotificationStrategy {
		/// <summary>
		/// Get the Display object to show on screen
		/// </summary>
		/// <param name="source">The source collection to get data from</param>
		/// <returns>The object to show on screen</returns>
		object GetDisplayMember(IList source);
	}

	/// <summary>
	/// Gets the last value from a collection
	/// </summary>
	public class CollectionLastValueNotificationStrategy : ICollectionNotificationStrategy {
		#region ICollectionNotificationStrategy Members

		/// <summary>
		/// Gets the last value from the collection
		/// </summary>
		/// <param name="source">The source collection to get the value from</param>
		/// <returns>Return the object to display on screen</returns>
		/// <exception cref="ArgumentNullException">Thrown when the source list is set to null</exception>
		public object GetDisplayMember(IList source) {
			if (source == null)
				throw new ArgumentNullException("source", "The source collection was set to null");
			return source[source.Count - 1];
		}

		#endregion
	}

	/// <summary>
	/// Gets the count of the collection to display on screen
	/// </summary>
	public class CollectionCountNotificationStrategy : ICollectionNotificationStrategy {
		#region ICollectionNotificationStrategy Members

		/// <summary>
		/// gets the count of the collection
		/// </summary>
		/// <param name="source">The source collection to get the count from</param>
		/// <returns>The count of the collection</returns>
		/// <exception cref="ArgumentNullException">Thrown when the source list is set to null</exception>
		public object GetDisplayMember(IList source) {
			if (source == null)
				throw new ArgumentNullException("source", "The source collection was set to null");
			return source.Count;
		}

		#endregion
	}

	/// <summary>
	/// Notification listener for a collection that implement INotifyCollectionChanged
	/// </summary>
	public class CollectionNotificationManager {
		/// <summary>
		/// creates a class that is registered to notification of INotifyCollectionChanged
		/// </summary>
		/// <param name="collectionBehaviour">The behavoir (return value) for the listnener</param>
		/// <returns>Return a listener instance that is registered to notifications</returns>
		public static IValueConverter RegisterCollectionNotification(CollectionBehaviour collectionBehaviour) {
			switch (collectionBehaviour) {
				case CollectionBehaviour.LastValue:
					return new CollectionNotifier(new CollectionLastValueNotificationStrategy());
				case CollectionBehaviour.Count:
					return new CollectionNotifier(new CollectionCountNotificationStrategy());
				default:
					break;
			}
			return null;
		}
	}

	/// <summary>
	/// Converter used to translate the collection property to a value set by the strategy specified
	/// </summary>
	public class CollectionNotifier : IValueConverter {
		ICollectionNotificationStrategy strategy;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="strategy">The strategy to use to convert data</param>
		public CollectionNotifier(ICollectionNotificationStrategy strategy) {
			this.strategy = strategy;
		}

		#region IValueConverter Members

		/// <summary>
		/// Converts the source collection to return the value specified by the strategy
		/// </summary>
		/// <param name="value">The collection value</param>
		/// <param name="targetType">The target type is not used in this context</param>
		/// <param name="parameter">The parameter is not used in this context</param>
		/// <param name="culture">The culture is not used in this context</param>
		/// <returns>Returns the value specified by the strategy</returns>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			IList source = value as IList;
			if (source == null)
				throw new NotSupportedException("Only IList is supported for collection behavior");
			return strategy.GetDisplayMember(source);
		}

		/// <summary>
		/// Converts the value back to the original value. Used for two way databinding
		/// This is not supported
		/// </summary>
		/// <param name="value">The value represented in the UI</param>
		/// <param name="targetType">The target type is not used in this context</param>
		/// <param name="parameter">The parameter type is not used in this context</param>
		/// <param name="culture">The culture type is not used in this context</param>
		/// <returns>Return the value of the collection</returns>
		///<exception cref="NotSupportedException">Thrown when trying to use two way databinding</exception>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			throw new NotSupportedException("Two way data binding is not supported for collection binding");
		}

		#endregion
	}
}
