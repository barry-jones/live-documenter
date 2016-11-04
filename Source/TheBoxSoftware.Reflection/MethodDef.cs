
namespace TheBoxSoftware.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Core;
    using Core.COFF;
    using Signitures;

    /// <summary>
    /// Represents a method definition as a reflected element.
    /// </summary>
    [DebuggerDisplay("Method Name={Name}")]
    public class MethodDef : MemberRef
    {
        private const int MethodBodySizeMask = 0x03;

        private uint _rva; // the reletive virtual address of this methods IL body
        private bool _isGeneric;
        private List<GenericTypeRef> _genericTypes;
        private bool _isSpecialName;
        private MethodAttributes _attributes;
        private MethodImplFlags _implementationFlags;
        private bool _isConversionOperator;
        private List<ParamDef> _parameters;

        public MethodDef()
        {
            _genericTypes = new List<GenericTypeRef>();
            _parameters = new List<ParamDef>();
        }

        /// <summary>
        /// Initialises a new instance of MethodDef for the provided data
        /// </summary>
        /// <param name="references">The references required to build the methods</param>
        /// <param name="container">The owning type for this method</param>
        /// <param name="row">The row detailing the method</param>
        /// <returns>The initialised MethodDef</returns>
        internal static MethodDef CreateFromMetadata(BuildReferences references, TypeDef container, MethodMetadataTableRow row)
        {
            MethodDefBuilder builder = new MethodDefBuilder(references, container, row);
            return builder.Build();
        }

        /// <summary>
        /// Obtains the details about the IL and body of this method and the contents
        /// of the MSIL.
        /// </summary>
        /// <returns>The instantiated <see cref="MethodBody"/>.</returns>
        public MethodBody GetMethodBody()
        {
            short maxStack = 0;
            int localsToken;
            int codeSize = 0;
            uint address = Assembly.FileAddressFromRVA(_rva);
            byte[] contents = Assembly.GetFileContents();

            byte firstByte = contents[address];
            byte[] instructions = new byte[0];

            if((firstByte & MethodBodySizeMask) == 0x02)
            {   // Tiny
                codeSize = firstByte >> 2;
                maxStack = 8;
                address++;
            }
            else if((firstByte & MethodBodySizeMask) == 0x03)
            {   // FAT
                Core.Offset offset = (int)address;
                uint flagsAndSize = BitConverter.ToUInt16(contents, offset.Shift(2));
                uint lengthOfHeader = flagsAndSize >> 12;
                maxStack = BitConverter.ToInt16(contents, offset.Shift(2));
                codeSize = BitConverter.ToInt32(contents, offset.Shift(4));
                localsToken = BitConverter.ToInt32(contents, offset.Shift(4));
                address = (uint)((int)offset);
            }

            // Popualate details of the method body
            MethodBody body = new MethodBody(
                GetInstructions(GetIL(address, codeSize)).ToList(),
                maxStack
                );
            return body;
        }

        /// <summary>
        /// Obtains the list of generic types that are defined and owned only by this member.
        /// </summary>
        /// <returns>A collection of generic types for this member</returns>
        public List<GenericTypeRef> GetGenericTypes()
        {
            return GenericTypes;
        }

        /// <summary>
        /// Obtains the Intermediate Language instructions for this method
        /// </summary>
        /// <param name="address">The address of the start of the actual code</param>
        /// <param name="codeSize">The size of the actual code.</param>
        /// <returns>A byte array of IL operations</returns>
        private byte[] GetIL(uint address, int codeSize)
        {
            byte[] contents = Assembly.GetFileContents();
            byte[] il = new byte[codeSize];
            for(int i = 0; i < codeSize; i++)
            {
                il[i] = contents[address + i];
            }
            return il;
        }

        /// <summary>
        /// Returns an array of instructions that describe the code portion of this method.
        /// </summary>
        /// <returns>The array of instructions</returns>
        private ILInstruction[] GetInstructions(byte[] fromILBytes)
        {
            List<ILInstruction> il = new List<ILInstruction>();
            OpCodesMap map = OpCodesMap.GetSingleton();

            // for now iterate over the IL bytes and change them to opcodes
            for(int i = 0; i < fromILBytes.Length; i++)
            {
                byte current = fromILBytes[i];
                OpCode code = OpCodes.Nop;
                if(current != 0xfe)
                {
                    code = map.GetCode(current);
                }
                else
                {
                    code = map.GetCode(BitConverter.ToInt16(fromILBytes, i));
                    i++;
                }

                ILInstruction instruction = null;
                switch(code.OperandType)
                {
                    case OperandType.InlineNone:
                        instruction = new InlineNoneILInstruction(code);
                        break;
                    case OperandType.ShortInlineBrTarget:
                        instruction = new ShortInlineBrTargetILInstruction(code, fromILBytes[i++]);
                        break;
                    case OperandType.InlineBrTarget:
                        instruction = new InlineBrTargetILInstruction(code, BitConverter.ToInt32(fromILBytes, i + 1));
                        i += 4;
                        break;
                    case OperandType.ShortInlineI:
                        instruction = new ShortInlineIILInstruction(code, fromILBytes[i++]);
                        break;
                    case OperandType.ShortInlineR:
                        instruction = new ShortInlineRILInstruction(code, BitConverter.ToSingle(fromILBytes, i + 1));
                        i += 4;
                        break;
                    case OperandType.ShortInlineVar:
                        instruction = new ShortInlineVarILInstruction(code, fromILBytes[i++]);
                        break;
                    case OperandType.InlineI:
                        instruction = new InlineIILInstruction(code, BitConverter.ToUInt32(fromILBytes, i + 1));
                        i += 4;
                        break;
                    case OperandType.InlineVar:
                        instruction = new InlineVarILInstruction(code, BitConverter.ToUInt16(fromILBytes, i + 1));
                        i += 2;
                        break;
                    case OperandType.InlineR:
                        instruction = new InlineRILInstruction(code, BitConverter.ToDouble(fromILBytes, i + 1));
                        i += 8;
                        break;
                    case OperandType.InlineI8:
                        instruction = new InlineI8ILInstruction(code, BitConverter.ToUInt64(fromILBytes, i + 1));
                        i += 8;
                        break;
                    case OperandType.InlineString:
                        instruction = new InlineStringILInstruction(this.Assembly, code, BitConverter.ToInt32(fromILBytes, i + 1));
                        i += 4;
                        break;
                    case OperandType.InlineSig:
                        instruction = new InlineSigILInstruction(this.Assembly, code, BitConverter.ToInt32(fromILBytes, i + 1));
                        i += 4;
                        break;
                    case OperandType.InlineField:
                        instruction = new InlineFieldILInstruction(this.Assembly, code, BitConverter.ToUInt32(fromILBytes, i + 1));
                        i += 4;
                        break;
                    case OperandType.InlineMethod:
                        instruction = new InlineMethodILInstruction(this.Assembly, code, BitConverter.ToUInt32(fromILBytes, i + 1));
                        i += 4;
                        break;
                    case OperandType.InlineType:
                        instruction = new InlineTypeILInstruction(this.Assembly, code, BitConverter.ToUInt32(fromILBytes, i + 1));
                        i += 4;
                        break;
                    case OperandType.InlineTok:
                        instruction = new InlineTokenILInstruction(this.Assembly, code, BitConverter.ToUInt32(fromILBytes, i + 1));
                        i += 4;
                        break;
                    case OperandType.InlineSwitch:
                        Int32 cases = BitConverter.ToInt32(fromILBytes, i + 1);
                        i += 4;
                        Int32[] jumpTargets = new Int32[cases];
                        for(Int32 counter = 0; counter < cases; counter++)
                        {
                            jumpTargets[counter] = BitConverter.ToInt32(fromILBytes, i + 1);
                            i += 4;
                        }
                        instruction = new InlineSwitchILInstruction(code, jumpTargets);
                        break;
                    default:
                        instruction = new ILInstruction(code);
                        break;
                }
                il.Add(instruction);
            }
            return il.ToArray();
        }

        /// <summary>
        /// Obtains a display ready version of the method name.
        /// </summary>
        /// <param name="includeNamespace">Indicates if the namespace should be included, this will include the type name.</param>
        /// <param name="includeParameters">Indicates if the parameters should be included.</param>
        /// <returns>A string representing a display ready version of the MethodDef name.</returns>
        public string GetDisplayName(bool includeNamespace, bool includeParameters)
        {
            DisplayNameSignitureConvertor convertor = new DisplayNameSignitureConvertor(this, includeNamespace, includeParameters);
            return convertor.Convert();
        }

        /// <summary>
        /// Obtains a display ready version of the method name, which includes the parameters of the MethodDef.
        /// </summary>
        /// <param name="includeNamespace">Indicates if the namespace should be included, this will include the type name.</param>
        /// <returns>A string representing a display ready version of the MethodDef name.</returns>
        public string GetDisplayName(bool includeNamespace)
        {
            return this.GetDisplayName(includeNamespace, true);
        }

        /// <summary>
        /// The parameters for this method.
        /// </summary>
        public List<ParamDef> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        /// <summary>
        /// Indicates if this method is a generic method or not.
        /// </summary>
        public bool IsGeneric
        {
            get { return _isGeneric; }
            set { _isGeneric = value; }
        }

        /// <summary>
        /// Collection of the generic types defined against this method.
        /// </summary>
        /// <remarks>
        /// If you need the generic types from this type and all its parent classes
        /// then utilise the <see cref="GetGenericTypes"/> method instead.
        /// </remarks>
        /// <seealso cref="GetGenericTypes"/>
        public List<GenericTypeRef> GenericTypes
        {
            get { return _genericTypes; }
            set { _genericTypes = value; }
        }

        /// <summary>
        /// Indicates if this method has a special name which is interpreted by the runtime,
        /// this is generally associated with the getters and setters or properties and
        /// events.
        /// </summary>
        public bool IsSpecialName
        {
            get { return _isSpecialName; }
            set { _isSpecialName = value; }
        }

        /// <summary>
        /// Gets or sets an set of flags detailing generic information about this method.
        /// </summary>
        public MethodAttributes MethodAttributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public override Visibility MemberAccess
        {
            get
            {
                switch(MethodAttributes & MethodAttributes.MemberAccessMask)
                {
                    case MethodAttributes.Public:
                        return Visibility.Public;
                    case MethodAttributes.Assem:
                        return Visibility.Internal;
                    case MethodAttributes.FamANDAssem:
                        return Visibility.Internal;
                    case MethodAttributes.Family:
                        return Visibility.Protected;
                    case MethodAttributes.Private:
                        return Visibility.Private;
                    case MethodAttributes.FamORAssem:
                        return Visibility.InternalProtected;
                    default:
                        return Visibility.Internal;
                }
            }
        }

        /// <summary>
        /// Gets or sets a set of flags detailing the implementation details of this
        /// method.
        /// </summary>
        public MethodImplFlags ImplementationFlags
        {
            get { return _implementationFlags; }
            set { _implementationFlags = value; }
        }

        /// <summary>
        /// Denotes if this method is generated and or managed by the compiler and not
        /// the programmer.
        /// </summary>
        /// <remarks>
        /// There is no actual property in the metadata that informs us of this, we simply
        /// user information from existing properties.
        /// </remarks>
        public bool IsCompilerGenerated
        {
            get
            {
                bool isSpecial = this.IsSpecialName && !this.IsConstructor;
                bool crap = this.Name.StartsWith("<");
                bool isCompilerAttribute = ((MemberRef)this).Attributes.Find(attribute => attribute.Name == "CompilerGeneratedAttribute") != null;

                return (isSpecial || crap) || isCompilerAttribute;
            }
        }

        /// <summary>
        /// A boolean value indicating if this method is a conversion operator.
        /// </summary>
        public bool IsConversionOperator
        {
            get { return _isConversionOperator; }
            set { _isConversionOperator = value; }
        }

        /// <summary>
        /// Indicates if this method is an extension method or not.
        /// </summary>
        public bool IsExtensionMethod
        {
            get
            {
                // TODO: why are we casting to a base type to get access to a property which we have 
                //  created with the same name!
                List<CustomAttribute> attributes = ((MemberRef)this).Attributes;
                if(attributes.Count > 0)
                {
                    for(int i = 0; i < attributes.Count; i++)
                    {
                        if(attributes[i].Name == "ExtensionAttribute")
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private class MethodDefBuilder
        {
            private MetadataDirectory _metadata;
            private MetadataToDefinitionMap _map;
            private AssemblyDef _assembly;
            private TypeDef _container;
            private MethodMetadataTableRow _fromRow;
            private MetadataStream _metadataStream;
            private BlobStream _blobStream;
            private MethodDef _methodToBuild;
            private int _rowIndex;
            private int _endOfMethodIndex;

            private BuildReferences _references;

            public MethodDefBuilder(BuildReferences references, TypeDef container, MethodMetadataTableRow fromRow)
            {
                _metadata = references.Metadata;
                _map = references.Map;
                _assembly = references.Assembly;
                _container = container;
                _fromRow = fromRow;
                _metadataStream = _metadata.Streams[Streams.MetadataStream] as MetadataStream;
                _blobStream = _metadata.Streams[Streams.BlobStream] as BlobStream;

                _references = references;
            }

            public MethodDef Build()
            {
                if(_methodToBuild != null) throw new InvalidOperationException("Can not use the same builder twice");

                _methodToBuild = new MethodDef();

                SetMethodProperties();
                CalculateIndexes();
                LoadGenericParameters();
                LoadParameters();

                return _methodToBuild;
            }

            private void LoadParameters()
            {
                MetadataTablesDictionary tables = _metadataStream.Tables;

                if(tables.ContainsKey(MetadataTables.Param))
                {
                    for(int i = _fromRow.ParamList; i < _endOfMethodIndex; i++)
                    {
                        ParamMetadataTableRow metadataRow = tables[MetadataTables.Param][i - 1] as ParamMetadataTableRow;
                        ParamDef param = new ParamDef(_references, _methodToBuild, metadataRow);

                        _map.Add(MetadataTables.Param, metadataRow, param);
                        _methodToBuild.Parameters.Add(param);
                    }
                }

                if(_methodToBuild.IsSpecialName && _methodToBuild.Name.Contains("set_Item"))
                {
                    // for setter methods on indexers the last parameter is actually the return value
                    _methodToBuild.Parameters.RemoveAt(_methodToBuild.Parameters.Count - 1);
                }
            }

            private void LoadGenericParameters()
            {
                CallingConventions callingConvention = MethodDefSigniture.GetCallingConvention(
                    _blobStream.GetSignitureContents((int)_methodToBuild.SignitureBlob.Value)
                    );
                bool isGenericMethod = (callingConvention & CallingConventions.Generic) != 0;

                if(isGenericMethod)
                {
                    List<GenericParamMetadataTableRow> genericParameters = _metadataStream.Tables.GetGenericParametersFor(
                        MetadataTables.MethodDef, _rowIndex + 1);
                    if(genericParameters.Count > 0)
                    {
                        _methodToBuild.IsGeneric = true;
                        foreach(GenericParamMetadataTableRow genParam in genericParameters)
                        {
                            _methodToBuild.GenericTypes.Add(
                                GenericTypeRef.CreateFromMetadata(_references, genParam)
                                );
                        }
                    }
                }
            }

            private void CalculateIndexes()
            {
                _rowIndex = _metadataStream.Tables.GetIndexFor(MetadataTables.MethodDef, _fromRow);
                int nextRow = _rowIndex < _metadataStream.Tables[MetadataTables.MethodDef].Length - 1
                    ? _rowIndex + 1
                    : -1;
                _endOfMethodIndex = 0;

                // calculate the end of method index so we have the start and end of list of parameters
                if(_metadataStream.Tables.ContainsKey(MetadataTables.Param))
                {
                    _endOfMethodIndex = _metadataStream.Tables[MetadataTables.Param].Length + 1;
                    if(nextRow != -1)
                    {
                        _endOfMethodIndex = ((MethodMetadataTableRow)_metadataStream.Tables[MetadataTables.MethodDef][nextRow]).ParamList;
                    }
                }
            }

            private void SetMethodProperties()
            {
                _methodToBuild.GenericTypes = new List<GenericTypeRef>();
                _methodToBuild.Parameters = new List<ParamDef>();
                _methodToBuild.UniqueId = _assembly.CreateUniqueId();
                _methodToBuild.Assembly = _assembly;
                _methodToBuild.Type = _container;
                _methodToBuild._rva = _fromRow.RVA;
                _methodToBuild.Name = _assembly.StringStream.GetString(_fromRow.Name.Value);
                _methodToBuild.SignitureBlob = _fromRow.Signiture;
                // Set flag based information
                _methodToBuild._isSpecialName = (_fromRow.Flags & MethodAttributes.SpecialName) != 0;
                _methodToBuild._attributes = _fromRow.Flags;
                _methodToBuild._implementationFlags = _fromRow.ImplFlags;
                _methodToBuild.Assembly = _assembly;

                // See details of MemberRef implementation for issues with this!
                if(_methodToBuild.Name.Length > 0)
                {
                    _methodToBuild.IsConstructor = _methodToBuild.Name[0] == '.';
                    _methodToBuild.IsOperator = !_methodToBuild.IsConstructor && _methodToBuild.Name.StartsWith("op_");
                    if(_methodToBuild.IsOperator)
                    {
                        _methodToBuild._isConversionOperator = _methodToBuild.Name == "op_Explicit" || _methodToBuild.Name == "op_Implicit";
                    }
                }
            }
        }
    }
}