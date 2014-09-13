using System;

namespace TheBoxSoftware.Documentation {
	/// <summary>
	/// Enumeration of the available document mappers.
	/// </summary>
	public enum Mappers {
		/// <summary>
		/// DocumentMapper that creates maps starting from the Assembly.
		/// </summary>
		AssemblyFirst,

		/// <summary>
		/// DocumentMaper that creates maps starting from the namespace.
		/// </summary>
		NamespaceFirst,

		GroupedNamespaceFirst
	}
}