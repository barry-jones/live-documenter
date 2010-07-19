using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model.Diagram.SequenceDiagram {
	using TheBoxSoftware.Reflection;

	/// <summary>
	/// The class that represents the sequence diagram
	/// </summary>
	public class SequenceDiagram {
		/// <summary>
		/// Initialises a new instance of the Diagram class.
		/// </summary>
		/// <param name="method">The method to sequence.</param>
		public SequenceDiagram(MethodDef method) {
			//
			Object.Clear();
			this.Start = this.CreateCall(method, null);
		}

		#region Methods
		public List<Object> GetObjects() {
			return Object.GetObjects();
		}

		private Call CreateCall(MethodDef method, Activation from) {
			ILInstruction[] instructions = method.GetMethodBody().Instructions.ToArray();

			Activation next;
			Call currentCall = new Call(method, from, out next);
			if (from != null && from.Recieved != null) {
				currentCall.Caller = from.Recieved.RecievingActivation;
			}

			foreach (ILInstruction current in instructions) {
				if (current is InlineMethodILInstruction) {
					InlineMethodILInstruction methodCall = current as InlineMethodILInstruction;
					MemberRef calledMethod = methodCall.Method;
					if (calledMethod is MethodDef) {
						this.CreateCall((MethodDef)calledMethod, next);
					}
				}
			}
			return currentCall;
		}

		private Activation CreateActivation(Call call) {
			Activation activation = new Activation();
			activation.Recieved = call;
			return activation;
		}
		#endregion

		/// <summary>
		/// The start point for the sequence diagram.
		/// </summary>
		public Call Start { get; set; }
	}
}
