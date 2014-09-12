using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	public enum CodedIndexes : byte {
		TypeDefOrRef,
		HasConstant,
		HasCustomAttribute,
		HasFieldMarshall,
		HasDeclSecurity,
		MemberRefParent,
		HasSemantics,
		MethodDefOrRef,
		MemberForwarded,
		Implementation,
		CustomAttributeType,
		ResolutionScope,
		TypeOrMethodDef
	}
}
