using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model.Diagram.SequenceDiagram {
	/// <summary>
	/// Represents a period of activation (work) for an object.
	/// </summary>
	public class Activation {
		public Activation() {
			this.Calls = new List<Call>();
		}

		/// <summary>
		/// The call recieved that started the activation.
		/// </summary>
		public Call Recieved { get; set; }

		/// <summary>
		/// The sequence of calls made by the object during this period of activation.
		/// </summary>
		public List<Call> Calls { get; set; }
	}
}
