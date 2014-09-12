using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	internal interface IStructFormatter : IFormatter {
		List<SyntaxToken> FormatVisibility(StructSyntax syntax);
		List<SyntaxToken> FormatInterfaces(StructSyntax syntax);
		SyntaxTokenCollection Format(StructSyntax syntax);
	}
}
