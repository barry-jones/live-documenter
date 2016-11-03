
namespace TheBoxSoftware.Reflection
{
    using System;
    using TheBoxSoftware.Reflection.Core.COFF;

    /// <summary>
    /// Represents a single Intermediate Language Instruction
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Opcode={OpCode}")]
    public class ILInstruction
    {
        private OpCode _code;

        /// <summary>
        /// Initialises a new instance of the ILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        internal ILInstruction(OpCode code)
        {
            _code = code;
        }

        /// <summary>
        /// The OpCode of the IL Instruction
        /// </summary>
        public OpCode OpCode
        {
            get { return _code; }
        }
    }

    /// <summary>
    /// Represents an IL Instruction of Type InlineNone
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Opcode={OpCode}")]
    public class InlineNoneILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the InlineNoneILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        internal InlineNoneILInstruction(OpCode code)
            : base(code)
        {
        }
    }

    [System.Diagnostics.DebuggerDisplay("Opcode={OpCode}, Argument=")]
    public class ArgInlineNoneILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the InlineNoneILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        internal ArgInlineNoneILInstruction(OpCode code)
            : base(code)
        {
        }
    }

    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, Target={Target}")]
    public class InlineBrTargetILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the InlineBrTargetILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        /// <param name="target">The offset target to break to</param>
        internal InlineBrTargetILInstruction(OpCode code, Int32 target)
            : base(code)
        {
            Target = target;
        }

        /// <summary>
        /// The target offset for this instruction
        /// </summary>
        public int Target { get; set; }
    }

    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, Constant={Constant}")]
    public class ShortInlineIILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the ShortInlineIILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        /// <param name="target">The short target offset to </param>
        internal ShortInlineIILInstruction(OpCode code, byte constant) :
            base(code)
        {
            Constant = constant;
        }

        /// <summary>
        /// The constant being pushed on to the stack in this operation
        /// </summary>
        public uint Constant { get; set; }
    }

    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, Constant={Constant}")]
    public class ShortInlineVarILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the ShortInlineVarILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        /// <param name="target">The short target offset to </param>
        internal ShortInlineVarILInstruction(OpCode code, byte constant) :
            base(code)
        {
            Constant = constant;
        }

        /// <summary>
        /// The constant being pushed on to the stack in this operation
        /// </summary>
        public uint Constant { get; set; }
    }

    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, Constant={Constant}")]
    public class ShortInlineRILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the ShortInlineRILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        /// <param name="target">The short target offset to </param>
        internal ShortInlineRILInstruction(OpCode code, Single constant) :
            base(code)
        {
            Constant = constant;
        }

        /// <summary>
        /// The constant being pushed on to the stack in this operation
        /// </summary>
        public float Constant { get; set; }
    }

    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, Constant={Constant}")]
    public class InlineIILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the InlineIILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        /// <param name="constant">The integer constant to be pushed on to the stack</param>
        internal InlineIILInstruction(OpCode code, uint constant)
            : base(code)
        {
            Constant = constant;
        }

        /// <summary>
        /// The integer constant to be pushed on to the stack
        /// </summary>
        public uint Constant { get; set; }
    }

    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, Constant={Constant}")]
    public class InlineVarILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the InlineVarILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        /// <param name="constant">The integer constant to be pushed on to the stack</param>
        internal InlineVarILInstruction(OpCode code, UInt16 constant)
            : base(code)
        {
            Constant = constant;
        }

        /// <summary>
        /// The integer constant to be pushed on to the stack
        /// </summary>
        public ushort Constant { get; set; }
    }

    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, StringToken={UDStringToken}, String={String}")]
    public class InlineStringILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the InlineVarILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        /// <param name="constant">The integer constant to be pushed on to the stack</param>
        internal InlineStringILInstruction(AssemblyDef assembly, OpCode code, Int32 udStringToken)
            : base(code)
        {
            UDStringToken = udStringToken;
        }

        /// <summary>
        /// The integer constant to be pushed on to the stack
        /// </summary>
        /// <remarks>
        /// THIS WILL BE REMOVED WHEN THE STRING IS PROPERLY LOADED
        /// </remarks>
        public int UDStringToken { get; set; }

        /// <summary>
        /// The string being loaded by the instruction
        /// </summary>
        public string String { get; set; }
    }

    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, SignitureToken={SignitureToken}")]
    public class InlineSigILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the InlineSigILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        /// <param name="constant">The integer constant to be pushed on to the stack</param>
        internal InlineSigILInstruction(AssemblyDef assembly, OpCode code, int signitureToken)
            : base(code)
        {
            SignitureToken = signitureToken;
        }

        /// <summary>
        /// The integer constant to be pushed on to the stack
        /// </summary>
        /// <remarks>
        /// THIS WILL BE REMOVED WHEN THE STRING IS PROPERLY LOADED
        /// </remarks>
        public int SignitureToken { get; set; }
    }

    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, Constant={Constant}")]
    public class InlineRILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the InlineRILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        /// <param name="constant">The integer constant to be pushed on to the stack</param>
        internal InlineRILInstruction(OpCode code, Double constant)
            : base(code)
        {
            Constant = constant;
        }

        /// <summary>
        /// The integer constant to be pushed on to the stack
        /// </summary>
        public double Constant { get; set; }
    }

    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, Constant={Constant}")]
    public class InlineI8ILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the InlineIILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        /// <param name="constant">The long constant to be pushed on to the stack</param>
        internal InlineI8ILInstruction(OpCode code, UInt64 constant)
            : base(code)
        {
            Constant = constant;
        }

        /// <summary>
        /// The long constant to be pushed on to the stack
        /// </summary>
        public ulong Constant { get; set; }
    }

    /// <summary>
    /// Represents an IL instruction of type ShortInlineBrTarget
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Opcode={OpCode}, Target={Target}")]
    public class ShortInlineBrTargetILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the ShortInlineBrTargetILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        /// <param name="target">The targetes offset</param>
        internal ShortInlineBrTargetILInstruction(OpCode code, byte target)
            : base(code)
        {
            Target = target;
        }

        public byte Target { get; set; }
    }

    /// <summary>
    /// Represents an IL instruction of type InlineMethod
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, Method={Method.Name}")]
    public class InlineMethodILInstruction : ILInstruction
    {
        private MemberRef _methodDef;

        /// <summary>
        /// Initialises a new instance of the InlineMethodILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        internal InlineMethodILInstruction(AssemblyDef assembly, OpCode code, uint metadataToken)
            : base(code)
        {
            _methodDef = assembly.ResolveMetadataToken(metadataToken) as MemberRef;
        }

        /// <summary>
        /// The definition of the method this instruction calls
        /// </summary>
        public MemberRef Method
        {
            get { return _methodDef; }
        }
    }

    /// <summary>
    /// Represents an IL instruction of type InlineMethod
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, Type={Type.Name}")]
    public class InlineTypeILInstruction : ILInstruction
    {
        private ILMetadataToken _token;
        private uint _index;
        private TypeRef _typeDef;

        /// <summary>
        /// Initialises a new instance of the InlineMethodILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        internal InlineTypeILInstruction(AssemblyDef assembly, OpCode code, uint metadataToken)
            : base(code)
        {
            MetadataToDefinitionMap map = assembly.File.Map;
            Core.COFF.MetadataStream metadataStream = assembly.File.GetMetadataDirectory().GetMetadataStream();

            // Get the details in the token
            _token = (ILMetadataToken)(metadataToken & 0xff000000);
            _index = metadataToken & 0x00ffffff;

            // 
            if(_token == ILMetadataToken.TypeDef)
            {
                _typeDef = (TypeDef)map.GetDefinition(MetadataTables.TypeDef, metadataStream.GetEntryFor(MetadataTables.TypeDef, _index));
            }
            else if(_token == ILMetadataToken.TypeRef)
            {
                _typeDef = (TypeRef)map.GetDefinition(MetadataTables.TypeRef, metadataStream.GetEntryFor(MetadataTables.TypeRef, _index));
            }
            else if(_token == ILMetadataToken.TypeSpec)
            {
                _typeDef = (TypeDef)map.GetDefinition(MetadataTables.TypeSpec, metadataStream.GetEntryFor(MetadataTables.TypeSpec, _index));
            }
        }

        /// <summary>
        /// The definition of the method this instruction calls
        /// </summary>
        public TypeRef Type
        {
            get { return _typeDef; }
        }
    }

    /// <summary>
    /// Represents an IL instruction of type InlineMethod
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, Type={Type.Name}")]
    public class InlineTokenILInstruction : ILInstruction
    {
        private ILMetadataToken _token;
        private uint _index;
        private object _entry;

        /// <summary>
        /// Initialises a new instance of the InlineMethodILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        internal InlineTokenILInstruction(AssemblyDef assembly, OpCode code, uint metadataToken)
            : base(code)
        {
            MetadataToDefinitionMap map = assembly.File.Map;
            Core.COFF.MetadataStream metadataStream = assembly.File.GetMetadataDirectory().GetMetadataStream();

            // Get the details in the token
            _token = (ILMetadataToken)(metadataToken & 0xff000000);
            _index = metadataToken & 0x00ffffff;

            // 
            if(_token == ILMetadataToken.TypeDef)
            {
                _entry = map.GetDefinition(MetadataTables.TypeDef, metadataStream.GetEntryFor(MetadataTables.TypeDef, _index));
            }
            else if(_token == ILMetadataToken.TypeRef)
            {
                _entry = map.GetDefinition(MetadataTables.TypeRef, metadataStream.GetEntryFor(MetadataTables.TypeRef, _index));
            }
            else if(_token == ILMetadataToken.TypeSpec)
            {
                _entry = map.GetDefinition(MetadataTables.TypeSpec, metadataStream.GetEntryFor(MetadataTables.TypeSpec, _index));
            }
            else if(_token == ILMetadataToken.MethodDef)
            {
                _entry = map.GetDefinition(MetadataTables.MethodDef, metadataStream.GetEntryFor(MetadataTables.MethodDef, _index));
            }
            else if(_token == ILMetadataToken.MemberRef)
            {
                _entry = map.GetDefinition(MetadataTables.MemberRef, metadataStream.GetEntryFor(MetadataTables.MemberRef, _index));
            }
            else if(_token == ILMetadataToken.MethodSpec)
            {
                _entry = map.GetDefinition(MetadataTables.MethodSpec, metadataStream.GetEntryFor(MetadataTables.MethodSpec, _index));
            }
            else if(_token == ILMetadataToken.FieldDef)
            {
                _entry = map.GetDefinition(MetadataTables.Field, metadataStream.GetEntryFor(MetadataTables.Field, _index));
            }
        }

        /// <summary>
        /// The definition of the method this instruction calls
        /// </summary>
        public object Type
        {
            get { return _entry; }
        }
    }

    /// <summary>
    /// Represents an IL instruction of type InlineField
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, Field={Field.Name}")]
    public class InlineFieldILInstruction : ILInstruction
    {
        private ILMetadataToken _token;
        private uint _index;
        private MemberRef _fieldDef;

        /// <summary>
        /// Initialises a new instance of the InlineMethodILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        internal InlineFieldILInstruction(AssemblyDef assembly, OpCode code, uint metadataToken)
            : base(code)
        {
            MetadataToDefinitionMap map = assembly.File.Map;
            Core.COFF.MetadataStream metadataStream = assembly.File.GetMetadataDirectory().GetMetadataStream();

            // Get the details in the token
            _token = (ILMetadataToken)(metadataToken & 0xff000000);
            _index = metadataToken & 0x00ffffff;

            // 
            if(_token == ILMetadataToken.FieldDef)
            {
                _fieldDef = (FieldDef)map.GetDefinition(MetadataTables.Field, metadataStream.GetEntryFor(MetadataTables.Field, _index));
            }
            else if(_token == ILMetadataToken.MemberRef)
            {
                _fieldDef = (MemberRef)map.GetDefinition(MetadataTables.MemberRef, metadataStream.GetEntryFor(MetadataTables.MemberRef, _index));
            }
        }

        /// <summary>
        /// The definition of the method this instruction calls
        /// </summary>
        public MemberRef Field
        {
            get { return _fieldDef; }
        }
    }

    /// <summary>
    /// Represents an IL instruction of type InlineSwitch
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("OpCode={OpCode}, JumpTargets={JumpTargets.Length}")]
    public class InlineSwitchILInstruction : ILInstruction
    {
        /// <summary>
        /// Initialises a new instance of the InlineSwitchILInstruction class
        /// </summary>
        /// <param name="code">The OpCode describing the operation of the instruction</param>
        internal InlineSwitchILInstruction(OpCode code, Int32[] jumpTargets)
            : base(code)
        {
            JumpTargets = jumpTargets;
        }

        /// <summary>
        /// The offset targets for each of the case statements in the switch operation
        /// </summary>
        public int[] JumpTargets { get; set; }
    }
}