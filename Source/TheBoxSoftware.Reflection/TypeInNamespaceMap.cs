
namespace TheBoxSoftware.Reflection
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Internal mapping class that maps types into there respective namespaces. The type itself
    /// is not returned only an index to the defenition map.
    /// </summary>
    internal class TypeInNamespaceMap
    {
        private Dictionary<string, List<TypeDef>> _typeInNamespace = new Dictionary<string, List<TypeDef>>();

        /// <summary>
        /// Adds a new type to a namespace with a reference to its associated metadata file.
        /// </summary>
        /// <param name="type">The type being added</param>
        public void Add(TypeDef type)
        {
            string inNamespace = type.Namespace;

            if(!_typeInNamespace.ContainsKey(inNamespace))
            {
                _typeInNamespace.Add(inNamespace, new List<TypeDef>());
            }
            _typeInNamespace[inNamespace].Add(type);
        }

        public void Remove(TypeDef type)
        {
            string inNamespace = type.Namespace;

            if(_typeInNamespace.ContainsKey(inNamespace))
            {
                _typeInNamespace[inNamespace].Remove(type);

                if(_typeInNamespace[inNamespace].Count == 0)
                {
                    _typeInNamespace.Remove(inNamespace);
                }
            }
        }
        
        /// <summary>
        /// Returns all the namespaces in the map.
        /// </summary>
        public List<string> GetAllNamespaces()
        {
            return _typeInNamespace.Keys.ToList();
        }

        /// <summary>
        /// Returns all the types in their respective namespaces.
        /// </summary>
        /// <returns>The dictionary of types in namespaces.</returns>
        public Dictionary<string, List<TypeDef>> GetAllTypesInNamespaces()
        {
            Dictionary<string, List<TypeDef>> copy = new Dictionary<string, List<TypeDef>>();

            foreach(KeyValuePair<string, List<TypeDef>> current in _typeInNamespace)
            {
                copy.Add(current.Key, new List<TypeDef>(current.Value));
            }

            return copy;
        }

        /// <summary>
        /// Searches for the <paramref name="typeName"/> in the <paramref name="inNamespace"/>.
        /// </summary>
        /// <param name="inNamespace">The namespace to search in.</param>
        /// <param name="typeName">The type name to search for.</param>
        /// <returns>The found type or null if not found.</returns>
        public TypeDef FindTypeInNamespace(string inNamespace, string typeName)
        {
            if(inNamespace == null || !_typeInNamespace.ContainsKey(inNamespace))
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
        /// Checks if the <paramref name="theNamespace"/> is in the map.
        /// </summary>
        /// <param name="theNamespace">The namespaces name to check for.</param>
        /// <returns>True if found else false.</returns>
        public bool ContainsNamespace(string theNamespace)
        {
            return _typeInNamespace.ContainsKey(theNamespace);
        }
    }
}