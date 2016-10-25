
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;

    internal interface IStructFormatter : IFormatter
    {
        List<SyntaxToken> FormatVisibility(StructSyntax syntax);

        List<SyntaxToken> FormatInterfaces(StructSyntax syntax);

        SyntaxTokenCollection Format(StructSyntax syntax);
    }
}