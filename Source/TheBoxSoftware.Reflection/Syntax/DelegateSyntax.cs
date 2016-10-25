
namespace TheBoxSoftware.Reflection.Syntax
{
    using System;
    using System.Collections.Generic;
    using Reflection.Signitures;

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

        /// <summary>
        /// Obtains the inheritance modifier for the class.
        /// </summary>
        /// <returns>The inheritance modifier.</returns>
        /// <remarks>
        /// Although the language specification does not specify a static modifier here,
        /// classes which are defined as both abstract and sealed seems to be the way to
        /// define the static modifier in the metadata. That is a static class can not have
        /// instances created and can not be derived from.
        /// </remarks>
        public Inheritance GetInheritance()
        {
            Inheritance classInheritance = Inheritance.Default;

            if(
                (_type.Flags & Core.COFF.TypeAttributes.Abstract) == Core.COFF.TypeAttributes.Abstract &&
                (_type.Flags & Core.COFF.TypeAttributes.Sealed) == Core.COFF.TypeAttributes.Sealed
                )
            {
                classInheritance = Inheritance.Static;
            }
            else if((_type.Flags & Core.COFF.TypeAttributes.Abstract) == Core.COFF.TypeAttributes.Abstract)
            {
                classInheritance = Inheritance.Abstract;
            }
            else if((_type.Flags & Core.COFF.TypeAttributes.Sealed) == Core.COFF.TypeAttributes.Sealed)
            {
                classInheritance = Inheritance.Sealed;
            }

            return classInheritance;
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