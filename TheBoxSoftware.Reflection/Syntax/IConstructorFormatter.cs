using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	public interface IConstructorFormatter : IFormatter {
		List<SyntaxToken> FormatVisibility(ConstructorSyntax syntax);
		SyntaxToken FormatInheritance(ConstructorSyntax syntax);
		List<SyntaxToken> FormatParameters(ConstructorSyntax syntax);
		List<SyntaxToken> Format(ConstructorSyntax syntax);
	}
}
