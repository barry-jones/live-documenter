
namespace TheBoxSoftware.Reflection.Syntax
{
    using System;
    using Reflection.Signitures;

    /// <summary>
    /// Provides access to the important information for creating formatted
    /// syntax for <see cref="PropertyDef"/>s.
    /// </summary>
    internal class PropertySyntax : Syntax
    {
        private PropertyDef _propertyDef;
        private MethodDef _get;
        private MethodDef _set;

        /// <summary>
        /// Initialises a new instance of the EventSyntax class.
        /// </summary>
        /// <param name="propertyDef">The details of the event to get the information from.</param>
        public PropertySyntax(PropertyDef propertyDef)
        {
            _propertyDef = propertyDef;
            _get = propertyDef.GetMethod;
            _set = propertyDef.SetMethod;
        }

        public Visibility GetVisibility()
        {
            return _propertyDef.MemberAccess;
        }

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

        public Inheritance GetInheritance()
        {
            MethodDef method = _get ?? _set;
            return ConvertMethodInheritance(method.Attributes);
        }

        public new TypeDetails GetType()
        {
            TypeDetails details = null;
            if(_get != null)
            {
                ReturnTypeSignitureToken returnType = (ReturnTypeSignitureToken)_get.Signiture.Tokens.Find(
                    t => t.TokenType == SignitureTokens.ReturnType
                    );
                details = returnType.GetTypeDetails(_get);
            }
            else
            {
                ParamSignitureToken delegateType = _set.Signiture.GetParameterTokens()[0];
                details = delegateType.GetTypeDetails(_set);
            }
            return details;
        }

        public string GetIdentifier()
        {
            return _propertyDef.Name;
        }

        public MethodDef GetMethod
        {
            get { return _get; }
        }

        public MethodDef SetMethod
        {
            get { return _set; }
        }
    }
}