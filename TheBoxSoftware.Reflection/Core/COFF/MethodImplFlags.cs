using System;

namespace TheBoxSoftware.Reflection.Core.COFF {
	/// <summary>
	/// Flags describing a <see cref="MethodMetadataRow"/>s implementation.
	/// </summary>
	/// <seealso cref="MethodMetadataRow"/>
	[Flags]
	public enum MethodImplFlags {
		/// <summary>
		/// These 2 bits contain one of the following values:
		/// </summary>
		CodeTypeMask		= 0x0003,
		/// <summary>Method impl is CIL</summary>
		IL					= 0x0000,
		/// <summary>Method impl is native</summary>
		Native				= 0x0001,
		/// <summary>
		/// Reserved: shall be zero in conforming implementations
		/// </summary>
		OPTIL				= 0x0002,
		/// <summary>
		/// Method impl is provided by the runtime
		/// </summary>
		Runtime				= 0x0003,

		/// <summary>
		/// Flags specifying whether the code is managed or unmanaged. This bit contains 
		/// one of the following values:
		/// </summary>
		ManagedMask			= 0x0004,
		/// <summary>Method impl is unmanaged, otherwise managed</summary>
		Unmanaged			= 0x0004,
		/// <summary>Method impl is managed</summary>
		Managed				= 0x0000,

		/// <summary>
		/// Implementation info and interop Indicates method is defined; used primarily 
		/// in merge scenarios.
		/// </summary>
		ForwardRef			= 0x0010,
		/// <summary>
		/// Reserved: conforming implementations can ignore
		/// </summary>
		PreserveSig			= 0x0080,
		/// <summary>
		/// Reserved: shall be zero in conforming implementations
		/// </summary>
		InternalCall		= 0x1000,
		/// <summary>
		/// Method is single threaded through the body
		/// </summary>
		Synchronized		= 0x0020,
		/// <summary>Method cannot be inlined</summary>
		NoInlining			= 0x0008,
		/// <summary>Range check value</summary>
		MaxMethodImplVal	= 0xffff
	}
}
