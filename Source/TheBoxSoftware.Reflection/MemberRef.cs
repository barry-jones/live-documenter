
namespace TheBoxSoftware.Reflection
{
    using System;
    using Core.COFF;

    /// <summary>
    /// Represents an instance of a Member that is referenced from an external source.
    /// </summary>
    public class MemberRef : ReflectedMember
    {
        private bool _isConstructor = false;
        private bool _isOperator = false;
        private CodedIndex _class;
        private TypeRef _type;
        private BlobIndex _signitureBlob;

        /// <summary>
        /// Factor method for instantiating and populating MemberRef instances from
        /// Metadata.
        /// </summary>
        /// <param name="assembly">The assembly the reference is defined in.</param>
        /// <param name="metadata">The metadata the reference is detailed in.</param>
        /// <param name="row">The actual metadata row with the details of the member.</param>
        /// <returns>An instantiated MemberRef instance.</returns>
        internal static MemberRef CreateFromMetadata(
                AssemblyDef assembly,
                MetadataDirectory metadata,
                MemberRefMetadataTableRow row)
        {
            MemberRef memberRef = new MemberRef();

            memberRef.UniqueId = assembly.CreateUniqueId();
            memberRef.Name = assembly.StringStream.GetString(row.Name.Value);
            memberRef.SignitureBlob = row.Signiture;
            memberRef.Assembly = assembly;
            memberRef._class = row.Class;

            // These methods of detecting different method types are not
            // infalable. A user can create a method for example that starts iwth
            // get_, set_ or op_. This best detail is stored in the MethodSemantics
            // table AND we will need to load that at some point :/
            memberRef.IsConstructor = memberRef.Name.StartsWith(".");
            memberRef.IsOperator = memberRef.Name.StartsWith("op_");

            return memberRef;
        }

        /// <summary>
        /// Gets or sets the type which defines this member
        /// </summary>
        public TypeRef Type
        {
            get
            {
                if(_type == null)
                {
                    // as per page: 126 of the ECMA, the class column can contain a reference to a TypeRef,
                    // MemberRef, TypeSpec or ModuleRef entry
                    // TODO: Handle TypeSpec and ModuleRef references
                    object o = Assembly.ResolveCodedIndex(_class);
                    if(o is TypeRef)
                    {
                        _type = (TypeRef)o;
                    }
                    else if(o is MethodDef)
                    {
                        _type = ((MethodDef)o).Type;
                    }
                }

                return _type;
            }
            set { _type = value; }
        }

        /// <summary>
        /// Obtains the index in the BlobStream where the methods signiture
        /// is stored.
        /// </summary>
        /// <seealso cref="Signiture"/>
        protected BlobIndex SignitureBlob
        {
            get { return _signitureBlob; }
            set { _signitureBlob = value; }
        }

        /// <summary>
        /// Gets a value indicating if this member is a constructor.
        /// </summary>
        public bool IsConstructor
        {
            get { return _isConstructor; }
            protected set { _isConstructor = value; }
        }

        /// <summary>
        /// Gets a value indicating if this method referes to an operator overloaded
        /// method implementation.
        /// </summary>
        public bool IsOperator
        {
            get { return _isOperator; }
            protected set { _isOperator = value; }
        }

        /// <summary>
        /// Gets the signiture defined for this member.
        /// </summary>
        internal Signitures.Signature Signiture
        {
            get
            {
                return Assembly.GetSigniture(SignitureBlob);
            }
        }
    }
}