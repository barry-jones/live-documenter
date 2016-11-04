
namespace TheBoxSoftware.Reflection
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Core.COFF;

    /// <summary>
    /// Describes a parameter
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class ParamDef : ReflectedMember
    {
        private ParamAttributeFlags _flags;
        private List<ConstantInfo> _constants;
        private int _sequence;
        private MethodDef _method;

        public ParamDef()
        {
            _constants = new List<ConstantInfo>();
        }

        /// <summary>
        /// Constructs a ParamDef from the PeCoffFile metadata.
        /// </summary>
        /// <param name="references">The collection of references required to build from metadata</param>
        /// <param name="container">The method which defines this parameter</param>
        /// <param name="row">The metadata row to initialise this parameter with</param>
        internal ParamDef(BuildReferences references, MethodDef container, ParamMetadataTableRow row)
        {
            _constants = new List<ConstantInfo>();
            UniqueId = references.Assembly.CreateUniqueId();
            Name = references.Assembly.StringStream.GetString(row.Name.Value);
            _method = container;
            _sequence = row.Sequence;
            Assembly = references.Assembly;
            _flags = row.Flags;
        }

        public TypeRef GetTypeRef()
        {
            if(_sequence == 0)
            {
                // TODO: Need to understand this more, but it generally
                // refers to information about the return type for the method.
                return null;
            }
            else
            {
                Signitures.ParamSignitureToken token = _method.Signiture.GetParameterTokens()[_sequence - 1];
                return token.ResolveParameter(_method.Assembly, this);
            }
        }

        /// <summary>
        /// Reference to the method that owns the parameter
        /// </summary>
        public MethodDef Method
        {
            get { return _method; }
            set { _method = value; }
        }

        /// <Summary>
        /// A number that indicates the sequence in the parameter list this ParamDef
        /// refers to on its parent method.
        /// </Summary>
        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }

        /// <summary>
        /// The constant values associated with the parameters if any.
        /// </summary>
        public List<ConstantInfo> Constants
        {
            get { return _constants; }
            set { _constants = value; }
        }

        /// <summary>
        /// Indicates if the parameter has been declared as an in paramter
        /// </summary>
        public bool IsIn
        {
            get { return (_flags & ParamAttributeFlags.In) != 0; }
        }

        /// <summary>
        /// Indicates if the parameter has been declared as an out parameter
        /// </summary>
        public bool IsOut
        {
            get { return (_flags & ParamAttributeFlags.Out) != 0; }
        }

        /// <summary>
        /// Indicates if the parameter has been declared as optional.
        /// </summary>
        public bool IsOptional
        {
            get { return (_flags & ParamAttributeFlags.Optional) != 0; }
        }
    }
}