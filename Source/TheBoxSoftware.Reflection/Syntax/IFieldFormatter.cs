
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;

    internal interface IFieldFormatter : IFormatter
    {
        SyntaxToken GetType(FieldSyntax syntax);

        List<SyntaxToken> GetVisibility(FieldSyntax syntax);

        SyntaxTokenCollection Format(FieldSyntax syntax);
    }
}