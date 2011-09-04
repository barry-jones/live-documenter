using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	/// <summary>
	/// Interface defining a template for a formatters for class syntax.
	/// </summary>
	public interface IClassFormatter : IFormatter {
		/// <summary>
		/// Returns a formatted version of the class visibility. Public, Private
		/// etc.
		/// </summary>
		/// <param name="syntax">The class to format.</param>
		/// <returns>
		/// A string representing the visibility of the class. For example C#
		/// implementation would return <c>public</c> for a public class.
		/// </returns>
		List<SyntaxToken> FormatVisibility(ClassSyntax syntax);

		/// <summary>
		/// Returns a formatted version of the class inheritance modifier. Abstract,
		/// Sealed etc.
		/// </summary>
		/// <param name="syntax">The class to format.</param>
		/// <returns>
		/// A string representing the formatted inheritance for the class. For
		/// example C# implementation would return <c>abstract</c> for an
		/// abstract class.
		/// </returns>
		SyntaxToken FormatInheritance(ClassSyntax syntax);

		/// <summary>
		/// Returns a formatted version of the class base decleration.
		/// </summary>
		/// <param name="syntax">The class to format.</param>
		/// <returns>
		/// A formatted class base decleration. For example a C# implementation
		/// could return <c>: BaseClass, IInterface</c>.
		/// </returns>
		List<SyntaxToken> FormatClassBase(ClassSyntax syntax);

		/// <summary>
		/// Returns the fully formatted class.
		/// </summary>
		/// <param name="syntax">The class details to format.</param>
		/// <returns>A formatted class syntax decleration.</returns>
		/// <remarks>
		/// When implementing; this method will orchestrate the calling
		/// and formatting of the individual Format methods.
		/// </remarks>
		List<SyntaxToken> Format(ClassSyntax syntax);
	}
}
