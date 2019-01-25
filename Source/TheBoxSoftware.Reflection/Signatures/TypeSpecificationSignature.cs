
namespace TheBoxSoftware.Reflection.Signatures
{
    using System.Text;

    /// <summary>
    /// Represents a signiture for a type specification as detailed in
    /// section 23.2.14 in ECMA 335.
    /// </summary>
    internal sealed class TypeSpecificationSignature : Signature
    {
        /// <summary>
        /// Instantiates a new instance of the TypeSpecificationSigniture class.
        /// </summary>
        /// <param name="signiture">The actual signiture contents.</param>
        public TypeSpecificationSignature(byte[] signiture)
            : base(Signatures.TypeSpecification)
        {

            TypeToken = new TypeSignatureToken(signiture, 0);
        }

        /// <summary>
        /// Obtains the details of the type.
        /// </summary>
        /// <param name="member">The member to resolve against.</param>
        /// <returns>The details of the type having the specification.</returns>
        public TypeDetails GetTypeDetails(ReflectedMember member)
        {
            return TypeToken.GetTypeDetails(member);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[TypeSpec: ");

            foreach(SignatureToken t in Tokens)
            {
                sb.Append(t.ToString());
            }

            sb.Append("] ");

            return sb.ToString();
        }

        public TypeSignatureToken TypeToken { get; set; }
    }
}