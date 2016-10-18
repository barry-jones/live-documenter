using System.Collections.Generic;

namespace TheBoxSoftware.Reflection.Signitures
{
    /// <summary>
    /// Provides details about a type and its surounding information.
    /// </summary>
    internal class TypeDetails
    {
        /// <summary>
        /// The major type details this could be any TypeRef based class. Including
        /// generic parameter, TypeSpec and TypeDef and Ref instances.
        /// </summary>
        public TypeRef Type { get; set; }

        public bool IsArray { get; set; }

        public bool IsMultidemensionalArray { get; set; }

        public bool IsByRef { get; set; }

        public bool IsPointer { get; set; }

        /// <summary>
        /// The type details for the array. This is relevant when the type has the
        /// IsArray property set to true. Further this Type property is no longer
        /// valid.
        /// </summary>
        public TypeDetails ArrayOf { get; set; }

        /// <summary>
        /// Indicates that the <see cref="Type"/> is a generic type and has one or
        /// more arguments specified in <see cref="GenericParameters"/>.
        /// </summary>
        public bool IsGenericInstance { get; set; }

        public List<TypeDetails> GenericParameters { get; set; }

        public ArrayShapeSignitureToken ArrayShape { get; set; }
    }
}