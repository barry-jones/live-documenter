
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    using System.Collections.Generic;

    internal sealed class VBEnumerationFormatter : VBFormatter, IEnumerationFormatter
    {
        private EnumSyntax _syntax;

        public VBEnumerationFormatter(EnumSyntax syntax)
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
            tokens.Add(new SyntaxToken("Enum", SyntaxTokens.Keyword));
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
            tokens.Add(Constants.Space);
            tokens.Add(Constants.KeywordAs);
            tokens.Add(Constants.Space);
            tokens.Add(FormatTypeName(syntax.GetUnderlyingType()));

            return tokens;
        }
    }
}