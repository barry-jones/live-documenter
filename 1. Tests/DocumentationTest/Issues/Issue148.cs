using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace DocumentationTest.Issues
{
	interface Issue148
	{
		[return: MarshalAs(UnmanagedType.Bool)]
		[SuppressUnmanagedCodeSecurity]
		bool GetKeepOriginalFormat();

		[return: MarshalAs(UnmanagedType.I4)]
		void CollectData([In, MarshalAs(UnmanagedType.I4)] int id, 
			[In, MarshalAs(UnmanagedType.SysInt)] IntPtr valueName, 
			[In, MarshalAs(UnmanagedType.SysInt)] IntPtr data, 
			[In, MarshalAs(UnmanagedType.I4)] int totalBytes, 
			[MarshalAs(UnmanagedType.SysInt)] out IntPtr res
			);
	}
}
