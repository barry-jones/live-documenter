using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Saxon.Api;
using System.IO;
using System.Threading;
using System.Runtime;
using System.Diagnostics;
using Microsoft.Win32;

namespace TheBoxSoftware.Documentation.Exporting {
	using TheBoxSoftware.Reflection;
	using TheBoxSoftware.Reflection.Comments;
	using TheBoxSoftware.Documentation.Exporting.Rendering;

	public sealed class HtmlHelp2Exporter : Exporter {
		private System.Text.RegularExpressions.Regex illegalFileCharacters;
		private string tempdirectory = string.Empty;
		private ExportConfigFile config = null;
		private int currentExportStep = 1;

		/// <summary>
		/// Initialises a new instance of the HtmlHelp1Exporter.
		/// </summary>
		/// <param name="currentFiles">The files to be exported.</param>
		/// <param name="settings">The settings for the export.</param>
		/// <param name="config">The export config file, from the LDEC container.</param>
		public HtmlHelp2Exporter(List<DocumentedAssembly> currentFiles, ExportSettings settings, ExportConfigFile config)
			: base(currentFiles, settings) {
			string regex = string.Format("{0}{1}",
				 new string(Path.GetInvalidFileNameChars()),
				 new string(Path.GetInvalidPathChars()));
						illegalFileCharacters = new System.Text.RegularExpressions.Regex(
							string.Format("[{0}]", System.Text.RegularExpressions.Regex.Escape(regex))
							);
			this.config = config;
		}
	}
}
