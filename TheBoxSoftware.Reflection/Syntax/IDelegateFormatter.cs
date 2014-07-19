using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	internal interface IDelegateFormatter : IFormatter {
		List<SyntaxToken> FormatVisibility(DelegateSyntax syntax);
		List<SyntaxToken> FormatReturnType(DelegateSyntax syntax);
		List<SyntaxToken> FormatParameters(MethodSyntax syntax);
		SyntaxTokenCollection Format(DelegateSyntax syntax);
	}
}
