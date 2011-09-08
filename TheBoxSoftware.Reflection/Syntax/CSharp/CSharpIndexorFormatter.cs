using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax.CSharp {
	/// <summary>
	/// Provides C# Language specific formatting of indexers.
	/// </summary>
	/// <remarks>
	/// <para>The C# Langauge Specification states that indexers should be displayed
	/// as follows:</para>
	/// <code>
	/// indexer-declaration:
    ///		attributesopt   indexer-modifiersopt   indexer-declarator   {   accessor-declarations   }
	///		indexer-modifiers:
    ///		indexer-modifier
    ///		indexer-modifiers   indexer-modifier
	/// indexer-modifier:
    ///		new
    ///		public
    ///		protected
    ///		internal
    ///		private
    ///		virtual
    ///		sealed
    ///		override
    ///		abstract
    ///		extern
	/// indexer-declarator:
    ///		type   this   [   formal-parameter-list   ]
	///		type   interface-type   .   this   [   formal-parameter-list   ] 
	/// </code>
	/// </remarks>
	public sealed class CSharpIndexerFormatter : CSharpFormatter, IIndexorFormatter {
		private IndexorSyntax syntax;

		/// <summary>
		/// Initialises a new instance of the CSharpIndexorFormatter.
		/// </summary>
		/// <param name="syntax">The syntax containing the information about the member.</param>
		public CSharpIndexerFormatter(IndexorSyntax syntax) {
			this.syntax = syntax;
		}

		public SyntaxTokenCollection Format() {
			return this.Format(this.syntax);
		}

		public SyntaxToken FormatIdentifier(IndexorSyntax syntax) {
			return new SyntaxToken("this", SyntaxTokens.Text);
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

		/// <summary>
		/// Formats the indexer based on the language specification as a 
		/// collection of syntax tokens.
		/// </summary>
		/// <param name="syntax">The syntax class that describes the indexer.</param>
		/// <returns>The collection of tokens describing the indexer in the language</returns>
		public SyntaxTokenCollection Format(IndexorSyntax syntax) {
			SyntaxTokenCollection tokens = new SyntaxTokenCollection();
			tokens.AddRange(this.FormatVisibility(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.AddRange(this.FormatType(syntax));
			tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
			tokens.Add(this.FormatIdentifier(syntax));

			// Provide the properties to access the indexer, these are
			// obtained from the get method.
			MethodSyntax getMethod = new MethodSyntax(this.syntax.GetMethod);
			tokens.Add(new SyntaxToken("[", SyntaxTokens.Text));
			List<ParameterDetails> parameters = getMethod.GetParameters();
			for(int i = 0; i < parameters.Count; i++) {
				ParameterDetails current = parameters[i];

				tokens.AddRange(base.FormatTypeDetails(current.TypeDetails));
				tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				tokens.Add(new SyntaxToken(current.Name, SyntaxTokens.Text));

				if (i < parameters.Count - 1) {
					tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
				}
			}
			tokens.Add(new SyntaxToken("]", SyntaxTokens.Text));

			tokens.Add(new SyntaxToken(" {", SyntaxTokens.Text));
			if (this.syntax.GetMethod != null) {
				tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
				if (syntax.GetVisibility() != syntax.GetGetterVisibility()) {
					tokens.AddRange(this.FormatGetVisibility(syntax));
					tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				}
				tokens.Add(new SyntaxToken("get", SyntaxTokens.Keyword));
				tokens.Add(new SyntaxToken(";", SyntaxTokens.Text));
			}
			if (this.syntax.SetMethod != null) {
				tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
				if (syntax.GetVisibility() != syntax.GetSetterVisibility()) {
					tokens.AddRange(this.FormatSetVisibility(syntax));
					tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
				}
				tokens.Add(new SyntaxToken("set", SyntaxTokens.Keyword));
				tokens.Add(new SyntaxToken(";", SyntaxTokens.Text));
			}
			tokens.Add(new SyntaxToken("\n\t}", SyntaxTokens.Text));
			return tokens;
		}
	}
}
