using System;
using System.Collections.Generic;
using System.Diagnostics;
using TheBoxSoftware.Reflection.Core.COFF;
using TheBoxSoftware.Reflection.Signitures;

namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// Details a reference to a Type that resides in another assembly.
    /// </summary>
    [DebuggerDisplay("Type={ToString()}")]
    public class TypeRef : ReflectedMember
    {
        /// <field>
        /// A reference to the metadata resolution scope details, that will allow
        /// us to obtain the details to resolve this reference to its external entry.
        /// </field>
        private CodedIndex _resolutionScope;

        /// <summary>
        /// Initialises a new instance of the TypeRef class.
        /// </summary>
        /// <param name="references">A container of all the references required to build the typeref.</param>
        /// <param name="row">The metadata row that describes the type reference.</param>
        /// <returns>A reference to a TypeRef that represents the metadata row.</returns>
        internal static TypeRef CreateFromMetadata(BuildReferences references, TypeRefMetadataTableRow row)
        {
            TypeRef typeRef = new TypeRef();

            typeRef.UniqueId = references.Assembly.CreateUniqueId();
            typeRef.Name = references.Assembly.StringStream.GetString(row.Name.Value);
            typeRef.Namespace = references.Assembly.StringStream.GetString(row.Namespace.Value);
            typeRef.IsExternalReference = true;
            typeRef._resolutionScope = row.ResolutionScope;
            typeRef.IsGeneric = typeRef.Name.IndexOf('`') != -1;    // Must be a better way :/
            typeRef.Assembly = references.Assembly;
            typeRef.ExtensionMethods = new List<MethodDef>();

            return typeRef;
        }

        /// <summary>
        /// Returns the fully qualified name for this type
        /// </summary>
        /// <returns>A fully qualified name</returns>
        public string GetFullyQualifiedName()
        {
            return this.GetUniqueName();
        }

        /// <summary>
        /// Returns a nice display name for the type
        /// </summary>
        /// <param name="includeNamespace">
        /// Indicates wether or not the namespace should be included.
        /// </param>
        /// <returns>A string that is a nice representation of the type for display purposes.</returns>
        /// <remarks>
        /// This method will return a name that can be used to display to users of
        /// applications utilising this type.
        /// <example>
        /// Generic Type: List&lt;MyClass&gt;
        /// Array: MyClass[]
        /// Normal: MyClass
        /// </example>
        /// </remarks>
        public virtual string GetDisplayName(bool includeNamespace)
        {
            string name = string.Empty;
            if (this is TypeDef)
            {
                DisplayNameSignitureConvertor convertor = new DisplayNameSignitureConvertor((TypeDef)this, includeNamespace);
                return convertor.Convert();
            }
            else
            {
                if (includeNamespace)
                {
                    name = this.GetFullyQualifiedName();
                }
                else
                {
                    name = this.Name;
                }
                if (this.IsGeneric)
                {
                    if (this is TypeSpec)
                    {
                        TypeRef def = ((TypeSpec)this).TypeDetails.Type;
                        name = def.GetDisplayName(includeNamespace);
                    }
                    else
                    {
                        int count = int.Parse(name.Substring(name.IndexOf('`') + 1));
                        name = name.Substring(0, name.IndexOf('`'));
                        name += "<" + new String(',', count - 1) + ">";
                    }
                }
            }
            return name;
        }

        /// <summary>
        /// Returns a string representation of the TypeRef
        /// </summary>
        /// <returns>A string that represents the type reference.</returns>
        public override string ToString()
        {
            return string.Format("{0}{1}{2}", this.Namespace, string.IsNullOrEmpty(this.Namespace) ? string.Empty : ".", this.Name);
        }

        /// <summary>
        /// Returns the unique name for this definition
        /// </summary>
        /// <returns>A string</returns>
        internal string GetUniqueName()
        {
            return string.IsNullOrEmpty(this.Namespace) ? this.Name : this.Namespace + "." + this.Name;
        }

        /// <summary>
        /// Returns a reference to the extenral member that this reference can be
        /// resolved in. This will generally be a reference to the external 
        /// </summary>
        public virtual AssemblyRef ResolvingAssembly 
        {
            get 
            {
                AssemblyRef inheritsFrom = null;

                if (_resolutionScope.Index != 0)
                {
                    inheritsFrom = Assembly.ResolveCodedIndex(_resolutionScope) as AssemblyRef;
                }

                return inheritsFrom;
            }
        }

        /// <summary>
        /// The namespace in which this type resides.
        /// </summary>
        public virtual string Namespace { get; set; }

        /// <summary>
        /// Indicates if this TypeRef is an array instance or not
        /// </summary>
        public virtual bool IsArray { get; set; }

        /// <summary>
        /// TODO Should implement this properly for TypeDef, TypeRef entries
        /// </summary>
        public virtual bool IsGeneric { get; set; }

        /// <summary>
        /// Indicates wether or not this member is a reference to an external type.
        /// </summary>
        public virtual bool IsExternalReference { get; set; }

        /// <summary>
        /// Extension methods associated with this type.
        /// </summary>
        /// <remarks>
        /// Extension methods are defined on the TypeRef as this represents a reference which 
        /// is used in this library. So in essance all types loaded will be recorded. TypeDef
        /// derives for this type too, which means all assembly defined types will get this
        /// functionality.
        /// </remarks>
        public List<MethodDef> ExtensionMethods { get; set; }
    }
}