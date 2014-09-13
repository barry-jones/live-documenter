using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model.Diagram.SequenceDiagram {
	using TheBoxSoftware.Reflection;

	/// <summary>
	/// Represents a call from one object to another in a sequence diagram.
	/// </summary>
	internal class Call {
		public Call(MethodDef method, Activation from, out Activation next) {
			this.Name = method.Name;
			this.Reciever = Object.Create(method.Type.Name, method.Type.Name);
			this.RecievingActivation = next = this.Reciever.RecieveCall(this);
			if (from != null) {
				from.Calls.Add(this);
			}
		}

		/// <summary>
		/// The object that Called the reciever.
		/// </summary>
		public Activation Caller { get; set; }

		/// <summary>
		/// The object that recieves the Call
		/// </summary>
		public Object Reciever { get; set; }

		/// <summary>
		/// Gets or sets the name of the method being called on the reciever.
		/// </summary>
		public string Name { get; set; }

		public Activation RecievingActivation { get; set; }
	}
}
