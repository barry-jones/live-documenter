using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection {
	using TheBoxSoftware.Reflection.Core.COFF;

	/// <summary>
	/// Represents a single event for a type. An event is made up from one or
	/// two MethodDef entries in the type. These are generally prefixed with
	/// add_ and remove_. This class allows those events to be treated as a
	/// single unit.
	/// </summary>
	/// <seealso cref="MethodDef"/>
	public sealed class EventDef : ReflectedMember {
		/// <summary>
		/// Factory method for instantiating an event from the details provided 
		/// in the metadata.
		/// </summary>
		/// <param name="assembly">The assembly the event is defined in.</param>
		/// <param name="container">The containing type for the event.</param>
		/// <param name="metadata">The metadata directory the details are stored in.</param>
		/// <param name="row">The row that provides access to the details for this event.</param>
		/// <returns>An instantiated EventDef instance.</returns>
		public static EventDef CreateFromMetadata(AssemblyDef assembly, 
				TypeDef container, 
				MetadataDirectory metadata,
				EventMetadataTableRow row) {
			EventDef createdEvent = new EventDef();

			createdEvent.Type = container;
			createdEvent.UniqueId = row.FileOffset;
			createdEvent.Name = assembly.StringStream.GetString(row.Name.Value);
			createdEvent.Assembly = assembly;

			return createdEvent;
		}

		#region Properties
		/// <summary>
		/// The name of the event as defined in the type.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The type which contains this event.
		/// </summary>
		public TypeDef Type { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Attemps to find the add method for this event from its
		/// containing type.
		/// </summary>
		/// <returns>
		/// The method defenition for add portion of the event or null
		/// if not found.
		/// </returns>
		public MethodDef GetAddEventMethod() {
			return this.Type.Methods.Find(
				method => method.Name == ("add_" + this.Name)
				);
		}

		/// <summary>
		/// Attemps to find the remove method for this event from its
		/// containing type.
		/// </summary>
		/// <returns>
		/// The method defenition for remove portion of the event or null
		/// if not found.
		/// </returns>
		public MethodDef GetRemoveEventMethod() {
			return this.Type.Methods.Find(
				method => method.Name == ("remove_" + this.Name)
				);
		}
		#endregion
	}
}
