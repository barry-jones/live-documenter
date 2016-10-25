
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;

    internal interface IEventFormatter : IFormatter
    {
        SyntaxToken FormatIdentifier(EventSyntax syntax);

        List<SyntaxToken> FormatType(EventSyntax syntax);

        List<SyntaxToken> FormatVisibility(EventSyntax syntax);

        SyntaxToken FormatInheritance(EventSyntax syntax);
    }
}