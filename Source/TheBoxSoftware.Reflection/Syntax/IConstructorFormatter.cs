
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;

    internal interface IConstructorFormatter : IFormatter
    {
        List<SyntaxToken> FormatVisibility(ConstructorSyntax syntax);

        SyntaxToken FormatInheritance(ConstructorSyntax syntax);

        List<SyntaxToken> FormatParameters(ConstructorSyntax syntax);

        SyntaxTokenCollection Format(ConstructorSyntax syntax);
    }
}
