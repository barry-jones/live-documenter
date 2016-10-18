using System;
using TheBoxSoftware.Reflection.Core.COFF;
using TheBoxSoftware.Reflection.Signitures;

namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// Details the specification of a type. This is generally used by the metadata
    /// to allow for types to derive from and implement interfaces and classes that
    /// are generic.
    /// </summary>
    internal sealed class TypeSpec : TypeRef
    {
        private TypeDetails _details = null;

        /// <summary>
        /// Creates a new instance of the TypeSpec class, using the provided information.
        /// </summary>
        /// <param name="assembly">The assembly this type specification is defined.</param>
        /// <param name="metatadata">The metadata directory containing the information.</param>
        /// <param name="row">The metadata row containing the actual details.</param>
        /// <returns>The instantiated type specification.</returns>
        public static TypeSpec CreateFromMetadata(AssemblyDef assembly, MetadataDirectory metatadata, TypeSpecMetadataTableRow row)
        {
            TypeSpec spec = new TypeSpec();
            spec.UniqueId = assembly.CreateUniqueId();
            spec.Assembly = assembly;
            spec.SignitureBlob = row.Signiture;
            return spec;
        }

        /// <summary>
        /// Loads the details of the underlying type and specification.
        /// </summary>
        private void LoadDetails()
        {
            TypeSpecificationSigniture signiture = this.Signiture;
            ElementTypes elementType = signiture.Type.ElementType.ElementType;

            this._details = signiture.GetTypeDetails(this);
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
        /// Storage for details about the type specifications signiture.
        /// </summary>
        private BlobIndex SignitureBlob { get; set; }

        /// <summary>
        /// The signiture defined for this member.
        /// </summary>
        internal Reflection.Signitures.TypeSpecificationSigniture Signiture
        {
            get
            {
                if(!this.Assembly.File.IsMetadataLoaded)
                {
                    throw new InvalidOperationException(
                        "The Signiture can not be parsed correctly until the metadata has been loaded."
                        );
                }

                BlobStream stream = (BlobStream)((Core.COFF.CLRDirectory)this.Assembly.File.Directories[
                    Core.PE.DataDirectories.CommonLanguageRuntimeHeader]).Metadata.Streams[Streams.BlobStream];
                return (TypeSpecificationSigniture)stream.GetSigniture(
                    (int)this.SignitureBlob.Value, this.SignitureBlob.SignitureType
                    );
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
        public TypeDef ImplementingType { get; set; }
    }
}