using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest {
	/// <summary>
	/// Documentation for a public interface
	/// </summary>
	public interface DocumentedInterface {
		/// <summary>
		/// Documentation for an interfaces property
		/// </summary>
		string Something { get; set; }

		/// <summary>
		/// Documentation for an interfaces method
		/// </summary>
		/// <myown cref="Something">
		/// This is something I decided to add, a myown element.
		/// </myown>
		void DoSomething();
	}
}
