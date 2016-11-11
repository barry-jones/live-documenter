
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Linq;
    using System.Text;
    using Core;

    /// <summary>
    /// Represents the group of SignitureTokens that are read together.
    /// </summary>
    internal sealed class ReturnTypeSignatureToken : SignatureTokenContainer
    {
        /// <summary>
        /// Initialises a new instance of the ReturnTypeSignitureToken class.
        /// </summary>
        /// <param name="signiture">The signiture to read.</param>
        /// <param name="offset">The offset to start processing at.</param>
        public ReturnTypeSignatureToken(byte[] signiture, Offset offset)
            : base(SignatureTokens.ReturnType)
        {
            while(CustomModifierToken.IsToken(signiture, offset))
            {
                Tokens.Add(new CustomModifierToken(signiture, offset));
            }

            if(ElementTypeSignatureToken.IsToken(signiture, offset, ElementTypes.ByRef))
            {
                Tokens.Add(new ElementTypeSignatureToken(signiture, offset));    // ByRef
                Tokens.Add(new TypeSignatureToken(signiture, offset));   // Type
            }
            else if(ElementTypeSignatureToken.IsToken(signiture, offset, ElementTypes.Void | ElementTypes.TypedByRef))
            {
                Tokens.Add(new ElementTypeSignatureToken(signiture, offset));    // Void, TypedByRef
            }
            else
            {
                Tokens.Add(new TypeSignatureToken(signiture, offset));
            }
        }

        internal TypeRef ResolveType(AssemblyDef assembly, ReflectedMember member)
        {
            SignatureToken token = Tokens.Last();

            if(token is TypeSignatureToken)
            {
                return ((TypeSignatureToken)token).ResolveType(assembly, member);
            }
            else
            {
                return ((ElementTypeSignatureToken)token).ResolveToken(assembly);
            }
        }

        public TypeDetails GetTypeDetails(ReflectedMember member)
        {
            TypeDetails details = new TypeDetails();
            SignatureToken token = Tokens.Last();

            if(token is TypeSignatureToken)
            {
                details = ((TypeSignatureToken)token).GetTypeDetails(member);
            }
            else
            {
                details.Type = ((ElementTypeSignatureToken)token).ResolveToken(member.Assembly);
            }

            return details;
        }

        /// <summary>
        /// Produces a string representation of the return type signiture.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[ReturnType: ");

            foreach(SignatureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}