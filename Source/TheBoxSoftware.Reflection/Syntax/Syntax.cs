using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	/// <summary>
	/// A base class for all Syntax classes that provides useful methods and
	/// properties for managing the required syntax information for concrete
	/// implementations.
	/// </summary>
	internal abstract class Syntax {
		/// <summary>
		/// Obtains the name of the type provided. This class will remove any
		/// superflous characters from the name to return the name that the user
		/// will understand and had entered when creating the type.
		/// </summary>
		/// <param name="type">The type to get the name of.</param>
		/// <returns>A string representing the name of the type.</returns>
		/// <remarks>
		/// .NET Framework will add special characters to names to allow, for example,
		/// generic methods to be overloaded. These characeters are removed in this
		/// method and the user defined name of the type is returned.
		/// <example>
		/// // User creates type
		/// public class MyGenericType&lt;T&gt;
		/// // Framework outputs
		/// MyGenericType`1
		/// // Method returns
		/// MyGenericType
		/// </example>
		/// </remarks>
		protected string GetTypeName(TypeRef type) {
			string name = string.Empty;
			name = type.Name;
			if (type.IsGeneric) {
				int count = int.Parse(name.Substring(name.IndexOf('`') + 1));
				name = name.Substring(0, name.IndexOf('`'));
			}
			return name;
		}
	}
}
