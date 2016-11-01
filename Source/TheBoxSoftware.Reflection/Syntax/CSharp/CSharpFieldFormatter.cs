
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    using System.Collections.Generic;

    internal sealed class CSharpFieldFormatter : CSharpFormatter, IFieldFormatter
    {
        private FieldSyntax _syntax;

        public CSharpFieldFormatter(FieldSyntax syntax)
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
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.Add(GetType(syntax));
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));

            return tokens;
        }
    }
}