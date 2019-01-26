
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

        public FieldDef() { }

        /// <summary>
        /// Initialises an instance of the FieldDef with the specified details.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="definingAssembly">The assembly the field is defined in.</param>
        /// <param name="containingType">The type the field is declared in.</param>
        /// <param name="attributes">Attributes describing the field.</param>
        /// <param name="signitureIndex">Index in to the signature stream for the field.</param>
        public FieldDef(string name, AssemblyDef definingAssembly, TypeDef containingType, FieldAttributes attributes, BlobIndex signitureIndex)
        {
            UniqueId = definingAssembly.CreateUniqueId();
            Assembly = definingAssembly;
            Type = containingType;
            Name = name;
            SignitureBlob = signitureIndex;
            _flags = attributes;
            _constants = new List<ConstantInfo>();
        }

        /// <summary>
        /// Indicates if this field is system generated. .NET creates backing fields
        /// in certain situations.
        /// </summary>
        public bool IsSystemGenerated
        {
            // `value__` is the compiler generated field for enumerations

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
        /// Not used at the moment.
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
                switch(_flags & FieldAttributes.FieldAccessMask)
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