
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    using System.Collections.Generic;

    internal sealed class VBPropertyFormatter : VBFormatter, IPropertyFormatter
    {
        private PropertySyntax _syntax;

        public VBPropertyFormatter(PropertySyntax syntax)
        {
            _syntax = syntax;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        public SyntaxToken FormatIdentifier(PropertySyntax syntax)
        {
            return new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text);
        }

        public List<SyntaxToken> FormatType(PropertySyntax syntax)
        {
            return FormatTypeDetails(syntax.GetType());
        }

        public List<SyntaxToken> FormatVisibility(PropertySyntax syntax)
        {
            return FormatVisibility(syntax.GetVisibility());
        }

        public SyntaxToken FormatInheritance(PropertySyntax syntax)
        {
            return FormatInheritance(syntax.GetInheritance());
        }

        public List<SyntaxToken> FormatGetVisibility(PropertySyntax syntax)
        {
            return FormatVisibility(syntax.GetGetterVisibility());
        }

        public List<SyntaxToken> FormatSetVisibility(PropertySyntax syntax)
        {
            return FormatVisibility(syntax.GetSetterVisibility());
        }

        public SyntaxTokenCollection Format(PropertySyntax syntax)
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