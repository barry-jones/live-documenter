
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    using System.Collections.Generic;

    internal sealed class VBDelegateFormatter : VBFormatter, IDelegateFormatter
    {
        private DelegateSyntax _syntax;

        public VBDelegateFormatter(DelegateSyntax syntax)
        {
            _syntax = syntax;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        public List<SyntaxToken> FormatVisibility(DelegateSyntax syntax)
        {
            return FormatVisibility(syntax.GetVisibility());
        }

        public List<SyntaxToken> FormatReturnType(DelegateSyntax syntax)
        {
            return FormatTypeDetails(syntax.GetReturnType());
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

        public SyntaxTokenCollection Format(DelegateSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();

            tokens.AddRange(FormatVisibility(syntax));
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken("Delegate", SyntaxTokens.Keyword));
            tokens.Add(Constants.Space);
            if(IsMethodFunction(syntax.GetReturnType()))
            {
                tokens.Add(Constants.KeywordFunction);
            }
            else
            {
                tokens.Add(Constants.KeywordSub);
            }
            tokens.Add(Constants.Space);
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
            tokens.AddRange(FormatParameters(syntax.Method));

            // Add the generic details of the delegate
            if(_syntax.Class.IsGeneric)
            {
                List<GenericTypeRef> genericTypes = _syntax.GetGenericParameters();
                tokens.AddRange(FormatGenericParameters(genericTypes));
            }

            if(this.IsMethodFunction(syntax.GetReturnType()))
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