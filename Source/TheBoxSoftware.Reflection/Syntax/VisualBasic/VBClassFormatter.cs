
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    using System.Collections.Generic;

    internal sealed class VBClassFormatter : VBFormatter, IClassFormatter
    {
        private ClassSyntax _syntax;

        public VBClassFormatter(ClassSyntax syntax)
        {
            _syntax = syntax;
        }

        public List<SyntaxToken> FormatVisibility(ClassSyntax syntax)
        {
            return FormatVisibility(syntax.GetVisibility());
        }

        public SyntaxToken FormatInheritance(ClassSyntax syntax)
        {
            return FormatInheritance(syntax.GetInheritance());
        }

        public List<SyntaxToken> FormatClassBase(ClassSyntax syntax)
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();
            bool hasBaseType = false;

            // Create the list of types and interfaces
            if(syntax.Class.InheritsFrom != null && syntax.Class.InheritsFrom.GetFullyQualifiedName() != "System.Object")
            {
                hasBaseType = true;
            }

            if(hasBaseType)
            {
                tokens.Add(new SyntaxToken("Derives", SyntaxTokens.Keyword));
                tokens.Add(Constants.Space);
                tokens.AddRange(FormatTypeDetails(syntax.GetBaseClass()));
            }

            Signitures.TypeDetails[] interfaces = syntax.GetInterfaces();
            for(int i = 0; i < interfaces.Length; i++)
            {
                if(i == 0)
                {
                    tokens.Add(new SyntaxToken(" _\n\t", SyntaxTokens.Text));
                    tokens.Add(new SyntaxToken("Implements", SyntaxTokens.Keyword));
                    tokens.Add(Constants.Space);
                }
                else if(hasBaseType && i == 0 || i != 0)
                {
                    tokens.Add(new SyntaxToken(", _\n\t\t", SyntaxTokens.Text));
                }
                tokens.AddRange(FormatTypeDetails(interfaces[i]));
            }

            return tokens;
        }

        public SyntaxTokenCollection Format(ClassSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();
            SyntaxToken inheritanceModifier = FormatInheritance(syntax);

            tokens.AddRange(FormatVisibility(syntax));
            if(inheritanceModifier != null)
            {
                tokens.Add(Constants.Space);
                tokens.Add(inheritanceModifier);
            }
            tokens.Add(Constants.Space);
            tokens.Add(Constants.KeywordClass);
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));

            if(_syntax.Class.IsGeneric)
            {
                List<GenericTypeRef> genericTypes = _syntax.GetGenericParameters();
                tokens.AddRange(FormatGenericParameters(genericTypes));
            }

            List<SyntaxToken> baseTokens = FormatClassBase(syntax);
            if(baseTokens.Count > 0)
            {
                tokens.Add(new SyntaxToken(" _\n\t", SyntaxTokens.Text));
                tokens.AddRange(baseTokens);
            }

            return tokens;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }
    }
}