
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the basic requirements for the information that a Formatter
    /// that implements this interface should return.
    /// </summary>
    internal interface IIndexorFormatter : IFormatter
    {
        SyntaxToken FormatIdentifier(IndexorSyntax syntax);

        List<SyntaxToken> FormatType(IndexorSyntax syntax);

        List<SyntaxToken> FormatVisibility(IndexorSyntax syntax);

        SyntaxToken FormatInheritance(IndexorSyntax syntax);

        List<SyntaxToken> FormatGetVisibility(IndexorSyntax syntax);

        List<SyntaxToken> FormatSetVisibility(IndexorSyntax syntax);

        SyntaxTokenCollection Format(IndexorSyntax syntax);
    }
}