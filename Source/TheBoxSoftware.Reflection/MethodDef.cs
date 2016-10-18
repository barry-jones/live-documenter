using System;
using System.Collections.Generic;
using System.Linq;
using TheBoxSoftware.Reflection.Core.COFF;
using TheBoxSoftware.Reflection.Signitures;

namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// Represents a method definition as a reflected element.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Method Name={Name}")]
    public class MethodDef : MemberRef
    {
        private const int METHOD_BODY_SIZE_MASK = 0x03;

        /// <summary>
        /// Obtains the list of generic types that are defined and owned only by this member.
        /// </summary>
        /// <returns>A collection of generic types for this member</returns>
        public List<GenericTypeRef> GetGenericTypes()
        {
            return this.GenericTypes;
        }

        /// <summary>
        /// Initialises a new instance of MethodDef for the provided data
        /// </summary>
        /// <param name="assembly">The assembly this method is a part of</param>
        /// <param name="container">The owning type for this method</param>
        /// <param name="metadata">The metadata directory</param>
        /// <param name="row">The row detailing the method</param>
        /// <returns>The initialised MethodDef</returns>
        internal static MethodDef CreateFromMetadata(
                AssemblyDef assembly,
                TypeDef container,
                MetadataDirectory metadata,
                MethodMetadataTableRow row)
        {
            MetadataToDefinitionMap map = assembly.File.Map;
            MethodDef method = new MethodDef();

            method.GenericTypes = new List<GenericTypeRef>();
            method.UniqueId = assembly.CreateUniqueId();
            method.Assembly = assembly;
            method.Type = container;
            method.RVA = (int)row.RVA;
            method.Name = assembly.StringStream.GetString(row.Name.Value);
            method.SignitureBlob = row.Signiture;
            // Set flag based information
            method.IsSpecialName = (row.Flags & MethodAttributes.SpecialName) == MethodAttributes.SpecialName;
            method.Attributes = row.Flags;
            method.ImplementationFlags = row.ImplFlags;
            method.Assembly = assembly;

            // See details of MemberRef implementation for issues with this!
            method.IsConstructor = method.Name.StartsWith(".");
            method.IsOperator = method.Name.StartsWith("op_");
            method.IsConversionOperator = method.Name == "op_Explicit" || method.Name == "op_Implicit";

            // Load the parameters for this method
            MetadataStream metadataStream = (MetadataStream)metadata.Streams[Streams.MetadataStream];
            int rowIndex = metadataStream.Tables.GetIndexFor(MetadataTables.MethodDef, row);
            int nextRow = rowIndex < metadataStream.Tables[MetadataTables.MethodDef].Length - 1
                ? rowIndex + 1
                : -1;
            int endOfMethodIndex = 0;
            if(metadataStream.Tables.ContainsKey(MetadataTables.Param))
            {
                endOfMethodIndex = metadataStream.Tables[MetadataTables.Param].Length + 1;
                if(nextRow != -1)
                {
                    endOfMethodIndex = ((MethodMetadataTableRow)metadataStream.Tables[MetadataTables.MethodDef][nextRow]).ParamList;
                }
            }

            BlobStream stream = (BlobStream)((Core.COFF.CLRDirectory)assembly.File.Directories[
                Core.PE.DataDirectories.CommonLanguageRuntimeHeader]).Metadata.Streams[Streams.BlobStream];
            if((Signitures.MethodDefSigniture.GetCallingConvention(assembly.File,
                stream.GetSignitureContents((int)method.SignitureBlob.Value)) & CallingConventions.Generic) == CallingConventions.Generic)
            {
                List<GenericParamMetadataTableRow> genericParameters = metadataStream.Tables.GetGenericParametersFor(
                    MetadataTables.MethodDef, rowIndex + 1);
                if(genericParameters.Count > 0)
                {
                    method.IsGeneric = true;
                    foreach(GenericParamMetadataTableRow genParam in genericParameters)
                    {
                        method.GenericTypes.Add(GenericTypeRef.CreateFromMetadata(
                            assembly, metadata, genParam
                            ));
                    }
                }
            }

            // Now load all the methods between our index and the endOfMethodIndex
            method.Parameters = new List<ParamDef>();
            if(metadataStream.Tables.ContainsKey(MetadataTables.Param))
            {
                for(int i = row.ParamList; i < endOfMethodIndex; i++)
                {
                    ParamMetadataTableRow metadataRow = (ParamMetadataTableRow)metadataStream.Tables[MetadataTables.Param][i - 1];
                    ParamDef param = ParamDef.CreateFromMetadata(method, metadata, metadataRow);
                    map.Add(MetadataTables.Param, metadataRow, param);
                    method.Parameters.Add(param);
                }
            }

            if(method.IsSpecialName && method.Name.Contains("set_Item"))
            {
                // for setter methods on indexers the last parameter is actually the return value
                method.Parameters.RemoveAt(method.Parameters.Count - 1);
            }

            return method;
        }

        /// <summary>
        /// Obtains the details about the IL and body of this method and the contents
        /// of the MSIL.
        /// </summary>
        /// <returns>The instantiated <see cref="MethodBody"/>.</returns>
        public MethodBody GetMethodBody()
        {
            Int16 maxStack = 0;
            Int32 localsToken;
            Int32 codeSize = 0;
            int address = this.Assembly.File.FileAddressFromRVA(this.RVA);
            List<byte> contents = new List<byte>(this.Assembly.File.FileContents);

            byte sizeMask = 0x03;
            byte firstByte = contents[address];
            byte[] instructions = new byte[0];
            if((firstByte & sizeMask) == 0x02)
            {       // Tiny
                codeSize = firstByte >> 2;
                maxStack = 8;
                address++;
            }
            else if((firstByte & sizeMask) == 0x03)
            {   // FAT
                TheBoxSoftware.Reflection.Core.Offset offset = address;
                uint flagsAndSize = BitConverter.ToUInt16(this.Assembly.File.FileContents, offset.Shift(2));
                uint lengthOfHeader = flagsAndSize >> 12;
                maxStack = BitConverter.ToInt16(this.Assembly.File.FileContents, offset.Shift(2));
                codeSize = BitConverter.ToInt32(this.Assembly.File.FileContents, offset.Shift(4));
                localsToken = BitConverter.ToInt32(this.Assembly.File.FileContents, offset.Shift(4));
                address = offset;
            }

            // Popualate details of the method body
            MethodBody body = new MethodBody(
                this.GetInstructions(this.GetIL(address, codeSize)).ToList(),
                maxStack
                );
            return body;
        }

        /// <summary>
        /// Obtains the Intermediate Language instructions for this method
        /// </summary>
        /// <param name="address">The address of the start of the actual code</param>
        /// <param name="codeSize">The size of the actual code.</param>
        /// <returns>A byte array of IL operations</returns>
        private byte[] GetIL(int address, int codeSize)
        {
            List<byte> contents = new List<byte>(this.Assembly.File.FileContents);
            return contents.GetRange(address, (int)codeSize).ToArray();
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
                        instruction = new InlineFieldILInstruction(this.Assembly, code, BitConverter.ToInt32(fromILBytes, i + 1));
                        i += 4;
                        break;
                    case OperandType.InlineMethod:
                        instruction = new InlineMethodILInstruction(this.Assembly, code, BitConverter.ToInt32(fromILBytes, i + 1));
                        i += 4;
                        break;
                    case OperandType.InlineType:
                        instruction = new InlineTypeILInstruction(this.Assembly, code, BitConverter.ToInt32(fromILBytes, i + 1));
                        i += 4;
                        break;
                    case OperandType.InlineTok:
                        instruction = new InlineTokenILInstruction(this.Assembly, code, BitConverter.ToInt32(fromILBytes, i + 1));
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
        /// A reference to the return type for this method.
        /// </summary>
        //public TypeRef ReturnType { get; set; }

        /// <summary>
        /// The parameters for this method.
        /// </summary>
        public List<ParamDef> Parameters { get; set; }

        /// <summary>
        /// The RVA for the methods IL body.
        /// </summary>
        private int RVA { get; set; }

        /// <summary>
        /// Indicates if this method is a generic method or not.
        /// </summary>
        public bool IsGeneric { get; set; }

        /// <summary>
        /// Collection of the generic types defined against this method.
        /// </summary>
        /// <remarks>
        /// If you need the generic types from this type and all its parent classes
        /// then utilise the <see cref="GetGenericTypes"/> method instead.
        /// </remarks>
        /// <seealso cref="GetGenericTypes"/>
        public List<GenericTypeRef> GenericTypes { get; set; }

        /// <summary>
        /// Indicates if this method has a special name which is interpreted by the runtime,
        /// this is generally associated with the getters and setters or properties and
        /// events.
        /// </summary>
        public bool IsSpecialName { get; set; }

        /// <summary>
        /// Gets or sets an set of flags detailing generic information about this method.
        /// </summary>
        public MethodAttributes Attributes { get; set; }

        public override Visibility MemberAccess
        {
            get
            {
                switch(this.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.MemberAccessMask)
                {
                    case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Public:
                        return Visibility.Public;
                    case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Assem:
                        return Visibility.Internal;
                    case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.FamANDAssem:
                        return Visibility.Internal;
                    case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Family:
                        return Visibility.Protected;
                    case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Private:
                        return Visibility.Private;
                    case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.FamORAssem:
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
        public MethodImplFlags ImplementationFlags { get; set; }

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
        public bool IsConversionOperator { get; set; }

        /// <summary>
        /// Indicates if this method is an extension method or not.
        /// </summary>
        public bool IsExtensionMethod
        {
            get
            {
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
    }
}