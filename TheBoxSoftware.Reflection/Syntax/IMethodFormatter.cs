using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	// TODO: extern, new, modifiers

	internal interface IMethodFormatter : IFormatter {
		List<SyntaxToken> FormatVisibility(MethodSyntax syntax);
		SyntaxToken FormatInheritance(MethodSyntax syntax);
		List<SyntaxToken> FormatParameters(MethodSyntax syntax);
		List<SyntaxToken> FormatReturnType(MethodSyntax syntax);
		SyntaxTokenCollection Format(MethodSyntax syntax);
	}
}
