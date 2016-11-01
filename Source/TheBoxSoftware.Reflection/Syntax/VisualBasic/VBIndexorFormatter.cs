
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    using System.Collections.Generic;

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
    internal class VBIndexorFormatter : VBFormatter, IIndexorFormatter
    {
        private IndexorSyntax _syntax;

        public VBIndexorFormatter(IndexorSyntax syntax)
        {
            _syntax = syntax;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        public SyntaxToken FormatIdentifier(IndexorSyntax syntax)
        {
            return new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text);
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

        public SyntaxTokenCollection Format(IndexorSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();
            tokens.AddRange(FormatVisibility(syntax));
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken("Property", SyntaxTokens.Keyword));
            tokens.Add(Constants.Space);
            tokens.Add(FormatIdentifier(syntax));
            tokens.Add(Constants.Space);
            tokens.Add(Constants.KeywordAs);
            tokens.Add(Constants.Space);
            tokens.AddRange(FormatType(syntax));
            return tokens;
        }
    }
}