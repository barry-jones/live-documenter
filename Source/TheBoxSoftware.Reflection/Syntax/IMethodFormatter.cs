
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;
    // TODO: extern, new, modifiers

    internal interface IMethodFormatter : IFormatter
    {
        List<SyntaxToken> FormatVisibility(MethodSyntax syntax);

        SyntaxToken FormatInheritance(MethodSyntax syntax);

        List<SyntaxToken> FormatParameters(MethodSyntax syntax);

        List<SyntaxToken> FormatReturnType(MethodSyntax syntax);

        SyntaxTokenCollection Format(MethodSyntax syntax);
    }
}