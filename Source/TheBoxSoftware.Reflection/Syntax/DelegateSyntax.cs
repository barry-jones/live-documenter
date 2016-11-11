
namespace TheBoxSoftware.Reflection.Syntax
{
    using System;
    using System.Collections.Generic;
    using Reflection.Signatures;

    internal class DelegateSyntax : Syntax
    {
        private TypeDef _type;
        // All delegates have an invoke method and this seems to detail the return type
        // and parameters for the delegate. So here we will store a loaded syntax instance
        // containing those details.
        private MethodSyntax _invokeMethodSyntax;

        /// <summary>
        /// Initialises a new instance of the ClassSyntax class.
        /// </summary>
        /// <param name="type">The type to retrieve syntax details for.</param>
        public DelegateSyntax(TypeDef type)
        {
            _type = type;

            MethodDef invokeMethod = _type.Methods.Find(m => m.Name == "Invoke");
            _invokeMethodSyntax = new MethodSyntax(invokeMethod);
        }

        /// <summary>
        /// Obtains the visibility modifier for the class.
        /// </summary>
        /// <returns>The visibility modifier for the class.</returns>
        public Visibility GetVisibility()
        {
            return _type.MemberAccess;
        }

        public TypeDetails GetReturnType()
        {
            return _invokeMethodSyntax.GetReturnType();
        }

        public List<ParameterDetails> GetParameters()
        {
            return _invokeMethodSyntax.GetParameters();
        }

        /// <summary>
        /// Obtains details of the generic parameters associated with this type.
        /// </summary>
        /// <returns>The array of generic parameters.</returns>
        /// <remarks>
        /// This method is only really valid when the type <see cref="TypeRef.IsGeneric"/>
        /// property has been set to true.
        /// </remarks>
        public List<GenericTypeRef> GetGenericParameters()
        {
            return _type.GenericTypes;
        }

        /// <summary>
        /// Obtains the identifier for the delegate.
        /// </summary>
        /// <returns>The type identifier.</returns>
        public string GetIdentifier()
        {
            string name = _type.Name;

            if(_type.IsGeneric)
            {
                name = name.Substring(0, name.IndexOf('`'));
            }

            return name;
        }

        /// <summary>
        /// Access to the class reference being represented by this syntax class.
        /// </summary>
        public TypeDef Class
        {
            get { return _type; }
        }

        public MethodSyntax Method
        {
            get { return _invokeMethodSyntax; }
        }
    }
}