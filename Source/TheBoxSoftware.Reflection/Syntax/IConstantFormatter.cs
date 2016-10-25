
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;

    internal interface IConstantFormatter : IFormatter
    {
        SyntaxToken GetType(ConstantSyntax syntax);

        List<SyntaxToken> GetVisibility(ConstantSyntax syntax);

        SyntaxTokenCollection Format(ConstantSyntax syntax);
    }
}