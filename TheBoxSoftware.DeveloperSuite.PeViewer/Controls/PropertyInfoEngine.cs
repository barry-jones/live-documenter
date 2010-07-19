using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace TheBoxSoftware.DeveloperSuite.PEViewer.Controls {
	/// <summary>
	/// Creates an instance of an object from the property info specified
	/// </summary>
	/// <typeparam name="T">The Type to create</typeparam>
	/// <param name="propertyInfo">The property info to create the type</param>
	/// <returns>Return the object constructed from the property info</returns>
	public delegate T CreateUiPresenter<T>(PropertyInfo propertyInfo);

	/// <summary>
	/// Helper class that get a list of property from a specific type
	/// </summary>
	public class PropertyInfoEngine {
		/// <summary>
		/// Create a list of ui element from a specific type
		/// </summary>
		/// <typeparam name="T">The type of objects to create</typeparam>
		/// <param name="type">The type to analyze</param>
		/// <param name="createUiPresenter">a delegate that creates an instance of ui elemet from a property info</param>
		/// <returns>Returns a list of UI element objects</returns>
		public static IList<T> GetProperties<T>(Type type, CreateUiPresenter<T> createUiPresenter) {
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			if (properties != null && properties.Length > 0) {
				IList<T> validProperties = new List<T>(properties.Length);
				foreach (PropertyInfo prop in properties)
					if (prop.CanRead)
						validProperties.Add(createUiPresenter(prop));
				return validProperties;
			}
			return new T[0];
		}

		/// <summary>
		/// Gets a list of instance properties that have a getter
		/// </summary>
		/// <param name="type">The type to analze</param>
		/// <returns>Returns a list of properties</returns>
		public static IList<PropertyInfo> GetProperties(Type type) {
			return GetProperties<PropertyInfo>(type, delegate(PropertyInfo prop) { return prop; });
		}
	}
}
