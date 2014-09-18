using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.API.LiveDocumenter {
	/// <summary>
	/// Set of constants that define all of the different cref identifiers
	/// that can be used in code comments.
	/// </summary>
	internal static class CRefConstants 
    {
		/// <summary>
		/// Constant that defines a field reference.
		/// </summary>
		public const string FieldTypeIndicator = "F";
		/// <summary>
		/// Constant that defines a property reference.
		/// </summary>
		public const string PropertyTypeIndicator = "P";
		/// <summary>
		/// Constant that defines a method reference.
		/// </summary>
		public const string MethodTypeIndicator = "M";
		/// <summary>
		/// Constant that defines a type reference.
		/// </summary>
		public const string TypeIndicator = "T";
		/// <summary>
		/// Constant that defines a namespace reference.
		/// </summary>
		public const string NamespaceTypeIndicator = "N";
		/// <summary>
		/// Constant that defines an event reference.
		/// </summary>
		public const string EventTypeIndicator = "E";
		/// <summary>
		/// Constant that defines a resolution error reference.
		/// </summary>
		public const string ErrorTypeIndicator = "!";

		/// <summary>
		/// Returns the string indicator for the specified CRef type.
		/// </summary>
		/// <param name="type">The type to return the indicator for.</param>
		/// <returns>The indicator.</returns>
		/// <exception cref="NotImplementedException">
		/// The provided constant in the enumeration <paramref name="type"/> has
		/// not been implemented.
		/// </exception>
		public static string GetIndicatorFor(CRefTypes type)
        {
			switch (type)
            {
				case CRefTypes.Field: return CRefConstants.FieldTypeIndicator;
				case CRefTypes.Method: return CRefConstants.MethodTypeIndicator;
				case CRefTypes.Property: return CRefConstants.PropertyTypeIndicator;
				case CRefTypes.Type: return CRefConstants.TypeIndicator;
				case CRefTypes.Event: return CRefConstants.EventTypeIndicator;
				case CRefTypes.Namespace: return CRefConstants.NamespaceTypeIndicator;
				case CRefTypes.Error: return CRefConstants.ErrorTypeIndicator;
				default:
					NotImplementedException ex = new NotImplementedException();
					ex.Data["type"] = type;
					throw ex;
			}
		}
	}
}
