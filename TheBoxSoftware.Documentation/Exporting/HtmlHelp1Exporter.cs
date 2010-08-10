using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation.Exporting {
	public sealed class HtmlHelp1Exporter : Exporter {
		public HtmlHelp1Exporter(List<DocumentedAssembly> currentFiles, ExportSettings settings)
			: base(currentFiles, settings) {
		}

		protected override void GenerateDocumentMap() {
			throw new NotImplementedException();
		}

		public override void Export() {
			base.Export();
		}
	}
}
