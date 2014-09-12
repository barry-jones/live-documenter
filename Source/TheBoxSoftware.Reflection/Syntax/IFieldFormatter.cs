using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	internal interface IFieldFormatter : IFormatter {
		SyntaxToken GetType(FieldSyntax syntax);
		List<SyntaxToken> GetVisibility(FieldSyntax syntax);
		SyntaxTokenCollection Format(FieldSyntax syntax);
	}
}
