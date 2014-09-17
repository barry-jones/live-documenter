﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	/// <summary>
	/// Enumeration of token types that <see cref="SyntaxToken"/> can
	/// be.
	/// </summary>
	public enum SyntaxTokens {
		/// <summary>
		/// This is a normal text token.
		/// </summary>
		Text,
		/// <summary>
		/// This token represents a keyword.
		/// </summary>
		Keyword
	}
}