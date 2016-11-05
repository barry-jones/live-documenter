
namespace TheBoxSoftware.Reflection
{
    using Core.COFF;

    /// <summary>
    /// Represents a single event for a type. An event is made up from one or two MethodDef entries 
    /// in the type. These are generally prefixed with add_ and remove_. This class allows those 
    /// events to be treated as a single unit.
    /// </summary>
    /// <seealso cref="MethodDef"/>
    public sealed class EventDef : ReflectedMember
    {
        private TypeDef _containingType;

        public EventDef() { }

        public EventDef(string name, AssemblyDef definingAssembly, TypeDef containingType)
        {
            Name = name;
            Assembly = definingAssembly;
            UniqueId = definingAssembly.CreateUniqueId();
            _containingType = containingType;
        }
        
        /// <summary>
        /// Attemps to find the add method for this event from its containing type.
        /// </summary>
        /// <returns>
        /// The method defenition for add portion of the event or null if not found.
        /// </returns>
        public MethodDef FindAddMethod()
        {
            // build the event name, some event have full namespaces declared
            string eventName = this.GetInternalName("add");

            return this.Type.Methods.Find(method => method.Name == eventName);
        }

        /// <summary>
        /// Attemps to find the remove method for this event from its containing type.
        /// </summary>
        /// <returns>
        /// The method defenition for remove portion of the event or null if not found.
        /// </returns>
        public MethodDef FindRemoveMethod()
        {
            // build the event name, some event have full namespaces declared
            string eventName = this.GetInternalName("remove");

            return this.Type.Methods.Find(method => method.Name == eventName);
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
            return $"{addOrRemove}_{this.Name.Substring(this.Name.LastIndexOf('.') + 1)}";
        }

        /// <summary>
        /// The type which contains this event.
        /// </summary>
        public TypeDef Type
        {
            get { return _containingType; }
            set { _containingType = value; }
        }

        public override Visibility MemberAccess
        {
            get
            {
                int addVisibility = 0;
                int removeVisibility = 0;
                MethodDef addMethod = this.FindAddMethod();
                MethodDef removeMethod = this.FindRemoveMethod();
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
    }
}
