using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Pages.Elements {
	/// <summary>
	/// 
	/// </summary>
	public class TypeParamEntry : Block {
		public TypeParamEntry(string name, string description) {
			this.Resources.MergedDictionaries.Add(DocumentationResources.BaseResources);
			this.Param = name;
			this.Description = description;
		}

		public string Param { get; set; }
		public string Description { get; set; }
	}
}
