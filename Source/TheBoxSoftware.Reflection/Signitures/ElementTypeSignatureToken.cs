
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Diagnostics;
    using Core;

    /// <summary>
    /// <para>
    /// Class that represents the simplist single element when a type is involved in
    /// a signiture. Where for example a type can be represented simply as a base type
    /// a class, or a valuetype, the ElementTypeSignitureToken will contain the relevant
    /// details about it and provide a mechanism for resolving the type.
    /// </para>
    /// <para>
    /// Where a type is described by more than a single element; that element will have
    /// its superflous detail described in the <see cref="TypeSignatureToken"/> class.
    /// </para>
    /// </summary>
    /// <seealso cref="TypeSignatureToken"/>
    [DebuggerDisplay("ElementType: {ElementType}, {Token}")]
    internal sealed class ElementTypeSignatureToken : SignatureToken
    {
        private uint _token;
        private object _definition;
        private ElementTypes _elementType;

        /// <summary>
        /// Instantiates a new instance of the ElementTypeSignitureToken class.
        /// </summary>
        /// <param name="signiture">The signiture where this token is defined.</param>
        /// <param name="offset">The current offset in the signiture to read the token.</param>
        /// <remarks>
        /// <para>
        /// An ElementTypeSignitureToken details the element of a Type signiture. These
        /// elements are defined in section 23.1.16 in ECMA 335. Where a type can contain
        /// multiple ElementTypeSignitureTokens each building up to reveal more information
        /// about the type. This class will only ever provide a single item of detail.
        /// </para>
        /// </remarks>
        public ElementTypeSignatureToken(byte[] signiture, Offset offset)
            : base(SignatureTokens.ElementType)
        {

            ElementType = (ElementTypes)GetCompressedValue(signiture, offset);
            int typeMask;
            uint token;

            switch(ElementType)
            {
                case ElementTypes.Class:
                    // Read the typedef, typeref or typespec token
                    typeMask = 0x00000003;
                    token = GetCompressedValue(signiture, offset);
                    switch(typeMask & token)
                    {
                        case 0: // TypeDef
                            Token = token >> 2 | (int)ILMetadataToken.TypeDef; // (token & typeMask) | token >> 2;
                            break;
                        case 1: // TypeRef
                            Token = token >> 2 | (int)ILMetadataToken.TypeRef; //(token & typeMask) | token >> 2;
                            break;
                        case 2: // TypeSpec
                            Token = token >> 2 | (int)ILMetadataToken.TypeSpec; // (token & typeMask) | token >> 2;
                            break;
                    }
                    break;

                case ElementTypes.ValueType:
                    // Read the typedef, typeref or typespec token
                    typeMask = 0x00000003;
                    token = GetCompressedValue(signiture, offset);
                    switch(typeMask & token)
                    {
                        case 0: // TypeDef
                            Token = token >> 2 | (int)ILMetadataToken.TypeDef; // (token & typeMask) | token >> 2;
                            break;
                        case 1: // TypeRef
                            Token = token >> 2 | (int)ILMetadataToken.TypeRef; //(token & typeMask) | token >> 2;
                            break;
                    }
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
        /// <param name="signiture">The signiture blob.</param>
        /// <param name="offset">The offset in the signiture.</param>
        /// <param name="allowed">The allowed element type flags.</param>
        /// <returns>True of false</returns>
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
            return $"[ElementType: {Definition}] ";
        }

        /// <summary>
        /// The token parameter to this element type, this is not always relevant
        /// so can be zero.
        /// </summary>
        public uint Token
        {
            get { return _token; }
            private set { _token = value; }
        }

        /// <summary>
        /// The definition of the specified element. This is populated when the element
        /// is a well known system type. Token will be 0;
        /// </summary>
        public object Definition
        {
            get { return _definition; }
            set { _definition = value; }
        }

        /// <summary>
        /// The enumerated value indicating which type of element is contained in this token.
        /// </summary>
        public ElementTypes ElementType
        {
            get { return _elementType; }
            private set { _elementType = value; }
        }
    }
}