
namespace TheBoxSoftware.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using TheBoxSoftware.Reflection.Core.COFF;
    using TheBoxSoftware.Reflection.Signatures;

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
        private List<MethodDef> _extensionMethods;
        private bool _isExternalReference;
        private bool _isGeneric;
        private string _namespace;

        public TypeRef() { }

        /// <summary>
        /// Initiliases a new instance of the TypeRef class using the provided details.
        /// </summary>
        /// <param name="definingAssembly">The assembly which defines the type reference</param>
        /// <param name="name">The name of the type reference</param>
        /// <param name="namespaceName">The namespace it is defined in</param>
        /// <param name="resolutionScope">A CodedIndex determining the resolve the external reference</param>
        public TypeRef(AssemblyDef definingAssembly, string name, string namespaceName, CodedIndex resolutionScope)
        {
            UniqueId = definingAssembly.CreateUniqueId();
            Assembly = definingAssembly;
            Name = name;
            Namespace = namespaceName;
            _isExternalReference = true;
            _resolutionScope = resolutionScope;
            _isGeneric = name.IndexOf('`') != -1;
            _extensionMethods = new List<MethodDef>();
        }

        /// <summary>
        /// Returns the fully qualified name for this type
        /// </summary>
        /// <returns>A fully qualified name</returns>
        public string GetFullyQualifiedName()
        {
            return GetUniqueName();
        }

        /// <summary>
        /// Returns a nice display name for the type
        /// </summary>
        /// <param name="includeNamespace">Indicates wether or not the namespace should be included.</param>
        /// <include file='code-documentation/reflection.xml' path='docs/typeref/member[@name="getdisplayname"]'/>
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
                    name = GetFullyQualifiedName();
                }
                else
                {
                    name = Name;
                }
                if (IsGeneric)
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
            return string.Format("{0}{1}{2}", Namespace, string.IsNullOrEmpty(Namespace) ? string.Empty : ".", Name);
        }

        /// <summary>
        /// Returns the unique name for this definition
        /// </summary>
        /// <returns>A string</returns>
        internal string GetUniqueName()
        {
            return string.IsNullOrEmpty(Namespace) ? Name : Namespace + "." + Name;
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
        public virtual string Namespace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }

        /// <summary>
        /// TODO Should implement this properly for TypeDef, TypeRef entries
        /// </summary>
        public virtual bool IsGeneric
        {
            get { return _isGeneric; }
            set { _isGeneric = value; }
        }

        /// <summary>
        /// Indicates wether or not this member is a reference to an external type.
        /// </summary>
        public virtual bool IsExternalReference
        {
            get { return _isExternalReference; }
            set { _isExternalReference = value; }
        }

        /// <summary>
        /// Extension methods associated with this type.
        /// </summary>
        /// <include file='code-documentation/reflection.xml' path='docs/typeref/member[@name="extensionmethods"]'/>
        public List<MethodDef> ExtensionMethods
        {
            get { return _extensionMethods; }
            set { _extensionMethods = value; }
        }
    }
}