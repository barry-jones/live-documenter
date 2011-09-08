using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	/// <summary>
	/// Interface defining a simple method that allows callers
	/// of implementing classes access to an definition not an
	/// implementation of a formatter.
	/// </summary>
	public interface IFormatter {
		/// <summary>
		/// Method to return the format of a specific <see cref="Syntax"/>
		/// class.
		/// </summary>
		/// <returns>The list of formatted tokens</returns>
		/// <remarks>
		/// Implementors should, when implementing this method, call the
		/// more strongly typed IFormatter interfaces format method while
		/// passing in the private Syntax reference.
		/// </remarks>
		SyntaxTokenCollection Format();
	}
}
