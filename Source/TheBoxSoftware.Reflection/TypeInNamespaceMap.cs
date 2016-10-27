
namespace TheBoxSoftware.Reflection
{
    using System.Collections.Generic;
    using System.Linq;
    using TheBoxSoftware.Reflection.Core;

    /// <summary>
    /// Internal mapping class that maps types in to there respective
    /// namespaces. The type itself is not returned only an index to the
    /// definition map.
    /// </summary>
    internal class TypeInNamespaceMap
    {
        private Dictionary<string, List<TypeDef>> _typeInNamespace = new Dictionary<string, List<TypeDef>>();
        private PeCoffFile _peCoffFile;

        public TypeInNamespaceMap(AssemblyDef inAssembly)
        {
            _peCoffFile = inAssembly.File;
        }

        /// <summary>
        /// Adds a new type to a namespaces with a reference to its associated metadata
        /// file.
        /// </summary>
        /// <param name="type">The type being added</param>
        /// <param name="table">The table storing the metadata</param>
        /// <param name="row">The row defining the details of the types metadata.</param>
        public void Add(TypeDef type, Core.COFF.MetadataTables table, Core.COFF.MetadataRow row)
        {
            string inNamespace = type.Namespace;

            if(!_typeInNamespace.ContainsKey(inNamespace))
            {
                _typeInNamespace.Add(inNamespace, new List<TypeDef>());
            }
            _typeInNamespace[inNamespace].Add(type);
        }

        /// <summary>
        /// Returns a collection of all the namespaces defined in this map.
        /// </summary>
        /// <returns>The string collection of namespaces.</returns>
        public List<string> GetAllNamespaces()
        {
            return _typeInNamespace.Keys.ToList<string>();
        }

        public Dictionary<string, List<TypeDef>> GetAllTypesInNamespaces()
        {
            // crappy clone of the internal dictionary - we dont want people chaning it
            Dictionary<string, List<TypeDef>> copy = new Dictionary<string, List<TypeDef>>();

            foreach(KeyValuePair<string, List<TypeDef>> current in _typeInNamespace)
            {
                copy.Add(current.Key, new List<TypeDef>(current.Value));
            }

            return copy;
        }

        /// <summary>
        /// This is going to be a slow method and it will get slower the more types are
        /// defined in an assembly. This is a last resort method for finding types in
        /// an assembly.
        /// </summary>
        /// <param name="inNamespace">The namspace the type is in.</param>
        /// <param name="typeName">The name of the type to find</param>
        /// <returns>The found TypeDef or null if not found.</returns>
        public TypeDef FindTypeInNamespace(string inNamespace, string typeName)
        {
            if(!_typeInNamespace.ContainsKey(inNamespace))
            {
                return null;
            }
            List<TypeDef> entries = _typeInNamespace[inNamespace];
            for(int i = 0; i < entries.Count; i++)
            {
                TypeDef def = entries[i];
                if(def.Name == typeName)
                    return def;
            }

            return null;
        }

        /// <summary>
        /// Checks if 'theNamespace' specified is defined in this assembly.
        /// </summary>
        /// <param name="theNamespace">The namespace to check for.</param>
        /// <returns>True if it is else false.</returns>
        public bool ContainsNamespace(string theNamespace)
        {
            return _typeInNamespace.ContainsKey(theNamespace);
        }
    }
}