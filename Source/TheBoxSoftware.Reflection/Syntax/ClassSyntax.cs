﻿
namespace TheBoxSoftware.Reflection.Syntax
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a mechanism for obtaining useful syntax information for a class.
    /// </summary>
    internal sealed class ClassSyntax : Syntax
    {
        private readonly TypeDef _type;

        /// <summary>
        /// Initialises a new instance of the ClassSyntax class.
        /// </summary>
        /// <param name="type">The type to retrieve syntax details for.</param>
        public ClassSyntax(TypeDef type)
        {
            _type = type;
        }

        /// <summary>
        /// Obtains the identifier for the class.
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
        /// Obtains the names of all the interfaces this class implements.
        /// </summary>
        /// <returns>An array of strings identifying the interfaces.</returns>
        public Signatures.TypeDetails[] GetInterfaces()
        {
            int implentationCount = _type.Implements.Count;
            Signatures.TypeDetails[] interfaces = new Signatures.TypeDetails[implentationCount];

            for(int i = 0; i < implentationCount; i++)
            {
                if(_type.Implements[i] is TypeSpec)
                {
                    interfaces[i] = ((TypeSpec)_type.Implements[i]).TypeDetails;
                }
                else
                {
                    Signatures.TypeDetails details = new Signatures.TypeDetails();
                    details.Type = _type.Implements[i];
                    interfaces[i] = details;
                }
            }

            return interfaces;
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
        /// Obtains the name of the base type this class implements.
        /// </summary>
        /// <returns>The base class for the type.</returns>
        public Signatures.TypeDetails GetBaseClass()
        {
#if DEBUG
            if(_type.InheritsFrom == null)
            {
                System.Diagnostics.Debug.WriteLine($"class {_type.Name} has a null base type.");
                return null;
            }
#endif
            if(_type.InheritsFrom is TypeSpec)
            {
                return ((TypeSpec)_type.InheritsFrom).TypeDetails;
            }
            else
            {
                Signatures.TypeDetails details = new Signatures.TypeDetails();
                details.Type = _type.InheritsFrom;
                return details;
            }
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
            return ConvertTypeInheritance(_type.Flags);
        }

        /// <summary>
        /// Access to the class reference being represented by this syntax class.
        /// </summary>
        public TypeDef Class
        {
            get { return _type; }
        }
    }
}