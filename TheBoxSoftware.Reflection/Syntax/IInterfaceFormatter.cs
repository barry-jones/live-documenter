using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	public interface IInterfaceFormatter : IFormatter {
		/// <summary>
		/// Returns a formatted version of the interface visibility. Public, Private
		/// etc.
		/// </summary>
		/// <param name="syntax">The interface to format.</param>
		/// <returns>
		/// A string representing the visibility of the interface. For example C#
		/// implementation would return <c>public</c> for a public class.
		/// </returns>
		List<SyntaxToken> FormatVisibility(InterfaceSyntax syntax);

		/// <summary>
		/// Returns a formatted version of the interface base decleration.
		/// </summary>
		/// <param name="syntax">The ineterface to format.</param>
		/// <returns>
		/// A formatted interface base decleration. For example a C# implementation
		/// could return <c>: BaseClass, IInterface</c>.
		/// </returns>
		List<SyntaxToken> FormatInterfaceBase(InterfaceSyntax syntax);


		/// <summary>
		/// Returns the fully formatted interface.
		/// </summary>
		/// <param name="syntax">The class details to format.</param>
		/// <returns>A formatted class syntax decleration.</returns>
		/// <remarks>
		/// When implementing; this method will orchestrate the calling
		/// and formatting of the individual Format methods.
		/// </remarks>
		List<SyntaxToken> Format(InterfaceSyntax syntax);
	}
}
