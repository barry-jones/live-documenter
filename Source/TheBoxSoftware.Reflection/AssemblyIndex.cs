
using System.Collections.Generic;

namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// An index class that maps relationships between the high level defenitions and
    /// low level metadata.
    /// </summary>
    internal class AssemblyIndex
    {
        private TypeInNamespaceMap _typeMap;
        private MetadataToDefinitionMap _metedataMap;

        public AssemblyIndex()
        {
        }

        public Dictionary<string, List<TypeDef>> GetTypesInNamespaces()
        {
            return _typeMap.GetAllTypesInNamespaces();
        }

        public List<string> GetNamespaces()
        {
            return _typeMap.GetAllNamespaces();
        }

        public TypeDef FindType(string theNamespace, string theTypeName)
        {
            if(string.IsNullOrEmpty(theTypeName) || string.IsNullOrEmpty(theNamespace)) return null;
            return _typeMap.FindTypeInNamespace(theNamespace, theTypeName);
        }

        internal TypeInNamespaceMap TypeMap
        {
            get { return _typeMap; }
            set { _typeMap = value; }
        }

        internal MetadataToDefinitionMap MetadataMap
        {
            get { return _metedataMap; }
            set { _metedataMap = value; }
        }
    }
}
