﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection {
	/// <summary>
	/// Enumeration of members that can be defined in an assembly.
	/// </summary>
	public enum MemberTypes : byte {
		Property,
		Field,
		Method,
		Constructor,
		Event
	}
}
