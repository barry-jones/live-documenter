
namespace TheBoxSoftware.Reflection.Syntax
{
    /// <summary>
    /// Enumeration of token types that <see cref="SyntaxToken"/> can be.
    /// </summary>
    public enum SyntaxTokens : byte
    {
        /// <summary>
        /// This is a normal text token.
        /// </summary>
        Text,
        /// <summary>
        /// This token represents a keyword.
        /// </summary>
        Keyword
    }
}