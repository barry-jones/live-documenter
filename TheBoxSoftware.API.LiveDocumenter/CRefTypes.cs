using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.API.LiveDocumenter {
	/// <summary>
	/// An enumeration of the available types of element that can be referenced
	/// by a cref path in the xml code comments for an assembly.
	/// </summary>
	public enum CRefTypes : byte {
		/// <summary>
		/// Represents a cref link to a namespace; these can not have commented directly
		/// but can be referenced.
		/// </summary>
		Namespace,

		/// <summary>
		/// Represents a path to a type defined in this or another assembly.
		/// </summary>
		Type,

		/// <summary>
		/// Represents a path to a property in a type.
		/// </summary>
		Property,

		/// <summary>
		/// Represents a fully qualified path to a field in a type.
		/// </summary>
		Field,

		/// <summary>
		/// Represents a fully qualified path to a method in a type.
		/// </summary>
		Method,

		/// <summary>
		/// Represents a fully qualified path to an event.
		/// </summary>
		Event,

		/// <summary>
		/// The compiler when generating the path could not resolve the type.
		/// </summary>
		Error
	}
}
