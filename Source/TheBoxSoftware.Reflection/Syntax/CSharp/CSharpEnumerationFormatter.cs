
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    using System.Collections.Generic;

    internal sealed class CSharpEnumerationFormatter : CSharpFormatter, IEnumerationFormatter
    {
        private EnumSyntax _syntax;

        public CSharpEnumerationFormatter(EnumSyntax syntax)
        {
            _syntax = syntax;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        public List<SyntaxToken> FormatVisibility(EnumSyntax syntax)
        {
            return FormatVisibility(syntax.GetVisibility());
        }

        public SyntaxTokenCollection Format(EnumSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();

            tokens.AddRange(FormatVisibility(syntax));
            tokens.Add(Constants.Space);
            tokens.Add(Constants.KeywordEnumeration);
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
            tokens.Add(new SyntaxToken(" : ", SyntaxTokens.Text));
            tokens.Add(FormatTypeName(syntax.GetUnderlyingType()));

            return tokens;
        }
    }
}