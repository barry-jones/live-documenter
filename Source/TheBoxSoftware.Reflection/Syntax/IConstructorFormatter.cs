using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	internal interface IConstructorFormatter : IFormatter {
		List<SyntaxToken> FormatVisibility(ConstructorSyntax syntax);
		SyntaxToken FormatInheritance(ConstructorSyntax syntax);
		List<SyntaxToken> FormatParameters(ConstructorSyntax syntax);
		SyntaxTokenCollection Format(ConstructorSyntax syntax);
	}
}
