
namespace TheBoxSoftware.Reflection
{
    using System.Collections.Generic;
    using Core.COFF;

    /// <summary>
    /// Describes the construction of a field in the CLR metadata.
    /// </summary>
    public class FieldDef : MemberRef
    {
        private List<ConstantInfo> _constants;
        private FieldAttributes _flags;

        /// <summary>
        /// Initialises a new instance of the FieldDef class based on the metadata provided
        /// </summary>
        /// <param name="references">The assembly that the field resides in</param>
        /// <param name="container">The type this field is contained in</param>
        /// <param name="row">The metadata row describing the field</param>
        /// <returns>The initialised field</returns>
        internal static FieldDef CreateFromMetadata(BuildReferences references, TypeDef container, FieldMetadataTableRow row)
        {
            FieldDef field = new FieldDef();

            field.UniqueId = references.Assembly.CreateUniqueId();
            field.Assembly = references.Assembly;
            field.Type = container;
            field.Name = references.Assembly.StringStream.GetString(row.Name.Value);
            field.SignitureBlob = row.Signiture;
            field.Flags = row.Flags;
            field.Constants = new List<ConstantInfo>();

            return field;
        }

        /// <summary>
        /// Indicates if this field is system generated. .NET creates backing fields
        /// in certain situations.
        /// </summary>
        public bool IsSystemGenerated
        {
            get
            {
                return Name == "value__" 
                    || Attributes.Find(attribute => attribute.Name == "CompilerGeneratedAttribute") != null;
            }
        }

        /// <summary>
        /// The flags detailing information about the field
        /// </summary>
        public FieldAttributes Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>
        /// TODO: Document
        /// </summary>
        public List<ConstantInfo> Constants
        {
            get { return _constants; }
            set { _constants = value; }
        }

        /// <summary>
        /// Indicates if the field is declared as a constant.
        /// </summary>
        public bool IsConstant
        {
            get
            {
                return Constants.Count > 0;
            }
        }

        public override Visibility MemberAccess
        {
            get
            {
                switch(this.Flags & FieldAttributes.FieldAccessMask)
                {
                    case FieldAttributes.Public:
                        return Visibility.Public;
                    case FieldAttributes.Assembly:
                        return Visibility.Internal;
                    case FieldAttributes.FamANDAssem:
                        return Visibility.Internal;
                    case FieldAttributes.Family:
                        return Visibility.Protected;
                    case FieldAttributes.Private:
                        return Visibility.Private;
                    case FieldAttributes.FamORAssem:
                        return Visibility.InternalProtected;
                    default:
                        return Visibility.Internal;
                }
            }
        }
    }
}