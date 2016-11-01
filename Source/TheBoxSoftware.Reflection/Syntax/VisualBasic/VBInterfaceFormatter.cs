
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    using System.Collections.Generic;

    internal sealed class VBInterfaceFormatter : VBFormatter, IInterfaceFormatter
    {
        private InterfaceSyntax _syntax;

        public VBInterfaceFormatter(InterfaceSyntax syntax)
        {
            _syntax = syntax;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        public List<SyntaxToken> FormatVisibility(InterfaceSyntax syntax)
        {
            return FormatVisibility(syntax.GetVisibility());
        }

        public List<SyntaxToken> FormatInterfaceBase(InterfaceSyntax syntax)
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            // Create the list of types and interfaces
            List<TypeRef> baseTypesAndInterfaces = new List<TypeRef>();

            baseTypesAndInterfaces.AddRange(syntax.GetInterfaces());

            if(baseTypesAndInterfaces.Count > 0)
            {
                tokens.Add(new SyntaxToken("Implements", SyntaxTokens.Keyword));
                tokens.Add(Constants.Space);
                for(int i = 0; i < baseTypesAndInterfaces.Count; i++)
                {
                    if(i != 0)
                    {
                        tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
                    }
                    tokens.Add(FormatTypeName(baseTypesAndInterfaces[i]));
                }
            }

            return tokens;
        }

        public SyntaxTokenCollection Format(InterfaceSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();

            tokens.AddRange(FormatVisibility(syntax));
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken("Interface", SyntaxTokens.Keyword));
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));

            List<SyntaxToken> baseTokens = FormatInterfaceBase(syntax);
            if(baseTokens.Count > 0)
            {
                tokens.Add(new SyntaxToken(" _\n\t", SyntaxTokens.Text));
                tokens.AddRange(baseTokens);
            }

            return tokens;
        }
    }
}