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
	public class EventSyntax : Syntax {
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
			Visibility addVisibility = Visibility.Private;
			Visibility removeVisibility = Visibility.Private;
			if (add != null) {
				switch (this.add.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.MemberAccessMask) {
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Public:
						addVisibility = Visibility.Public;
						break;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Assem:
						addVisibility = Visibility.Internal;
						break;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.FamANDAssem:
						addVisibility = Visibility.InternalProtected;
						break;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Family:
						addVisibility = Visibility.Protected;
						break;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Private:
						addVisibility = Visibility.Private;
						break;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.FamORAssem:
						addVisibility = Visibility.Internal;
						break;
					default:
						addVisibility = Visibility.Public;
						break;
				}
			}
			if (remove != null) {
				switch (this.remove.Attributes & TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.MemberAccessMask) {
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Public:
						removeVisibility = Visibility.Public; break;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Assem:
						removeVisibility = Visibility.Internal; break;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.FamANDAssem:
						removeVisibility = Visibility.InternalProtected; break;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Family:
						removeVisibility = Visibility.Protected; break;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.Private:
						removeVisibility = Visibility.Private; break;
					case TheBoxSoftware.Reflection.Core.COFF.MethodAttributes.FamORAssem:
						removeVisibility = Visibility.Internal; break;
					default:
						removeVisibility = Visibility.Public; break;
				}
			}

			return ((int)addVisibility > (int)removeVisibility)
				? addVisibility
				: removeVisibility;
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
