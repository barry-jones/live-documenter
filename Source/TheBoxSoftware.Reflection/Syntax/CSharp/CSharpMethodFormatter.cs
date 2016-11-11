
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    using System.Collections.Generic;
    using Signatures;

    internal sealed class CSharpMethodFormatter : CSharpFormatter, IMethodFormatter
    {
        private MethodSyntax _syntax;
        private Signature _signiture;

        public CSharpMethodFormatter(MethodSyntax syntax)
        {
            _syntax = syntax;
            _signiture = syntax.Method.Signiture;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        public List<SyntaxToken> FormatVisibility(MethodSyntax syntax)
        {
            return FormatVisibility(syntax.GetVisibility());
        }

        public SyntaxToken FormatInheritance(MethodSyntax syntax)
        {
            return FormatInheritance(syntax.GetInheritance());
        }

        public List<SyntaxToken> FormatParameters(MethodSyntax syntax)
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
                    if(syntax.Method.IsExtensionMethod)
                    {
                        tokens.Add(Constants.KeywordThis);
                        tokens.Add(Constants.Space);
                    }
                }

                tokens.AddRange(FormatParameterModifiers(parameters[i]));
                tokens.AddRange(FormatTypeDetails(parameters[i].TypeDetails));

                tokens.Add(Constants.Space);
                tokens.Add(new SyntaxToken(parameters[i].Name, SyntaxTokens.Text));
            }
            if(parameters.Count > 0)
            {
                tokens.Add(new SyntaxToken("\n\t", SyntaxTokens.Text));
            }
            tokens.Add(new SyntaxToken(")", SyntaxTokens.Text));

            return tokens;
        }

        public List<SyntaxToken> FormatReturnType(MethodSyntax syntax)
        {
            return FormatTypeDetails(syntax.GetReturnType());
        }

        public SyntaxTokenCollection Format(MethodSyntax syntax)
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
            tokens.AddRange(FormatReturnType(syntax));
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
            if(syntax.Method.IsGeneric)
            {
                tokens.Add(Constants.GenericStart);
                List<GenericTypeRef> genericTypes = syntax.GetGenericParameters();
                for(int i = 0; i < genericTypes.Count; i++)
                {
                    if(i != 0)
                    {
                        tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
                    }
                    tokens.Add(FormatTypeName(genericTypes[i]));
                }
                tokens.Add(Constants.GenericEnd);
            }
            tokens.AddRange(FormatParameters(syntax));

            return tokens;
        }
    }
}