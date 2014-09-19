using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace DocumentationTest
{
	// class PInvokeInterfaceTest
	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("7BF80980-BF32-101A-8BBB-00AA00300CAB")]
	public interface PInvokeInterfaceTest
	{
		[SuppressUnmanagedCodeSecurity]
		IntPtr GetHandle();

		[SuppressUnmanagedCodeSecurity]
		IntPtr GetHPal();

		//[return: MarshalAs(UnmanagedType.I2)]
		//[SuppressUnmanagedCodeSecurity]
		//short GetPictureType();

		[SuppressUnmanagedCodeSecurity]
		int GetWidth();

		[SuppressUnmanagedCodeSecurity]
		int GetHeight();

		[SuppressUnmanagedCodeSecurity]
		void Render();

		[SuppressUnmanagedCodeSecurity]
		void SetHPal([In] IntPtr phpal);

		[SuppressUnmanagedCodeSecurity]
		IntPtr GetCurDC();

		[SuppressUnmanagedCodeSecurity]
		void SelectPicture([In] IntPtr hdcIn, 
			[Out, MarshalAs(UnmanagedType.LPArray)] int[] phdcOut, 
			[Out, MarshalAs(UnmanagedType.LPArray)] int[] phbmpOut
			);

		//[return: MarshalAs(UnmanagedType.Bool)]
		//[SuppressUnmanagedCodeSecurity]
		//bool GetKeepOriginalFormat();

		[SuppressUnmanagedCodeSecurity]
		void SetKeepOriginalFormat([In, MarshalAs(UnmanagedType.Bool)] bool pfkeep);

		[SuppressUnmanagedCodeSecurity]
		void PictureChanged();

		[SuppressUnmanagedCodeSecurity]
		int GetAttributes();

		[SuppressUnmanagedCodeSecurity]
		void SetHdc([In] IntPtr hdc);
	}
}
