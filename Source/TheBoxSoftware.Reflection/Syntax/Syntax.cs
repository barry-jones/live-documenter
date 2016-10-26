
namespace TheBoxSoftware.Reflection.Syntax
{
    /// <summary>
    /// A base class for all Syntax classes that provides useful methods and
    /// properties for managing the required syntax information for concrete
    /// implementations.
    /// </summary>
    internal abstract class Syntax
    {
        /// <summary>
        /// Obtains the name of the type provided. This class will remove any superflous characters from 
        /// the name to return the name that the user will understand and had entered when creating the type.
        /// </summary>
        /// <param name="type">The type to get the name of.</param>
        /// <returns>A string representing the name of the type.</returns>
        /// <remarks>
        /// .NET Framework will add special characters to names to allow, for example, generic methods to 
        /// be overloaded. These characeters are removed in this method and the user defined name of the 
        /// type is returned.
        /// <example>
        /// // User creates type
        /// public class MyGenericType&lt;T&gt;
        /// // Framework outputs
        /// MyGenericType`1
        /// // Method returns
        /// MyGenericType
        /// </example>
        /// </remarks>
        protected string GetTypeName(TypeRef type)
        {
            string name = type.Name;
            if(type.IsGeneric)
            {
                name = name.Substring(0, name.IndexOf('`'));
            }
            return name;
        }

        protected Inheritance ConvertMethodInheritance(Core.COFF.MethodAttributes attributes)
        {
            Inheritance classInheritance = Inheritance.Default;

            if((attributes & Core.COFF.MethodAttributes.Static) == Core.COFF.MethodAttributes.Static)
            {
                classInheritance = Inheritance.Static;
            }
            else if((attributes & Core.COFF.MethodAttributes.Abstract) == Core.COFF.MethodAttributes.Abstract)
            {
                classInheritance = Inheritance.Abstract;
            }
            else if((attributes & Core.COFF.MethodAttributes.Virtual) == Core.COFF.MethodAttributes.Virtual)
            {
                classInheritance = Inheritance.Virtual;
            }

            return classInheritance;
        }

        protected Inheritance ConvertTypeInheritance(Core.COFF.TypeAttributes attributes)
        {
            Inheritance classInheritance = Inheritance.Default;

            if(
                (attributes & Core.COFF.TypeAttributes.Abstract) == Core.COFF.TypeAttributes.Abstract &&
                (attributes & Core.COFF.TypeAttributes.Sealed) == Core.COFF.TypeAttributes.Sealed
                )
            {
                classInheritance = Inheritance.Static;
            }
            else if((attributes & Core.COFF.TypeAttributes.Abstract) == Core.COFF.TypeAttributes.Abstract)
            {
                classInheritance = Inheritance.Abstract;
            }
            else if((attributes & Core.COFF.TypeAttributes.Sealed) == Core.COFF.TypeAttributes.Sealed)
            {
                classInheritance = Inheritance.Sealed;
            }

            return classInheritance;
        }
    }
}