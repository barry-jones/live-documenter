
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    using System.Collections.Generic;

    internal sealed class CSharpEventFormatter : CSharpFormatter, IEventFormatter
    {
        private EventSyntax _syntax;

        public CSharpEventFormatter(EventSyntax syntax)
        {
            _syntax = syntax;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        public SyntaxToken FormatIdentifier(EventSyntax syntax)
        {
            return new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text);
        }

        public List<SyntaxToken> FormatType(EventSyntax syntax)
        {
            return FormatTypeDetails(syntax.GetType());
        }

        public List<SyntaxToken> FormatVisibility(EventSyntax syntax)
        {
            return FormatVisibility(syntax.GetVisbility());
        }

        public SyntaxToken FormatInheritance(EventSyntax syntax)
        {
            return FormatInheritance(syntax.GetInheritance());
        }

        public SyntaxTokenCollection Format(EventSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();

            SyntaxToken inheritanceModifier = FormatInheritance(syntax);

            tokens.AddRange(FormatVisibility(syntax));
            if(inheritanceModifier != null)
            {
                tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
                tokens.Add(inheritanceModifier);
            }
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.Add(new SyntaxToken("event", SyntaxTokens.Keyword));
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.AddRange(FormatType(syntax));
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
            tokens.Add(new SyntaxToken(" {\n\t", SyntaxTokens.Text));
            tokens.Add(new SyntaxToken("add", SyntaxTokens.Keyword));
            tokens.Add(new SyntaxToken(";\n\t", SyntaxTokens.Text));
            tokens.Add(new SyntaxToken("remove", SyntaxTokens.Keyword));
            tokens.Add(new SyntaxToken(";\n\t", SyntaxTokens.Text));
            tokens.Add(new SyntaxToken("}", SyntaxTokens.Text));

            return tokens;
        }
    }
}