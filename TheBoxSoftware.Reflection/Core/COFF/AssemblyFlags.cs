using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum AssemblyFlags {
		/// <summary>
		/// The assembly reference holds the full (unhashed) public key.
		/// </summary>
		PublicKey					= 0x0001,
		/// <summary>
		/// The assembly is side-by-side compatible.
		/// </summary>
		SideBySideCompatible		= 0x0000,
		/// <summary>
		/// Reserved
		/// </summary>
		Reserved					= 0x0030,
		/// <summary>
		/// The implementation of this assembly at runtime is not expected
		/// to match the version seen at compile time.
		/// </summary>
		Retargetable				= 0x0100,
		/// <summary>
		/// Reserved. A conforming implementation of the CLI can ignore this
		/// setting on read; some implementations misht use this bit to indicate
		/// that a CIL-to-native-code compiler should generate CIL-to-native-code
		/// map.
		/// </summary>
		EnableJitCompileTracking	= 0x8000,
		/// <summary>
		/// Reserved (a conforming implementation of the CLI can ignor ethis
		/// setting on read; some implementations might use this bit to indicate
		/// that a CIL-to-native-code compiler should not generate optimised
		/// code.
		/// </summary>
		DisableJitCompileOptimizer	= 0x4000
	}
}
