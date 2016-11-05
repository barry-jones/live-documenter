
namespace TheBoxSoftware.Reflection.Core.COFF
{
    /// <summary>
    /// Enumeration of all the available metadata tables that can reside
    /// in a .net pe/coff file.
    /// </summary>
    public enum MetadataTables : byte
    {
        Module					= 0x00,
		TypeRef					= 0x01,
		TypeDef					= 0x02,
		Unused1					= 0x03,
		Field					= 0x04,
		Unused2					= 0x05,
		MethodDef				= 0x06,
		Unused3					= 0x07,
		Param					= 0x08,
		InterfaceImpl			= 0x09,
		MemberRef				= 0x0A,
		Constant				= 0x0B,
		CustomAttribute			= 0x0C,
		FieldMarshal			= 0x0D,
		DeclSecurity			= 0x0E,
		ClassLayout				= 0x0F,
		FieldLayout				= 0x10,
		StandAloneSig			= 0x11,
		EventMap				= 0x12,
		Unused4					= 0x13,
		Event					= 0x14,
		PropertyMap				= 0x15,
		Unused5					= 0x16,
		Property				= 0x17,
		MethodSemantics			= 0x18,
		MethodImpl				= 0x19,
		ModuleRef				= 0x1A,
		TypeSpec				= 0x1B,
		ImplMap					= 0x1C,
		FieldRVA				= 0x1D,
		Unused6					= 0x1E,
		Unused7					= 0x1F,
		Assembly				= 0x20,
		AssemblyProcessor		= 0x21,
		AssemblyOS				= 0x22,
		AssemblyRef				= 0x23,
		AssemblyRefProcessor	= 0x24,
		AssemblyRefOS			= 0x25,
		File					= 0x26,
		ExportedType			= 0x27,
		ManifestResource		= 0x28,
		NestedClass				= 0x29,
		GenericParam			= 0x2A,
		MethodSpec				= 0x2B,
		GenericParamConstraint	= 0x2C,
		Unused8					= 0x2D,
		Unused9					= 0x2E,
		Unused10				= 0x2F
    }
}