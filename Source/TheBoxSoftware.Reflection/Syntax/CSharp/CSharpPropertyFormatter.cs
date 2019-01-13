
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    using System.Collections.Generic;

    internal sealed class CSharpPropertyFormatter : CSharpFormatter, IPropertyFormatter
    {
        private PropertySyntax _syntax;

        public CSharpPropertyFormatter(PropertySyntax syntax)
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
            tokens.AddRange(FormatType(syntax));
            tokens.Add(Constants.Space);
            tokens.Add(FormatIdentifier(syntax));
            tokens.Add(new SyntaxToken(" {", SyntaxTokens.Text));
            if(_syntax.GetMethod != null)
            {
                tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
                if(syntax.GetVisibility() != syntax.GetGetterVisibility())
                {
                    tokens.AddRange(FormatGetVisibility(syntax));
                    tokens.Add(Constants.Space);
                }
                tokens.Add(Constants.KeywordGet);
                tokens.Add(new SyntaxToken(";", SyntaxTokens.Text));
            }
            if(_syntax.SetMethod != null)
            {
                tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
                if(syntax.GetVisibility() != syntax.GetSetterVisibility())
                {
                    tokens.AddRange(FormatSetVisibility(syntax));
                    tokens.Add(Constants.Space);
                }
                tokens.Add(Constants.KeywordSet);
                tokens.Add(new SyntaxToken(";", SyntaxTokens.Text));
            }
            tokens.Add(new SyntaxToken("\n\t}", SyntaxTokens.Text));
            return tokens;
        }
    }
}