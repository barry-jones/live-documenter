
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System;

    [Flags]
    public enum GenericParamAttributes : short
    {
        None                            = 0x0000,

        VarianceMask                    = 0x0003,
        Covariant                       = 0x0001,
        Contravariant                   = 0x0002,

        SpecialConstraintMask           = 0x001C,
        ReferenceTypeConstraint         = 0x0004,
        NotNullableValuleTypeConstraint = 0x0008,
        DefaultConstructorConstraint    = 0x0010
    }
}
