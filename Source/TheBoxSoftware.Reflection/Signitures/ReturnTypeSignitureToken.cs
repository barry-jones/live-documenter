
namespace TheBoxSoftware.Reflection.Signitures
{
    using System.Linq;
    using System.Text;
    using Core;

    /// <summary>
    /// Represents the group of SignitureTokens that are read together.
    /// </summary>
    internal sealed class ReturnTypeSignitureToken : SignitureTokenContainer
    {
        /// <summary>
        /// Initialises a new instance of the ReturnTypeSignitureToken class.
        /// </summary>
        /// <param name="signiture">The signiture to read.</param>
        /// <param name="offset">The offset to start processing at.</param>
        public ReturnTypeSignitureToken(byte[] signiture, Offset offset)
            : base(SignitureTokens.ReturnType)
        {
            while(CustomModifierToken.IsToken(signiture, offset))
            {
                Tokens.Add(new CustomModifierToken(signiture, offset));
            }

            if(ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.ByRef))
            {
                Tokens.Add(new ElementTypeSignitureToken(signiture, offset));    // ByRef
                Tokens.Add(new TypeSignitureToken(signiture, offset));   // Type
            }
            else if(ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.Void | ElementTypes.TypedByRef))
            {
                Tokens.Add(new ElementTypeSignitureToken(signiture, offset));    // Void, TypedByRef
            }
            else
            {
                Tokens.Add(new TypeSignitureToken(signiture, offset));
            }
        }

        internal TypeRef ResolveType(AssemblyDef assembly, ReflectedMember member)
        {
            SignitureToken token = Tokens.Last();

            if(token is TypeSignitureToken)
            {
                return ((TypeSignitureToken)token).ResolveType(assembly, member);
            }
            else
            {
                return ((ElementTypeSignitureToken)token).ResolveToken(assembly);
            }
        }

        public TypeDetails GetTypeDetails(ReflectedMember member)
        {
            TypeDetails details = new TypeDetails();
            SignitureToken token = Tokens.Last();

            if(token is TypeSignitureToken)
            {
                details = ((TypeSignitureToken)token).GetTypeDetails(member);
            }
            else
            {
                details.Type = ((ElementTypeSignitureToken)token).ResolveToken(member.Assembly);
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

            foreach(SignitureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}