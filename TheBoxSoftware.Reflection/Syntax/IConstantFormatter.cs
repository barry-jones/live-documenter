using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	public interface IConstantFormatter : IFormatter {
		SyntaxToken GetType(ConstantSyntax syntax);
		List<SyntaxToken> GetVisibility(ConstantSyntax syntax);
		List<SyntaxToken> Format(ConstantSyntax syntax);
	}
}
