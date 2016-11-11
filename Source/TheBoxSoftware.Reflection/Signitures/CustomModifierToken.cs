
namespace TheBoxSoftware.Reflection.Signitures
{
    using System;
    using System.Diagnostics;
    using Core;
    using Core.COFF;

    /// <summary>
    /// A token that describes a modified reference to a TypeDef, TypeRef or TypeSpec
    /// entry in the metadata tables.
    /// </summary>
	[DebuggerDisplay("Modifier: {Modifier} for [{CodedIndex.ToString()}]")]
    internal sealed class CustomModifierToken : SignatureToken
    {
        private ElementTypes _modifier;
        private CodedIndex _index;

        /// <summary>
        /// Initialises a CustomModifier token from the <paramref name="signiture"/> blob at the
        /// specified <paramref name="offset"/>.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
        /// <param name="offset">The offset in the signiture.</param>
		public CustomModifierToken(byte[] signiture, Offset offset)
            : base(SignatureTokens.CustomModifier)
        {
            _modifier = (ElementTypes)GetCompressedValue(signiture, offset);
            _index = ReadEncodedDefOrRefToken(signiture, offset);
        }

        /// <summary>
        /// Reads a DefOrRefToken from the <paramref name="signiture"/> which defines a compressed
        /// form of TypeRef, TypeSpec, or TypeDef token.
        /// </summary>
        /// <param name="signiture">The signiture blob.</param>
        /// <param name="offset">The offset in the signiture.</param>
        /// <returns></returns>
		private CodedIndex ReadEncodedDefOrRefToken(byte[] signiture, Offset offset)
        {
            // Read the typedef, typeref or typespec token
            int typeMask = 0x00000003;
            uint token = GetCompressedValue(signiture, offset);

            // Resolved values
            MetadataTables table;
            uint index = token >> 2;

            switch(typeMask & token)
            {
                case 0: // TypeDef
                    table = MetadataTables.TypeDef;
                    break;
                case 1: // TypeRef
                    table = MetadataTables.TypeRef;
                    break;
                case 2: // TypeSpec
                    table = MetadataTables.TypeSpec;
                    break;
                default:
                    throw new InvalidOperationException("Metadata Table could not be resolved for this Signiture");
            }
            return new CodedIndex(table, index);
        }

        /// <summary>
        /// Checks if the next token at the current offset can potentially be a
        /// CustomModifierToken.
        /// </summary>
        /// <param name="signiture">The signiture to preview</param>
        /// <param name="offset">The current offset in the signiture</param>
        /// <returns></returns>
        public static bool IsToken(byte[] signiture, int offset)
        {
            ElementTypes modifier = (ElementTypes)GetCompressedValue(signiture, offset);
            return modifier == ElementTypes.CModOptional || modifier == ElementTypes.CModRequired;
        }

        /// <summary>
        /// Produces a string representaion of the custom modifier token.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return $"[CustomMod: {Modifier} Index <{Index.Table},{Index.Index}>]";
        }

        /// <summary>
        /// The modifer for the token either CModOptional or CModRequired.
        /// </summary>
        public ElementTypes Modifier
        {
            get { return _modifier; }
            private set { _modifier = value; }
        }

        /// <summary>
        /// The index to the TypeDef, TypeRef or TypeSpec metadata table.
        /// </summary>
        public CodedIndex Index
        {
            get { return _index; }
            private set { _index = value; }
        }
    }
}