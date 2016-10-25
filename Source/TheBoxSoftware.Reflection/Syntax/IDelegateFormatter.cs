
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;

    internal interface IDelegateFormatter : IFormatter
    {
        List<SyntaxToken> FormatVisibility(DelegateSyntax syntax);

        List<SyntaxToken> FormatReturnType(DelegateSyntax syntax);

        List<SyntaxToken> FormatParameters(MethodSyntax syntax);

        SyntaxTokenCollection Format(DelegateSyntax syntax);
    }
}