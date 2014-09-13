using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	using TheBoxSoftware.Reflection.Signitures;

	/// <summary>
	/// Allows the caller to obtain details of an event in a structured
	/// way.
	/// </summary>
	internal class EventSyntax : Syntax {
		private EventDef eventDef;
		private MethodDef add;
		private MethodDef remove;

		/// <summary>
		/// Initialises a new instance of the EventSyntax class.
		/// </summary>
		/// <param name="eventDef">The details of the event to get the information from.</param>
		public EventSyntax(EventDef eventDef) {
			this.eventDef = eventDef;
			this.add = eventDef.GetAddEventMethod();
			this.remove = eventDef.GetRemoveEventMethod();
		}

		public string GetIdentifier() {
			return this.eventDef.Name;
		}

		/// <summary>
		/// Obtains the visibility for the event.
		/// </summary>
		/// <returns>The visibility.</returns>
		/// <remarks>
		/// This visibility of an event, as with a property, is
		/// determined by the most accessible method.
		/// </remarks>
		public Visibility GetVisbility() {
			return this.eventDef.MemberAccess;
		}

		public Inheritance GetInheritance() {
			MethodDef method = this.add == null ? this.remove : this.add;
			Inheritance classInheritance = Inheritance.Default;
			if ((method.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Static) == TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Static) {
				classInheritance = Inheritance.Static;
			}
			else if ((method.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Abstract) == TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Abstract) {
				classInheritance = Inheritance.Abstract;
			}
			else if ((method.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Virtual) == TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Virtual) {
				classInheritance = Inheritance.Virtual;
			}
			return classInheritance;
		}

		public TypeDetails GetType() {
			MethodDef method = this.remove == null ? this.add : this.remove;
			ParamSignitureToken delegateType = (ParamSignitureToken)method.Signiture.GetParameterTokens()[0];
			TypeDetails details = delegateType.GetTypeDetails(method);
			return details;
		}
	}
}
