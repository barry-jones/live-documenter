
namespace TheBoxSoftware.Reflection
{
    using Reflection.Core.COFF;
    using Reflection.Signatures;

    /// <summary>
    /// Details the specification of a type. This is generally used by the metadata
    /// to allow for types to derive from and implement interfaces and classes that
    /// are generic.
    /// </summary>
    internal sealed class TypeSpec : TypeRef
    {
        private TypeDetails _details = null;
        private BlobIndex _signitureIndexInBlob;
        private TypeDef _implementingType;

        public TypeSpec() { }

        /// <summary>
        /// Creates a new instance of the TypeSpec class using the provided information.
        /// </summary>
        /// <param name="definingAssembly">The assembly which defines the type specification</param>
        /// <param name="signitureIndex">The index in to the blod where the signiture for this type is defined.</param>
        public TypeSpec(AssemblyDef definingAssembly, BlobIndex signitureIndex)
        {
            UniqueId = definingAssembly.CreateUniqueId();
            Assembly = definingAssembly;
            _signitureIndexInBlob = signitureIndex;
        }

        /// <summary>
        /// Loads the details of the underlying type and specification.
        /// </summary>
        private void LoadDetails()
        {
            TypeSpecificationSignature signiture = this.Signiture;

            _details = signiture.GetTypeDetails(this);
        }

        /// <summary>
        /// Gets the name of the underlying type in this specification.
        /// </summary>
        public override string Name
        {
            get
            {
                if(_details == null)
                {
                    LoadDetails();
                }
                return _details.Type.Name;
            }
            set { base.Name = value; }
        }

        /// <summary>
        /// Gets the namespace of the underlying type in this specification.
        /// </summary>
        public override string Namespace
        {
            get
            {
                if(_details == null)
                {
                    LoadDetails();
                }
                return _details.Type.Namespace;
            }
            set { base.Namespace = value; }
        }

        /// <summary>
        /// Indicates if this type is a generic type.
        /// </summary>
        public override bool IsGeneric
        {
            get
            {
                if(_details == null)
                {
                    LoadDetails();
                }
                return _details.IsGenericInstance;
            }
            set { base.IsGeneric = value; }
        }

        /// <summary>
        /// The signiture defined for this member.
        /// </summary>
        internal TypeSpecificationSignature Signiture
        {
            get
            {
                return Assembly.GetSigniture(_signitureIndexInBlob) as TypeSpecificationSignature;
            }
        }

        /// <summary>
        /// Gets a reference to the details of the underlying type.
        /// </summary>
        public TypeDetails TypeDetails
        {
            get
            {
                if(_details == null)
                {
                    LoadDetails();
                }
                return _details;
            }
        }

        /// <summary>
        /// Gets or sets the type which is implementing this base type or interface.
        /// </summary>
        public TypeDef ImplementingType
        {
            get { return _implementingType; }
            set { _implementingType = value; }
        }
    }
}