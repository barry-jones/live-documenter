using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    [Flags]
    public enum ParamAttributeFlags : UInt16
    {
        In = 0x0001,
        Out = 0x0002,
        Optional = 0x0010,

        // reserved runtime use only
        ReservedMask = 0xf000,
        HasDefault = 0x1000,
        HasFieldMarshal = 0x2000,

        Unused = 0xcfe0
    }
}
