
namespace TheBoxSoftware.Reflection.Signitures
{
    using System;

    [Flags]
    internal enum CallingConventions
    {
        /// <summary>
        /// Default ("normal") method with a fixed-length argument list. ILAsm has no
        /// keyword for this calling convention.
        /// </summary>
        Default     = 0x00,
        StdCall     = 0x02,
        ThisCall    = 0x03,
        FastCall    = 0x04,
        /// <summary>
        /// Method with a variable-length argument list. The ILAsm keyword is <i>vararg</i>.
        /// </summary>
        VarArg      = 0x05,
        /// <summary>
        /// Field. ILAsm has no keyword for this calling convention.
        /// </summary>
        Field       = 0x6,
        /// <summary>
        /// Local variables. ILAsm has no keyword for this calling convention.
        /// </summary>
        LocalSig    = 0x7,
        /// <summary>
        /// Property. ILAsm has no keyword for this calling convention.
        /// </summary>
        Property    = 0x8,
        /// <summary>
        /// Unmanaged calling convention, not currently used by the runtime and not
        /// recognised by ILAsm
        /// </summary>
        Unmanaged   = 0x9,
        GenericInst = 0xa,
        NativeVarArg = 0xb,
        Max         = 0xc,
        Mask        = 0xf,
        Generic     = 0x10,
        /// <summary>
        /// Instance method that has an instance pointer (this) as an implicit first
        /// argument.
        /// </summary>
        HasThis     = 0x20,
        /// <summary>
        /// Method call signiture. This first explicitly specified parameter is the
        /// instance pointer. The ILAsm keywork is <i>explicit</i>.
        /// </summary>
        ExplicitThis = 0x40
    }
}