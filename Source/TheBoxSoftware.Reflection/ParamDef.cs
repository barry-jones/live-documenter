
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
        /// Constructs a ParamDef with the provided details.
        /// </summary>
        /// <param name="name">The name for the parameter</param>
        /// <param name="container">The method that defines and contains the parameter</param>
        /// <param name="sequence">The sequence in the parameter list for this parameter</param>
        /// <param name="definingAssembly">The assembly in which the parameter is defined</param>
        /// <param name="flags">The attribute flags for the parameter</param>
        public ParamDef(string name, MethodDef container, int sequence, AssemblyDef definingAssembly, ParamAttributeFlags flags)
        {
            _constants = new List<ConstantInfo>();
            _method = container;
            _sequence = sequence;
            _flags = flags;
            UniqueId = definingAssembly.CreateUniqueId();
            Assembly = definingAssembly;
            Name = name;
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