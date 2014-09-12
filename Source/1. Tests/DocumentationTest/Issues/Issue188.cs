using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest.Issues {
	/// <summary>
	/// Tests the problem reported in issue 188.
	/// </summary>
	public class Issue188 {
		/// <summary>
		/// This property defines both a getter and a setter
		/// </summary>
		public string GetAndSet { get; set; }
		
		/// <summary>
		/// This property only defines a setter
		/// </summary>
		public string SetNoGet { set {  } }

		/// <summary>
		/// This property has a getter and no setter.
		/// </summary>
		public string GetNoSet { get { return string.Empty; } }

		///// <summary>
		///// Test Item as a name
		///// </summary>
		//public string Item { get; set; }

		/// <summary>
		/// Multiple parameter indexer with only a setter
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int this[int index, int something] {
		    get { return 0; }
			set { }
		}

		/// <summary>
		/// Single parameter indexer with only a setter
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int this[int index] {
			get { return 0; }
			set {}
		}
	}
}
