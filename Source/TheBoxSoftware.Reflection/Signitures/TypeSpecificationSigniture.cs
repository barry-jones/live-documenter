using System.Text;
using TheBoxSoftware.Diagnostics;
using TheBoxSoftware.Reflection.Core;

namespace TheBoxSoftware.Reflection.Signitures
{
    /// <summary>
    /// Represents a signiture for a type specification as detailed in
    /// section 23.2.14 in ECMA 335.
    /// </summary>
    internal sealed class TypeSpecificationSigniture : Signiture
    {
        /// <summary>
        /// Instantiates a new instance of the TypeSpecificationSigniture class.
        /// </summary>
        /// <param name="file">The file containing the signiture</param>
        /// <param name="signiture">The actual signiture contents.</param>
        public TypeSpecificationSigniture(byte[] signiture)
            : base(Signitures.TypeSpecification)
        {

            this.Type = new TypeSignitureToken(signiture, 0);
        }

        /// <summary>
        /// Obtains the details of the type.
        /// </summary>
        /// <param name="member">The member to resolve against.</param>
        /// <returns>The details of the type having the specification.</returns>
        public TypeDetails GetTypeDetails(ReflectedMember member)
        {
            return this.Type.GetTypeDetails(member);
        }

#if TEST
        /// <summary>
        /// Prints tokens to the current TRACE output
        /// </summary>
        public void PrintTokens()
        {
            StringBuilder sb = new StringBuilder();

            foreach (SignitureToken token in this.Type.Tokens)
            {
                switch (token.TokenType)
                {
                    case SignitureTokens.ArrayShape: // non-nested token
                    case SignitureTokens.CallingConvention:
                    case SignitureTokens.Constraint:
                    case SignitureTokens.ElementType:
                    case SignitureTokens.GenericArgumentCount:
                    case SignitureTokens.Count:
                    case SignitureTokens.CustomModifier:
                    case SignitureTokens.GenericParameterCount:
                    case SignitureTokens.Type:
                    case SignitureTokens.TypeDefOrRefEncodedToken:
                    case SignitureTokens.Field:
                    case SignitureTokens.LocalSigniture:
                    case SignitureTokens.Param:
                    case SignitureTokens.ParameterCount:
                    case SignitureTokens.Prolog:
                    case SignitureTokens.Property:
                    case SignitureTokens.ReturnType:
                    case SignitureTokens.Sentinal:                    
                    default:
                        sb.Append(token.ToString()); 
                        break;
                }
            }

            TraceHelper.WriteLine(sb.ToString());
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        public TypeSignitureToken Type { get; set; }
    }
}