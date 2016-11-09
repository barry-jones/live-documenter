
namespace TheBoxSoftware.Reflection
{
    using System.Collections.Generic;
    using System.Linq;

    /// <include file='code-documentation/reflection.xml' path='docs/typeinnamespace/member[@name="class"]'/>
    internal class TypeInNamespaceMap
    {
        private Dictionary<string, List<TypeDef>> _typeInNamespace = new Dictionary<string, List<TypeDef>>();

        public TypeInNamespaceMap()
        {
        }

        /// <include file='code-documentation/reflection.xml' path='docs/typeinnamespace/member[@name="add"]'/>
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
        
        /// <include file='code-documentation/reflection.xml' path='docs/typeinnamespace/member[@name="getallnamespaces"]'/>
        public List<string> GetAllNamespaces()
        {
            return _typeInNamespace.Keys.ToList();
        }

        /// <include file='code-documentation/reflection.xml' path='docs/typeinnamespace/member[@name="getalltypesinnamespaces"]'/>
        public Dictionary<string, List<TypeDef>> GetAllTypesInNamespaces()
        {
            Dictionary<string, List<TypeDef>> copy = new Dictionary<string, List<TypeDef>>();

            foreach(KeyValuePair<string, List<TypeDef>> current in _typeInNamespace)
            {
                copy.Add(current.Key, new List<TypeDef>(current.Value));
            }

            return copy;
        }

        /// <include file='code-documentation/reflection.xml' path='docs/typeinnamespace/member[@name="getalltypesinnamespaces"]'/>
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

        /// <include file='code-documentation/reflection.xml' path='docs/typeinnamespace/member[@name="containsnamespace"]'/>
        public bool ContainsNamespace(string theNamespace)
        {
            return _typeInNamespace.ContainsKey(theNamespace);
        }
    }
}