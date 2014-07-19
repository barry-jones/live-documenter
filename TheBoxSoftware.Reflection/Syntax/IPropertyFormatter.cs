using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	internal interface IPropertyFormatter : IFormatter {
		SyntaxToken FormatIdentifier(PropertySyntax syntax);
		List<SyntaxToken> FormatType(PropertySyntax syntax);
		List<SyntaxToken> FormatVisibility(PropertySyntax syntax);
		SyntaxToken FormatInheritance(PropertySyntax syntax);
		List<SyntaxToken> FormatGetVisibility(PropertySyntax syntax);
		List<SyntaxToken> FormatSetVisibility(PropertySyntax syntax);
		SyntaxTokenCollection Format(PropertySyntax syntax);
	}
}
