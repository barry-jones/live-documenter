using System.Diagnostics;
using TheBoxSoftware.Reflection.Core.COFF;

namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// Describes a parameter
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class ParamDef : ReflectedMember
    {
        private ParamAttributeFlags _flags;

        /// <summary>
        /// Initialises a ParamDef from provided metadata
        /// </summary>
        /// <param name="owner">The owning method of the parameter</param>
        /// <param name="metadata">The metadata stream</param>
        /// <param name="row">The row that details the parameter</param>
        /// <returns>The instantiated defenition</returns>
        internal static ParamDef CreateFromMetadata(MethodDef owner, MetadataDirectory metadata, ParamMetadataTableRow row)
        {
            ParamDef parameter = new ParamDef();

            AssemblyDef assembly = owner.Assembly;
            parameter.UniqueId = assembly.CreateUniqueId();
            parameter.Name = assembly.StringStream.GetString(row.Name.Value);
            assembly = null;
            parameter.Method = owner;
            parameter.Sequence = row.Sequence;
            parameter.Assembly = owner.Assembly;
            parameter._flags = row.Flags;

            return parameter;
        }

        public TypeRef GetTypeRef()
        {
            if(this.Sequence == 0)
            {
                // TODO: Need to understand this more, but it generally
                // refers to information about the return type for the method.
                return null;
            }
            else
            {
                Reflection.Signitures.ParamSignitureToken token = this.Method.Signiture.GetParameterTokens()[this.Sequence - 1];
                return token.ResolveParameter(this.Method.Assembly, this);
            }
        }

        private TypeRef ResolveParameterType()
        {
            if(this.Sequence == 0)
            {
                // TODO: Need to understand this more, but it generally
                // refers to information about the return type for the method.
                return null;
            }
            else
            {
                Reflection.Signitures.ParamSignitureToken token = this.Method.Signiture.GetParameterTokens()[this.Sequence - 1];
                return token.ResolveParameter(this.Method.Assembly, this);
            }
        }

        /// <summary>
        /// Reference to the method that owns the parameter
        /// </summary>
        public MethodDef Method { get; set; }

        /// <Summary>
        /// A number that indicates the sequence in the parameter list this ParamDef
        /// refers to on its parent method.
        /// </Summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Indicates if the parameter has been declared as an in paramter
        /// </summary>
        public bool IsIn
        {
            get { return (_flags & ParamAttributeFlags.In) == ParamAttributeFlags.In; }
        }

        /// <summary>
        /// Indicates if the parameter has been declared as an out parameter
        /// </summary>
        public bool IsOut
        {
            get { return (_flags & ParamAttributeFlags.Out) == ParamAttributeFlags.Out; }
        }

        /// <summary>
        /// Indicates if the parameter has been declared as optional.
        /// </summary>
        public bool IsOptional
        {
            get { return (_flags & ParamAttributeFlags.Optional) == ParamAttributeFlags.Optional; }
        }
    }
}