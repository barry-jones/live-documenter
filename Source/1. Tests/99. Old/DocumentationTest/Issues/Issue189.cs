using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.Issues {
	/// <summary>
	/// Test for issue where a property named Item does not display it's
	/// syntax correctly.
	/// </summary>
	/// <remarks>
	/// This occurs because the Item property shares the same name as the Indexers
	/// generated methods.
	/// </remarks>
	public class Issue189 {
		/// <summary>
		/// Item property
		/// </summary>
		public string Item { get; set; }
	}
}
