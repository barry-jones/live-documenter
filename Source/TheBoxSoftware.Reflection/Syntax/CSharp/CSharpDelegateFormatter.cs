
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    using System;
    using System.Collections.Generic;

    internal sealed class CSharpDelegateFormatter : CSharpFormatter, IDelegateFormatter
    {
        private DelegateSyntax _syntax;

        public CSharpDelegateFormatter(DelegateSyntax syntax)
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
                    tokens.Add(new SyntaxToken($",{Environment.NewLine}", SyntaxTokens.Text));
                }
                else
                {
                    tokens.Add(new SyntaxToken(Environment.NewLine, SyntaxTokens.Text));
                }
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

        public SyntaxTokenCollection Format(DelegateSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();

            tokens.AddRange(FormatVisibility(syntax));
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.Add(new SyntaxToken("delegate", SyntaxTokens.Keyword));
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.AddRange(FormatReturnType(syntax));
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
            if(_syntax.Class.IsGeneric)
            {
                List<GenericTypeRef> genericTypes = _syntax.GetGenericParameters();
                tokens.AddRange(FormatGenericParameters(genericTypes));
            }
            tokens.AddRange(FormatParameters(syntax.Method));

            return tokens;
        }
    }
}