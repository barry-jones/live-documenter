using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF {
	using TheBoxSoftware.Reflection.Core.PE;

	public class CLRDirectory : Directory {
		#region Constructors
		public CLRDirectory(byte[] fileContents, int address)
			: base() {
			this.Header = new Cor20Header(fileContents, address);
		}
		#endregion

		#region Methods
		public override void ReadDirectories(PeCoffFile containingFile) {
			base.ReadDirectories(containingFile);

			this.Metadata = new MetadataDirectory(containingFile,
				containingFile.FileAddressFromRVA((int)this.Header.MetaData.VirtualAddress)
				);
		}
		#endregion

		#region Properties
		public Cor20Header Header {
			get;
			set;
		}

		public MetadataDirectory Metadata {
			get;
			set;
		}
		#endregion
	}
}
