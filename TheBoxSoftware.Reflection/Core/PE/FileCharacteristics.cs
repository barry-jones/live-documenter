using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.PE {

    [Flags]
    public enum FileCharacteristics : ushort {
        RelocsStripped = 0x0001,
        ExecutableImage = 0x002,
        LineNumbersStripped = 0x004,
        LocalSymbolsStripped = 0x008,
        AggressiveWhiteSpaceTrim = 0x0010,
        LargeAddressAware = 0x0020,
        BytesReversedLoEndian = 0x0080,
        Bit32Machine = 0x0100,
        debugStripped = 0x0200,
        RemovableRunFromSwap = 0x0400,
        NetRunFromSwap = 0x0800,
        System = 0x1000,
        Dll = 0x2000,
        UpSystemOnly = 0x4000,
        BytesReversedHiEndian = 0x8000
    }
}
