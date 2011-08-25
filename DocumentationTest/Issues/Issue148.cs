using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace DocumentationTest.Issues
{
	class Issue148
	{
		[return: MarshalAs(UnmanagedType.Bool)]
		[SuppressUnmanagedCodeSecurity]
		bool GetKeepOriginalFormat();
	}
}
