using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Signitures {
	public enum ElementTypes {
		End = 0x0,
		Void = 0x1,
		Boolean = 0x2,
		Char = 0x3,
		I1 = 0x4,
		U1 = 0x5,
		I2 = 0x6,
		U2 = 0x7,
		I4 = 0x8,
		U4 = 0x9,
		I8 = 0xa,
		U8 = 0xb,
		R4 = 0xc,
		R8 = 0xd,
		String = 0xe,

		// every type above PTR will be simple type
		Ptr = 0xf,          // PTR <type>
		ByRef = 0x10,       // BYREF <type>

		// Please use ELEMENT_TYPE_VALUETYPE. ELEMENT_TYPE_VALUECLASS is deprecated.
		ValueType = 0x11,     // VALUETYPE <class Token>
		Class = 0x12,     // CLASS <class Token>
		Var = 0x13,     // a class type variable VAR <U1>
		Array = 0x14,     // MDARRAY <type> <rank> <bcount> <bound1> ... <lbcount> <lb1> ...
		GenericInstance = 0x15,     // GENERICINST <generic type> <argCnt> <arg1> ... <argn>
		TypedByRef = 0x16,     // TYPEDREF  (it takes no args) a typed referece to some other type

		I = 0x18,     // native integer size
		U = 0x19,     // native unsigned integer size
		FunctionPointer = 0x1B,     // FNPTR <complete sig for the function including calling convention>
		Object = 0x1C,     // Shortcut for System.Object
		SZArray = 0x1D,     // Shortcut for single dimension zero lower bound array
		// SZARRAY <type>
		MVar = 0x1e,     // a method type variable MVAR <U1>

		// This is only for binding
		CModRequired = 0x1F,     // required C modifier : E_T_CMOD_REQD <mdTypeRef/mdTypeDef>
		CModOptional = 0x20,     // optional C modifier : E_T_CMOD_OPT <mdTypeRef/mdTypeDef>

		// This is for signatures generated internally (which will not be persisted in any way).
		Internal = 0x21,     // INTERNAL <typehandle>

		// Note that this is the max of base type excluding modifiers
		Max = 0x22,     // first invalid element type


		Modifier = 0x40,
		Sentinal = 0x01 | Modifier, // sentinel for varargs
		Pinned = 0x05 | Modifier,
		R4_HFA = 0x06 | Modifier, // used only internally for R4 HFA types
		R8_HFA = 0x07 | Modifier, // used only internally for R8 HFA types
	}
}
