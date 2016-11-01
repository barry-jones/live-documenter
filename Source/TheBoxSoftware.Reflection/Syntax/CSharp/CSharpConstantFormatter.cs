
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    using System.Collections.Generic;

    internal sealed class CSharpConstantFormatter : CSharpFormatter, IConstantFormatter
    {
        private ConstantSyntax _syntax;

        public CSharpConstantFormatter(ConstantSyntax syntax)
        {
            _syntax = syntax;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        public SyntaxToken GetType(ConstantSyntax syntax)
        {
            return FormatTypeName(syntax.GetType());
        }

        public List<SyntaxToken> GetVisibility(ConstantSyntax syntax)
        {
            return FormatVisibility(syntax.GetVisibility());
        }

        public SyntaxTokenCollection Format(ConstantSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();

            tokens.AddRange(GetVisibility(syntax));
            tokens.Add(Constants.Space);
            tokens.Add(Constants.KeywordConstant);
            tokens.Add(Constants.Space);
            tokens.Add(GetType(syntax));
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));

            return tokens;
        }
    }
}