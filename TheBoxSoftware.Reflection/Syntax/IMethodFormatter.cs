using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	// TODO: extern, new, modifiers

	public interface IMethodFormatter : IFormatter {
		List<SyntaxToken> FormatVisibility(MethodSyntax syntax);
		SyntaxToken FormatInheritance(MethodSyntax syntax);
		List<SyntaxToken> FormatParameters(MethodSyntax syntax);
		List<SyntaxToken> FormatReturnType(MethodSyntax syntax);
		List<SyntaxToken> Format(MethodSyntax syntax);
	}
}
