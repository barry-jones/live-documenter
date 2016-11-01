
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    using System.Collections.Generic;

    internal sealed class VBFieldFormatter : VBFormatter, IFieldFormatter
    {
        private FieldSyntax _syntax;

        public VBFieldFormatter(FieldSyntax syntax)
        {
            _syntax = syntax;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        public SyntaxToken GetType(FieldSyntax syntax)
        {
            return FormatTypeName(syntax.GetType());
        }

        public List<SyntaxToken> GetVisibility(FieldSyntax syntax)
        {
            return FormatVisibility(syntax.GetVisibility());
        }

        public SyntaxTokenCollection Format(FieldSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();

            tokens.AddRange(GetVisibility(syntax));
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
            tokens.Add(Constants.Space);
            tokens.Add(Constants.KeywordAs);
            tokens.Add(Constants.Space);
            tokens.Add(GetType(syntax));

            return tokens;
        }
    }
}