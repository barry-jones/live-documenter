
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    using System.Collections.Generic;
    using Signitures;

    internal sealed class VBConstructorFormatter : VBFormatter, IConstructorFormatter
    {
        private ConstructorSyntax _syntax;
        private Signiture _signiture;

        public VBConstructorFormatter(ConstructorSyntax syntax)
        {
            _syntax = syntax;
            _signiture = syntax.Method.Signiture;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        public List<SyntaxToken> FormatVisibility(ConstructorSyntax syntax)
        {
            return FormatVisibility(syntax.GetVisibility());
        }

        public SyntaxToken FormatInheritance(ConstructorSyntax syntax)
        {
            return FormatInheritance(syntax.GetInheritance());
        }

        public List<SyntaxToken> FormatParameters(ConstructorSyntax syntax)
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();
            List<ParameterDetails> parameters = syntax.GetParameters();

            tokens.Add(new SyntaxToken("(", SyntaxTokens.Text));
            for(int i = 0; i < parameters.Count; i++)
            {
                if(i != 0)
                {
                    tokens.Add(new SyntaxToken(",\n\t", SyntaxTokens.Text));
                }
                else
                {
                    tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
                }

                tokens.AddRange(FormatParameterModifiers(parameters[i]));

                tokens.Add(new SyntaxToken(parameters[i].Name, SyntaxTokens.Text));

                tokens.Add(Constants.Space);
                tokens.Add(Constants.KeywordAs);
                tokens.Add(Constants.Space);

                tokens.AddRange(FormatTypeDetails(parameters[i].TypeDetails));
            }
            if(parameters.Count > 0)
            {
                tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
            }
            tokens.Add(new SyntaxToken(")", SyntaxTokens.Text));

            return tokens;
        }

        public SyntaxTokenCollection Format(ConstructorSyntax syntax)
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
            tokens.Add(Constants.KeywordSub);
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken("New", SyntaxTokens.Keyword));
            tokens.AddRange(FormatParameters(syntax));

            return tokens;
        }
    }
}