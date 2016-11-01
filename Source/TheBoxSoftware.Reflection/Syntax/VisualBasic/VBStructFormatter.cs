
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    using System.Collections.Generic;

    internal sealed class VBStructFormatter : VBFormatter, IStructFormatter
    {
        private StructSyntax _syntax;

        public VBStructFormatter(StructSyntax syntax)
        {
            _syntax = syntax;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        public List<SyntaxToken> FormatVisibility(StructSyntax syntax)
        {
            return FormatVisibility(syntax.GetVisibility());
        }

        public List<SyntaxToken> FormatInterfaces(StructSyntax syntax)
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            // Create the list of types and interfaces
            List<TypeRef> baseTypesAndInterfaces = new List<TypeRef>();
            baseTypesAndInterfaces.AddRange(syntax.GetInterfaces());

            if(baseTypesAndInterfaces.Count > 0)
            {
                tokens.Add(new SyntaxToken(": ", SyntaxTokens.Text));
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

        public SyntaxTokenCollection Format(StructSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();

            tokens.AddRange(FormatVisibility(syntax));
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken("Structure", SyntaxTokens.Keyword));
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
            tokens.Add(Constants.Space);
            tokens.AddRange(FormatInterfaces(syntax));

            return tokens;
        }
    }
}