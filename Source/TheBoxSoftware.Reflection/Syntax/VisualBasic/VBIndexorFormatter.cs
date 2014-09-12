using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.VisualBasic {
	/// <summary>
	/// <para>
	/// Provides a formatting implementation for Indexor properties in 
	/// VB.NET.
	/// </para>
	/// <para>
	/// Below is an example of how indexors are formatted in VB.NET:
	/// <example>
	/// Public Property Item(index As Integer) As Object
	/// </example>
	/// </para>
	/// </summary>
	internal class VBIndexorFormatter : VBFormatter, IIndexorFormatter {
		private IndexorSyntax syntax;

		public VBIndexorFormatter(IndexorSyntax syntax) {
			this.syntax = syntax;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}

		public SyntaxToken FormatIdentifier(IndexorSyntax syntax) {
			return new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text);
		}

		public List<SyntaxToken> FormatType(IndexorSyntax syntax) {
			return this.FormatTypeDetails(syntax.GetType());
		}

		public List<SyntaxToken> FormatVisibility(IndexorSyntax syntax) {
			return this.FormatVisibility(syntax.GetVisibility());
		}

		public SyntaxToken FormatInheritance(IndexorSyntax syntax) {
			return this.FormatInheritance(syntax.GetInheritance());
		}

		public List<SyntaxToken> FormatGetVisibility(IndexorSyntax syntax) {
			return this.FormatVisibility(syntax.GetGetterVisibility());
		}

		public List<SyntaxToken> FormatSetVisibility(IndexorSyntax syntax) {
			return this.FormatVisibility(syntax.GetSetterVisibility());
		}

		public SyntaxTokenCollection Format(IndexorSyntax syntax) {
			SyntaxTokenCollection tokens = new SyntaxTokenCollection();
			tokens.AddRange(this.FormatVisibility(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("Property", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(this.FormatIdentifier(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(new SyntaxToken("As", SyntaxTokens.Keyword));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.AddRange(this.FormatType(syntax));
			return tokens;
		}
	}
}
