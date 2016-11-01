
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    using System.Collections.Generic;

    internal sealed class VBConstantFormatter : VBFormatter, IConstantFormatter
    {
        private ConstantSyntax _syntax;

        public VBConstantFormatter(ConstantSyntax syntax)
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

            // e.g. Protected Const MyConstant As Integer
            tokens.AddRange(GetVisibility(syntax));
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken("Const", SyntaxTokens.Keyword));
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