using TheBoxSoftware.Reflection.Core.COFF;

namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// Represents a single event for a type. An event is made up from one or
    /// two MethodDef entries in the type. These are generally prefixed with
    /// add_ and remove_. This class allows those events to be treated as a
    /// single unit.
    /// </summary>
    /// <seealso cref="MethodDef"/>
    public sealed class EventDef : ReflectedMember
    {
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
                EventMetadataTableRow row)
        {
            EventDef createdEvent = new EventDef();

            createdEvent.Type = container;
            createdEvent.UniqueId = row.FileOffset;
            createdEvent.Name = assembly.StringStream.GetString(row.Name.Value);
            createdEvent.Assembly = assembly;

            return createdEvent;
        }

        /// <summary>
        /// The type which contains this event.
        /// </summary>
        public TypeDef Type { get; set; }

        public override Visibility MemberAccess
        {
            get
            {
                int addVisibility = 0;
                int removeVisibility = 0;
                MethodDef addMethod = this.GetAddEventMethod();
                MethodDef removeMethod = this.GetRemoveEventMethod();
                if(addMethod != null)
                {
                    addVisibility = (int)addMethod.MemberAccess;
                }
                if(removeMethod != null)
                {
                    removeVisibility = (int)removeMethod.MemberAccess;
                }

                // The more public, the greater the number
                return (addVisibility > removeVisibility)
                    ? (Visibility)addVisibility
                    : (Visibility)removeVisibility;
            }
        }

        /// <summary>
        /// Gets a version of the events name that can be checked against the events method
        /// names (add, remove) in the owning type.
        /// </summary>
        /// <returns>A string containing the implementing method name</returns>
        private string GetInternalName(string addOrRemove)
        {
            // build the event name, some event have full namespaces declared
            // [#109] this is a bug fix but should be implemented properly when the eventdef is loaded
            return string.Format("{1}_{0}", this.Name.Substring(this.Name.LastIndexOf('.') + 1), addOrRemove);
        }

        /// <summary>
        /// Attemps to find the add method for this event from its
        /// containing type.
        /// </summary>
        /// <returns>
        /// The method defenition for add portion of the event or null
        /// if not found.
        /// </returns>
        public MethodDef GetAddEventMethod()
        {
            // build the event name, some event have full namespaces declared
            string eventName = this.GetInternalName("add");

            return this.Type.Methods.Find(method => method.Name == eventName);
        }

        /// <summary>
        /// Attemps to find the remove method for this event from its
        /// containing type.
        /// </summary>
        /// <returns>
        /// The method defenition for remove portion of the event or null
        /// if not found.
        /// </returns>
        public MethodDef GetRemoveEventMethod()
        {
            // build the event name, some event have full namespaces declared
            string eventName = this.GetInternalName("remove");

            return this.Type.Methods.Find(method => method.Name == eventName);
        }
    }
}
