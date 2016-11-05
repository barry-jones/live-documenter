
namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// A class that describes a property that has been defined in an <see cref="TypeDef"/>.
    /// </summary>
    public sealed class PropertyDef : ReflectedMember
    {
        // name, assembly, uniqueid, memberaccess and attributes
        private TypeDef _owningType;
        private MethodDef _getMethod;
        private MethodDef _setMethod;

        public PropertyDef() { }

        /// <summary>
        /// Initialises an instance of the PropertyDef with the specified details.
        /// </summary>
        /// <param name="definingAssembly">The assembly in which the property is defined.</param>
        /// <param name="name">The name of the property</param>
        /// <param name="containingType">The type that the property is defined in.</param>
        public PropertyDef(AssemblyDef definingAssembly, string name, TypeDef containingType)
        {
            UniqueId = definingAssembly.CreateUniqueId();
            Assembly = definingAssembly;
            Name = name;
            _owningType = containingType;
        }

        /// <summary>
        /// Indicates if this property is an Indexer based on the Getter and Setter methods.
        /// </summary>
        public bool IsIndexer()
        {
            bool isIndexer = false;
            bool getHasParameters = _getMethod != null && _getMethod.Parameters.Count > 0;
            bool setHasParaemters = _setMethod != null && _setMethod.Parameters.Count > 0;

            if(getHasParameters || setHasParaemters)
            {
                isIndexer = true;
            }

            return isIndexer;
        }

        private Visibility CalculateVisibility()
        {
            Visibility setterVisibility = _setMethod != null ? _setMethod.MemberAccess : 0;
            Visibility getterVisibility = _getMethod != null ? _getMethod.MemberAccess : 0;

            // The more public, the greater the value of the visibilty enumeration value
            return (setterVisibility > getterVisibility) ? setterVisibility : getterVisibility;
        }

        /// <summary>
        /// Reference to the TypeDef where this property is defined.
        /// </summary>
        public TypeDef OwningType
        {
            get { return _owningType; }
            set { _owningType = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="MethodDef"/> which relates to the associated get method for the property. 
        /// </summary>
        public MethodDef Getter
        {
            get { return _getMethod; }
            set { _getMethod = value; }
        }

        /// <summary>
        /// Gest or sets the <see cref="MethodDef"/> which relates to the associated set method for the property. 
        /// </summary>
        public MethodDef Setter
        {
            get { return _setMethod; }
            set { _setMethod = value; }
        }

        /// <summary>
        /// Obtains the level of access for this member
        /// </summary>
        public override Visibility MemberAccess
        {
            get { return CalculateVisibility(); }
        }
    }
}