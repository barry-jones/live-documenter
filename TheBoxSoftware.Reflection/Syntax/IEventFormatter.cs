using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	internal interface IEventFormatter : IFormatter {
		SyntaxToken FormatIdentifier(EventSyntax syntax);
		List<SyntaxToken> FormatType(EventSyntax syntax);
		List<SyntaxToken> FormatVisibility(EventSyntax syntax);
		SyntaxToken FormatInheritance(EventSyntax syntax);
	}
}
