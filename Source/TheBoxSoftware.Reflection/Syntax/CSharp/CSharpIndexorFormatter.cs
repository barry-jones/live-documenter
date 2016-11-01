
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    using System.Collections.Generic;

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
    internal sealed class CSharpIndexerFormatter : CSharpFormatter, IIndexorFormatter
    {
        private IndexorSyntax _syntax;

        /// <summary>
        /// Initialises a new instance of the CSharpIndexorFormatter.
        /// </summary>
        /// <param name="syntax">The syntax containing the information about the member.</param>
        public CSharpIndexerFormatter(IndexorSyntax syntax)
        {
            _syntax = syntax;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        public SyntaxToken FormatIdentifier(IndexorSyntax syntax)
        {
            return new SyntaxToken("this", SyntaxTokens.Text);
        }

        public List<SyntaxToken> FormatType(IndexorSyntax syntax)
        {
            return FormatTypeDetails(syntax.GetType());
        }

        public List<SyntaxToken> FormatVisibility(IndexorSyntax syntax)
        {
            return FormatVisibility(syntax.GetVisibility());
        }

        public SyntaxToken FormatInheritance(IndexorSyntax syntax)
        {
            return FormatInheritance(syntax.GetInheritance());
        }

        public List<SyntaxToken> FormatGetVisibility(IndexorSyntax syntax)
        {
            return FormatVisibility(syntax.GetGetterVisibility());
        }

        public List<SyntaxToken> FormatSetVisibility(IndexorSyntax syntax)
        {
            return FormatVisibility(syntax.GetSetterVisibility());
        }

        /// <summary>
        /// Formats the indexer based on the language specification as a 
        /// collection of syntax tokens.
        /// </summary>
        /// <param name="syntax">The syntax class that describes the indexer.</param>
        /// <returns>The collection of tokens describing the indexer in the language</returns>
        public SyntaxTokenCollection Format(IndexorSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();
            tokens.AddRange(FormatVisibility(syntax));
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.AddRange(FormatType(syntax));
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.Add(FormatIdentifier(syntax));

            // Provide the properties to access the indexer, these are
            // obtained from the get method.
            MethodSyntax getMethod = new MethodSyntax(
                _syntax.GetMethod != null ? _syntax.GetMethod : _syntax.SetMethod
                );
            tokens.Add(new SyntaxToken("[", SyntaxTokens.Text));
            List<ParameterDetails> parameters = getMethod.GetParameters();
            // dont output the last parameter if we are not using the get method as it is the return value...
            for(int i = 0; i < parameters.Count; i++)
            {
                ParameterDetails current = parameters[i];

                tokens.AddRange(FormatTypeDetails(current.TypeDetails));
                tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
                tokens.Add(new SyntaxToken(current.Name, SyntaxTokens.Text));

                if(i < parameters.Count - 1)
                {
                    tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
                }
            }
            tokens.Add(new SyntaxToken("]", SyntaxTokens.Text));

            tokens.Add(new SyntaxToken(" {", SyntaxTokens.Text));
            if(_syntax.GetMethod != null)
            {
                tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
                if(syntax.GetVisibility() != syntax.GetGetterVisibility())
                {
                    tokens.AddRange(FormatGetVisibility(syntax));
                    tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
                }
                tokens.Add(new SyntaxToken("get", SyntaxTokens.Keyword));
                tokens.Add(new SyntaxToken(";", SyntaxTokens.Text));
            }
            if(this._syntax.SetMethod != null)
            {
                tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
                if(syntax.GetVisibility() != syntax.GetSetterVisibility())
                {
                    tokens.AddRange(FormatSetVisibility(syntax));
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