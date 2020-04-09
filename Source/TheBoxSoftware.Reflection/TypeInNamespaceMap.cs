
namespace TheBoxSoftware.Reflection
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Creates and manages a map of <see cref="TypeDef"/> instances in to namespaces.
    /// </summary>
    /// <seealso cref="AssemblyDef"/>
    internal class TypeInNamespaceMap
    {
        private Dictionary<string, List<TypeDef>> _typeInNamespace = new Dictionary<string, List<TypeDef>>();

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
        
        public List<string> GetAllNamespaces()
        {
            return _typeInNamespace.Keys.ToList();
        }

        /// <summary>
        /// Get a dictionary of namespaces with a list of types in those namespaces.
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