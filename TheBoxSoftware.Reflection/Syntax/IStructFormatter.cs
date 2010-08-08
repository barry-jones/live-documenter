﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	public interface IStructFormatter : IFormatter {
		List<SyntaxToken> FormatVisibility(StructSyntax syntax);
		List<SyntaxToken> FormatInterfaces(StructSyntax syntax);
		List<SyntaxToken> Format(StructSyntax syntax);
	}
}