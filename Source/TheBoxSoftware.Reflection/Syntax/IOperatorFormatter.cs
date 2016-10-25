
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for sytax related classes to implement a standard mechanism
    /// for formatting operator overloads.
    /// </summary>
    internal interface IOperatorFormatter : IFormatter
    {
        List<SyntaxToken> FormatVisibility(OperatorSyntax syntax);

        SyntaxToken FormatInheritance(OperatorSyntax syntax);

        List<SyntaxToken> FormatParameters(OperatorSyntax syntax);

        List<SyntaxToken> FormatReturnType(OperatorSyntax syntax);

        List<SyntaxToken> FormatName(OperatorSyntax syntax);

        SyntaxTokenCollection Format(OperatorSyntax syntax);
    }
}