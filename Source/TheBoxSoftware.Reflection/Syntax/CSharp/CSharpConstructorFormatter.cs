
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    using System;
    using System.Collections.Generic;
    using Signatures;

    internal sealed class CSharpConstructorFormatter : CSharpFormatter, IConstructorFormatter
    {
        private ConstructorSyntax _syntax;
        private Signature _signiture;

        public CSharpConstructorFormatter(ConstructorSyntax syntax)
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
                    tokens.Add(new SyntaxToken($",{Environment.NewLine}\t", SyntaxTokens.Text));
                }
                else
                {
                    tokens.Add(new SyntaxToken($"{Environment.NewLine}\t", SyntaxTokens.Text));
                }

                // need to check out first as out is also by ref
                tokens.AddRange(FormatParameterModifiers(parameters[i]));
                tokens.AddRange(FormatTypeDetails(parameters[i].TypeDetails));

                tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
                tokens.Add(new SyntaxToken(parameters[i].Name, SyntaxTokens.Text));
            }
            if(parameters.Count > 0)
            {
                tokens.Add(new SyntaxToken(Environment.NewLine, SyntaxTokens.Text));
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
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
            tokens.AddRange(FormatParameters(syntax));

            return tokens;
        }
    }
}