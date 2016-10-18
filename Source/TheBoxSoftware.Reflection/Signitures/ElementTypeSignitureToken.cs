using System;
using System.Diagnostics;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures
{
    /// <summary>
    /// <para>
    /// Class that represents the simplist single element when a type is involved in
    /// a signiture. Where for example a type can be represented simply as a base type
    /// a class, or a valuetype, the ElementTypeSignitureToken will contain the relevant
    /// details about it and provide a mechanism for resolving the type.
    /// </para>
    /// <para>
    /// Where a type is described by more than a single element; that element will have
    /// its superflous detail described in the <see cref="TypeSignitureToken"/> class.
    /// </para>
    /// </summary>
    /// <seealso cref="TypeSignitureToken"/>
    [DebuggerDisplay("ElementType: {ElementType}, {Token}")]
    internal sealed class ElementTypeSignitureToken : SignitureToken
    {
        private Int32 _token;
        private object _definition;
        private ElementTypes _elementType;

        /// <summary>
        /// Instantiates a new instance of the ElementTypeSignitureToken class.
        /// </summary>
        /// <param name="file">The file which contains the signiture definition.</param>
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
        public ElementTypeSignitureToken(PeCoffFile file, byte[] signiture, Offset offset)
            : base(SignitureTokens.ElementType)
        {

            MetadataToDefinitionMap map = file.Map;
            this.ElementType = (ElementTypes)SignitureToken.GetCompressedValue(signiture, offset);
            int typeMask;
            int token;

            switch(this.ElementType)
            {
                case ElementTypes.Class:
                    // Read the typedef, typeref or typespec token
                    typeMask = 0x00000003;
                    token = SignitureToken.GetCompressedValue(signiture, offset);
                    switch(typeMask & token)
                    {
                        case 0: // TypeDef
                            this.Token = token >> 2 | (int)ILMetadataToken.TypeDef; // (token & typeMask) | token >> 2;
                            break;
                        case 1: // TypeRef
                            this.Token = token >> 2 | (int)ILMetadataToken.TypeRef; //(token & typeMask) | token >> 2;
                            break;
                        case 2: // TypeSpec
                            this.Token = token >> 2 | (int)ILMetadataToken.TypeSpec; // (token & typeMask) | token >> 2;
                            break;
                    }
                    break;

                case ElementTypes.ValueType:
                    // Read the typedef, typeref or typespec token
                    typeMask = 0x00000003;
                    token = SignitureToken.GetCompressedValue(signiture, offset);
                    switch(typeMask & token)
                    {
                        case 0: // TypeDef
                            this.Token = token >> 2 | (int)ILMetadataToken.TypeDef; // (token & typeMask) | token >> 2;
                            break;
                        case 1: // TypeRef
                            this.Token = token >> 2 | (int)ILMetadataToken.TypeRef; //(token & typeMask) | token >> 2;
                            break;
                    }
                    break;

                case ElementTypes.MVar:
                case ElementTypes.Var:
                    this.Token = SignitureToken.GetCompressedValue(signiture, offset);
                    break;

                // Well known types
                case ElementTypes.Boolean: this.Definition = map.GetWellKnownType("System", "Boolean"); break;
                case ElementTypes.I: this.Definition = map.GetWellKnownType("System", "IntPtr"); break;
                case ElementTypes.I1: this.Definition = map.GetWellKnownType("System", "SByte"); break;
                case ElementTypes.I2: this.Definition = map.GetWellKnownType("System", "Int16"); break;
                case ElementTypes.I4: this.Definition = map.GetWellKnownType("System", "Int32"); break;
                case ElementTypes.I8: this.Definition = map.GetWellKnownType("System", "Int64"); break;
                case ElementTypes.U: this.Definition = map.GetWellKnownType("System", "UIntPtr"); break;
                case ElementTypes.U1: this.Definition = map.GetWellKnownType("System", "Byte"); break;
                case ElementTypes.U2: this.Definition = map.GetWellKnownType("System", "UInt16"); break;
                case ElementTypes.U4: this.Definition = map.GetWellKnownType("System", "UInt32"); break;
                case ElementTypes.U8: this.Definition = map.GetWellKnownType("System", "UInt64"); break;
                case ElementTypes.Char: this.Definition = map.GetWellKnownType("System", "Char"); break;
                case ElementTypes.R4: this.Definition = map.GetWellKnownType("System", "Single"); break;
                case ElementTypes.R8: this.Definition = map.GetWellKnownType("System", "Double"); break;
                case ElementTypes.TypedByRef: this.Definition = map.GetWellKnownType("System", "TypedReference"); break;
                case ElementTypes.String: this.Definition = map.GetWellKnownType("System", "String"); break;
                case ElementTypes.Object: this.Definition = map.GetWellKnownType("System", "Object"); break;
                case ElementTypes.Void: this.Definition = map.GetWellKnownType("System", "Void"); break;
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
            ElementTypes value = (ElementTypes)SignitureToken.GetCompressedValue(signiture, offset);
            return value == allowed;
        }

        /// <summary>
        /// Resolves a token (Defintion) in the provided <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly reference to resolve the token with.</param>
        /// <returns>The resolved type reference.</returns>
		internal TypeRef ResolveToken(AssemblyDef assembly)
        {
            if(this.Definition != null)
            {
                return (TypeRef)this.Definition;
            }
            else
            {
                return (TypeRef)assembly.ResolveMetadataToken(this.Token);
            }
        }

        /// <summary>
        /// Produces a string representation of the element type token.
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString()
        {
            return string.Format("[ElementType: {0}] ", this._token);
        }

        /// <summary>
        /// The token parameter to this element type, this is not always relevant
        /// so can be zero.
        /// </summary>
        public Int32 Token
        {
            get { return this._token; }
            private set { this._token = value; }
        }

        /// <summary>
        /// The definition of the specified element. This is populated when the element
        /// is a well known system type. Token will be 0;
        /// </summary>
        public object Definition
        {
            get { return this._definition; }
            set { this._definition = value; }
        }

        /// <summary>
        /// The enumerated value indicating which type of element is contained in this token.
        /// </summary>
        public ElementTypes ElementType
        {
            get { return this._elementType; }
            private set { this._elementType = value; }
        }
    }
}