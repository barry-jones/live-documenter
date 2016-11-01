
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <code>
    /// interface-declaration:
    ///		attributes(opt) interface-modifiers(opt) interface identifier interface-base(opt)
    /// </code>
    /// </remarks>
    internal sealed class CSharpInterfaceFormatter : CSharpFormatter, IInterfaceFormatter
    {
        private InterfaceSyntax _syntax;

        public CSharpInterfaceFormatter(InterfaceSyntax syntax)
        {
            _syntax = syntax;
        }

        public SyntaxTokenCollection Format()
        {
            return Format(_syntax);
        }

        /// <summary>
        /// Formats the visibility modifier for the interface.
        /// </summary>
        /// <param name="syntax">The syntax to format.</param>
        /// <returns>A string representing the modifier for the interface.</returns>
        /// <remarks>
        /// <code>
        /// interface-modifiers:
        ///     interface-modifier
        ///     interface-modifiers   interface-modifier
        /// interface-modifier:
        ///     new
        ///     public
        ///     protected
        ///     internal
        ///     private 
        /// </code>
        /// </remarks>
        public List<SyntaxToken> FormatVisibility(InterfaceSyntax syntax)
        {
            return FormatVisibility(syntax.GetVisibility());
        }

        /// <summary>
        /// Formats the interface base decleration for a c# interface.
        /// </summary>
        /// <param name="syntax">The syntax to format.</param>
        /// <returns>A string representing the interface-base for the interface.</returns>
        /// <remarks>
        /// <code>
        /// interface-base:
        ///     ;   interface-type-list 
        /// </code>
        /// </remarks>
        public List<SyntaxToken> FormatInterfaceBase(InterfaceSyntax syntax)
        {
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            // Create the list of types and interfaces
            List<SyntaxToken> baseTypesAndInterfaces = new List<SyntaxToken>();

            baseTypesAndInterfaces.AddRange(FormatTypeName(syntax.GetInterfaces()));

            if(baseTypesAndInterfaces.Count > 0)
            {
                tokens.Add(new SyntaxToken(": ", SyntaxTokens.Text));
                for(int i = 0; i < baseTypesAndInterfaces.Count; i++)
                {
                    if(i != 0)
                    {
                        tokens.Add(new SyntaxToken(", ", SyntaxTokens.Text));
                    }
                    tokens.Add(baseTypesAndInterfaces[i]);
                }
            }

            return tokens;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntax"></param>
        /// <returns></returns>
        /// <remarks>
        /// <code>
        /// interface-declaration:
        ///		attributes(opt) interface-modifiers(opt) interface identifier interface-base(opt)
        /// </code>
        /// </remarks>
        public SyntaxTokenCollection Format(InterfaceSyntax syntax)
        {
            SyntaxTokenCollection tokens = new SyntaxTokenCollection();

            tokens.AddRange(FormatVisibility(syntax));
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.Add(new SyntaxToken("interface", SyntaxTokens.Keyword));
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.Add(new SyntaxToken(syntax.GetIdentifier(), SyntaxTokens.Text));
            tokens.Add(new SyntaxToken(" ", SyntaxTokens.Text));
            tokens.AddRange(FormatInterfaceBase(syntax));

            return tokens;
        }
    }
}