using System.Collections.Generic;
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
        /// <param name="references">The owning method of the parameter</param>
        /// <param name="container">The method this parameter is for.</param>
        /// <param name="row">The row that details the parameter</param>
        /// <returns>The instantiated defenition</returns>
        internal static ParamDef CreateFromMetadata(BuildReferences references, MethodDef container, ParamMetadataTableRow row)
        {
            ParamDef parameter = new ParamDef();

            parameter.Constants = new List<ConstantInfo>();
            parameter.UniqueId = references.Assembly.CreateUniqueId();
            parameter.Name = references.Assembly.StringStream.GetString(row.Name.Value);
            parameter.Method = container;
            parameter.Sequence = row.Sequence;
            parameter.Assembly = references.Assembly;
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
        /// The constant values associated with the parameters if any.
        /// </summary>
        public List<ConstantInfo> Constants { get; set; }

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