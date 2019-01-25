
namespace TheBoxSoftware.Reflection.Signatures
{
    using System.Diagnostics;
    using Core;

    /// <summary>
    /// A representation of a Type in a signature [23.1.16].
    /// </summary>
    /// <seealso cref="TypeSignatureToken"/>
    /// <include file='code-documentation\signatures.xml' path='docs/elementtypesignaturetoken/member[@name="class"]/*'/> 
    [DebuggerDisplay("ElementType: {ElementType}, {Token}")]
    internal sealed class ElementTypeSignatureToken : SignatureToken
    {
        /// <summary>
        /// Instantiates a new instance of the ElementTypeSignitureToken class.
        /// </summary>
        /// <include file='code-documentation\signatures.xml' path='docs/elementtypesignaturetoken/member[@name="ctor"]/*'/> 
        public ElementTypeSignatureToken(byte[] signiture, Offset offset)
            : base(SignatureTokens.ElementType)
        {
            ElementType = (ElementTypes)GetCompressedValue(signiture, offset);

            switch(ElementType)
            {
                case ElementTypes.Class:
                case ElementTypes.ValueType:
                    DecodeEncodedDefRefSpecToken(signiture, offset);
                    break;

                case ElementTypes.MVar:
                case ElementTypes.Var:
                    Token = SignatureToken.GetCompressedValue(signiture, offset);
                    break;

                // Well known types
                case ElementTypes.Boolean: Definition = WellKnownTypeDef.Boolean; break;
                case ElementTypes.I: Definition = WellKnownTypeDef.I; break;
                case ElementTypes.I1: Definition = WellKnownTypeDef.I1; break;
                case ElementTypes.I2: Definition = WellKnownTypeDef.I2; break;
                case ElementTypes.I4: Definition = WellKnownTypeDef.I4; break;
                case ElementTypes.I8: Definition = WellKnownTypeDef.I8; break;
                case ElementTypes.U: Definition = WellKnownTypeDef.U; break;
                case ElementTypes.U1: Definition = WellKnownTypeDef.U1; break;
                case ElementTypes.U2: Definition = WellKnownTypeDef.U2; break;
                case ElementTypes.U4: Definition = WellKnownTypeDef.U4; break;
                case ElementTypes.U8: Definition = WellKnownTypeDef.U8; break;
                case ElementTypes.Char: Definition = WellKnownTypeDef.Char; break;
                case ElementTypes.R4: Definition = WellKnownTypeDef.R4; break;
                case ElementTypes.R8: Definition = WellKnownTypeDef.R8; break;
                case ElementTypes.TypedByRef: Definition = WellKnownTypeDef.TypedByRef; break;
                case ElementTypes.String: Definition = WellKnownTypeDef.String; break;
                case ElementTypes.Object: Definition = WellKnownTypeDef.Object; break;
                case ElementTypes.Void: Definition = WellKnownTypeDef.Void; break;
            }
        }

        /// <summary>
        /// Checks if the token a the <paramref name="offset"/> in the <paramref name="signiture"/>
        /// is one of the <paramref name="allowed"/> element types.
        /// </summary>
        /// <include file='code-documentation\signatures.xml' path='docs/elementtypesignaturetoken/member[@name="istoken"]/*'/> 
        public static bool IsToken(byte[] signiture, int offset, ElementTypes allowed)
        {
            ElementTypes value = (ElementTypes)GetCompressedValue(signiture, offset);
            return value == allowed;
        }

        /// <summary>
        /// Resolves a token (Defintion) in the provided <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly reference to resolve the token with.</param>
        /// <returns>The resolved type reference.</returns>
		internal TypeRef ResolveToken(AssemblyDef assembly)
        {
            if(Definition != null)
            {
                return (TypeRef)Definition;
            }
            else
            {
                return (TypeRef)assembly.ResolveMetadataToken(Token);
            }
        }

        /// <summary>
        /// Produces a string representation of the element type token.
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString()
        {
            if (Definition != null)
                return $"[ElementType: {Definition}] ";
            
            return $"[ElementType: {ElementType} {Token}] ";
        }

        /// <summary>
        /// Decode the TypeDefOrRefOrSpecEncoded Token, the reverse of II.23.2.8.
        /// </summary>
        /// <param name="signiture">The signature being processed</param>
        /// <param name="offset">The current offset in to the signature</param>
        private void DecodeEncodedDefRefSpecToken(byte[] signiture, Offset offset)
        {
            // The reverse of the steps in 23.2.8 is:
            //  1. decompress
            //  2. shift the token back 2 bits
            //  3. Or the correct metadata token to the first two bits.
            uint typeMask = 0x00000003;
            uint token = GetCompressedValue(signiture, offset);

            switch (typeMask & token)
            {
                case 0:
                    Token = token >> 2 | (int)ILMetadataToken.TypeDef;
                    break;
                case 1:
                    Token = token >> 2 | (int)ILMetadataToken.TypeRef;
                    break;
                case 2:
                    Token = token >> 2 | (int)ILMetadataToken.TypeSpec;
                    break;
            }
        }

        /// <summary>
        /// The token parameter to this element type, this is not always relevant
        /// so can be zero.
        /// </summary>
        public uint Token { get; private set; }

        /// <summary>
        /// The definition of the specified element. This is populated when the element
        /// is a well known system type. Token will be 0;
        /// </summary>
        public object Definition { get; set; }

        /// <summary>
        /// The enumerated value indicating which type of element is contained in this token.
        /// </summary>
        public ElementTypes ElementType { get; private set; }
    }
}