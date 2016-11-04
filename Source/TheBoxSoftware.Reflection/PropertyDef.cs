﻿
namespace TheBoxSoftware.Reflection
{
    using Core.COFF;

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
        /// Initialises a new instance of the PropertyDef class.
        /// </summary>
        /// <param name="references">An object that contains all of the required references to build the property.</param>
        /// <param name="container">The containing type definition.</param>
        /// <param name="row">The row that defines the details of the property.</param>
        /// <returns>The instantiated property.</returns>
        internal static PropertyDef CreateFromMetadata(BuildReferences references, TypeDef container, PropertyMetadataTableRow row)
        {
            PropertyDef property = new PropertyDef();

            property.UniqueId = row.FileOffset;
            property.OwningType = container;
            property.Name = references.Assembly.StringStream.GetString(row.NameIndex.Value);
            property.Assembly = references.Assembly;

            return property;
        }

        private bool IsPropertyAnIndexer()
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

        public override Visibility MemberAccess
        {
            get { return CalculateVisibility(); }
        }

        /// <summary>
        /// Indicates if this property is an Indexer
        /// </summary>
        public bool IsIndexer
        {
            get { return IsPropertyAnIndexer(); }
        }
    }
}