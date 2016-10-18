using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures
{
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
        public ReturnTypeSignitureToken(PeCoffFile file, byte[] signiture, Offset offset)
            : base(SignitureTokens.ReturnType)
        {
            while(CustomModifierToken.IsToken(signiture, offset))
            {
                this.Tokens.Add(new CustomModifierToken(signiture, offset));
            }

            if(ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.ByRef))
            {
                this.Tokens.Add(new ElementTypeSignitureToken(file, signiture, offset));    // ByRef
                this.Tokens.Add(new TypeSignitureToken(file, signiture, offset));   // Type
            }
            else if(ElementTypeSignitureToken.IsToken(signiture, offset, ElementTypes.Void | ElementTypes.TypedByRef))
            {
                this.Tokens.Add(new ElementTypeSignitureToken(file, signiture, offset));    // Void, TypedByRef
            }
            else
            {
                this.Tokens.Add(new TypeSignitureToken(file, signiture, offset));
            }
        }

        internal TypeRef ResolveType(AssemblyDef assembly, ReflectedMember member)
        {
            if(this.Tokens.Last() is TypeSignitureToken)
            {
                return ((TypeSignitureToken)this.Tokens.Last()).ResolveType(assembly, member);
            }
            else
            {
                return ((ElementTypeSignitureToken)this.Tokens.Last()).ResolveToken(assembly);
            }
        }

        public TypeDetails GetTypeDetails(ReflectedMember member)
        {
            TypeDetails details = new TypeDetails();

            if(this.Tokens.Last() is TypeSignitureToken)
            {
                details = ((TypeSignitureToken)this.Tokens.Last()).GetTypeDetails(member);
            }
            else
            {
                details.Type = ((ElementTypeSignitureToken)this.Tokens.Last()).ResolveToken(member.Assembly);
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

            foreach(SignitureToken t in this.Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}