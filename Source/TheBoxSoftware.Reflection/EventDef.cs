
namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// Represents a single event for a type. An event is made up from one or two MethodDef entries 
    /// in the type. These are generally prefixed with add_ and remove_. This class allows those 
    /// events to be treated as a single unit.
    /// </summary>
    /// <seealso cref="MethodDef"/>
    public sealed class EventDef : ReflectedMember
    {
        private readonly TypeDef _containingType;

        /// <summary>
        /// Initialises a new instance of EventDef with the provided details.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="definingAssembly">The assembly the event is defined in.</param>
        /// <param name="containingType">The type the event is declared in.</param>
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
            string eventName = GetInternalName("add");
            return Type.Methods.Find(method => method.Name == eventName);
        }

        /// <summary>
        /// Attemps to find the remove method for this event from its containing type.
        /// </summary>
        /// <returns>
        /// The method defenition for remove portion of the event or null if not found.
        /// </returns>
        public MethodDef FindRemoveMethod()
        {
            string eventName = GetInternalName("remove");
            return Type.Methods.Find(method => method.Name == eventName);
        }

        /// <summary>
        /// Gets a version of the events name that can be checked against the events method
        /// names (add, remove) in the owning type.
        /// </summary>
        /// <returns>A string containing the implementing method name</returns>
        private string GetInternalName(string addOrRemove)
        {
            // method names are in the format add_Name and remove_Name.
            return addOrRemove + "_" + Name.Substring(Name.LastIndexOf('.') + 1);
        }

        private Visibility CalculateVisibility()
        {
            Visibility addVisibility = 0;
            Visibility removeVisibility = 0;
            MethodDef addMethod = FindAddMethod();
            MethodDef removeMethod = FindRemoveMethod();
            if(addMethod != null)
            {
                addVisibility = addMethod.MemberAccess;
            }
            if(removeMethod != null)
            {
                removeVisibility = removeMethod.MemberAccess;
            }

            // The more public, the greater the number
            return (addVisibility > removeVisibility)
                ? addVisibility
                : removeVisibility;
        }

        /// <summary>
        /// The type which contains this event.
        /// </summary>
        public TypeDef Type
        {
            get { return _containingType; }
        }

        public override Visibility MemberAccess
        {
            get { return CalculateVisibility(); }
        }
    }
}
