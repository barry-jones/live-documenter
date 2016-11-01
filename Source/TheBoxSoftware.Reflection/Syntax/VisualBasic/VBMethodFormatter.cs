
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    using System.Collections.Generic;
    using Signitures;

    internal sealed class VBMethodFormatter : VBFormatter, IMethodFormatter
    {
        private MethodSyntax _syntax;
        private Signiture _signiture;

        public VBMethodFormatter(MethodSyntax syntax)
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
                }
                tokens.Add(new SyntaxToken(parameters[i].Name, SyntaxTokens.Text));
                tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
                tokens.Add(new SyntaxToken("As", SyntaxTokens.Keyword));
                tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
                tokens.AddRange(this.FormatTypeDetails(parameters[i].TypeDetails));
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
            return this.FormatTypeDetails(syntax.GetReturnType());
        }

        public SyntaxTokenCollection Format(MethodSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();
            TypeDetails returnType = syntax.GetReturnType();
            bool isFunction = IsMethodFunction(returnType);

            SyntaxToken inheritanceModifier = FormatInheritance(syntax);

            tokens.AddRange(FormatVisibility(syntax));
            if(inheritanceModifier != null)
            {
                tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
                tokens.Add(inheritanceModifier);
            }

            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            if(isFunction)
            {
                tokens.Add(new SyntaxToken("Function", SyntaxTokens.Keyword));
            }
            else
            {
                tokens.Add(new SyntaxToken("Sub", SyntaxTokens.Keyword));
            }

            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
            if(syntax.Method.IsGeneric)
            {
                tokens.Add(new SyntaxToken("(", SyntaxTokens.Text));
                tokens.Add(new SyntaxToken("Of", SyntaxTokens.Keyword));
                tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
                List<GenericTypeRef> genericTypes = syntax.GetGenericParameters();
                for(int i = 0; i < genericTypes.Count; i++)
                {
                    if(i != 0)
                    {
                        tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
                    }
                    tokens.Add(FormatTypeName(genericTypes[i]));
                }
                tokens.Add(new SyntaxToken(")", SyntaxTokens.Text));
            }
            tokens.AddRange(FormatParameters(syntax));

            if(isFunction)
            {
                tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
                tokens.Add(new SyntaxToken("As", SyntaxTokens.Keyword));
                tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
                tokens.AddRange(this.FormatReturnType(syntax));
            }

            return tokens;
        }
    }
}