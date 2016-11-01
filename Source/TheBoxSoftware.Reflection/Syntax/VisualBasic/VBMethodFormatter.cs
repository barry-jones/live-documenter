
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
                tokens.Add(Constants.Space);
                tokens.Add(inheritanceModifier);
            }

            tokens.Add(Constants.Space);
            if(isFunction)
            {
                tokens.Add(Constants.KeywordFunction);
            }
            else
            {
                tokens.Add(Constants.KeywordSub);
            }

            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
            if(syntax.Method.IsGeneric)
            {
                tokens.Add(Constants.GenericStart);
                tokens.Add(Constants.Space);
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

            if(isFunction)
            {
                tokens.Add(Constants.Space);
                tokens.Add(Constants.KeywordAs);
                tokens.Add(Constants.Space);
                tokens.AddRange(FormatReturnType(syntax));
            }

            return tokens;
        }
    }
}