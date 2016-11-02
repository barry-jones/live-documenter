
namespace TheBoxSoftware.Reflection.Syntax
{
    using System;
    using Reflection.Signitures;

    /// <summary>
    /// Provides access to details of an Indexor defined in the metadata.
    /// </summary>
    internal class IndexorSyntax : Syntax
    {
        private PropertyDef _propertyDef;
        private MethodDef _get;
        private MethodDef _set;

        /// <summary>
        /// Initialises a new instance of the EventSyntax class.
        /// </summary>
        /// <param name="propertyDef">The details of the event to get the information from.</param>
        public IndexorSyntax(PropertyDef propertyDef)
        {
            _propertyDef = propertyDef;
            _get = propertyDef.GetMethod;
            _set = propertyDef.SetMethod;
        }

        /// <summary>
        /// Obtains the Visibility of the member.
        /// </summary>
        /// <returns>An enumerated value representing the visibility of the member.</returns>
        public Visibility GetVisibility()
        {
            return _propertyDef.MemberAccess;
        }

        /// <summary>
        /// Obtains the Visibility of the getter method of the property.
        /// </summary>
        /// <returns>An enumerated value representing the visibility of the member.</returns>
        public Visibility GetGetterVisibility()
        {
            if(_get == null)
            {
                if(_set == null)
                {
                    InvalidOperationException ex = new InvalidOperationException(
                        "A property exists without a get or set method."
                        );
                    throw ex;
                }
                return GetSetterVisibility();
            }
            else
            {
                return _get.MemberAccess;
            }
        }

        /// <summary>
        /// Obtains the Visibility of the setter method of the property.
        /// </summary>
        /// <returns>An enumerated value representing the visibility of the member.</returns>
        public Visibility GetSetterVisibility()
        {
            if(_set == null)
            {
                if(_get == null)
                {
                    InvalidOperationException ex = new InvalidOperationException(
                        "A property exists without a get or set method."
                        );
                    throw ex;
                }
                return GetGetterVisibility();
            }
            else
            {
                return _set.MemberAccess;
            }
        }

        /// <summary>
        /// Obtains details of how this member is inherited in base classes.
        /// </summary>
        /// <returns>An enumerated value describing how the method is inherited.</returns>
        public Inheritance GetInheritance()
        {
            MethodDef method = _get ?? _set;
            return ConvertMethodInheritance(method.MethodAttributes);
        }

        /// <summary>
        /// Obtains a class that describes the details of types defined in this property.
        /// </summary>
        /// <returns>The details of the type.</returns>
        public new TypeDetails GetType()
        {
            TypeDetails details = null;
            if(_get != null)
            {
                ReturnTypeSignitureToken returnType = _get.Signiture.Tokens.Find(
                    t => t.TokenType == SignitureTokens.ReturnType
                    ) as ReturnTypeSignitureToken;
                details = returnType.GetTypeDetails(_get);
            }
            else
            {
                ParamSignitureToken delegateType = _set.Signiture.GetParameterTokens()[0];
                details = delegateType.GetTypeDetails(_set);
            }
            return details;
        }

        /// <summary>
        /// Obtains the name of the property. This will always be 'Item' for
        /// indexors.
        /// </summary>
        /// <returns>The identifier of the property</returns>
        public string GetIdentifier()
        {
            return _propertyDef.Name;
        }

        /// <summary>
        /// A reference to the get method for this property.
        /// </summary>
        public MethodDef GetMethod
        {
            get { return _get; }
        }

        /// <summary>
        /// A reference to the set method for this property.
        /// </summary>
        public MethodDef SetMethod
        {
            get { return _set; }
        }
    }
}