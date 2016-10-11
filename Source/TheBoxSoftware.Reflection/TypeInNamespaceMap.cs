using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// Internal mapping class that maps types in to there respective
    /// namespaces. The type itself is not returned only an index to the
    /// definition map.
    /// </summary>
    internal class TypeInNamespaceMap
    {
        private Dictionary<string, List<Entry>> _typeInNamespace = new Dictionary<string, List<Entry>>();
        private AssemblyDef _assembly;

        public TypeInNamespaceMap(AssemblyDef inAssembly)
        {
            _assembly = inAssembly;
        }

        /// <summary>
        /// Adds a new type to a namespaces with a reference to its associated metadata
        /// file.
        /// </summary>
        /// <param name="inNamespace">The namespace the type is defined in</param>
        /// <param name="table">The table storing the metadata</param>
        /// <param name="row">The row defining the details of the types metadata.</param>
        public void Add(string inNamespace, Core.COFF.MetadataTables table, Core.COFF.MetadataRow row)
        {
            if(!_typeInNamespace.ContainsKey(inNamespace))
            {
                _typeInNamespace.Add(inNamespace, new List<Entry>());
            }
            _typeInNamespace[inNamespace].Add(new Entry(table, row.FileOffset));
        }

        /// <summary>
        /// Returns a collection of all the namespaces defined in this map.
        /// </summary>
        /// <returns>The string collection of namespaces.</returns>
        public List<string> GetAllNamespaces()
        {
            return _typeInNamespace.Keys.ToList<string>();
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
            List<Entry> entries = _typeInNamespace[inNamespace];
            for(int i = 0; i < entries.Count; i++)
            {
                Entry currentEntry = entries[i];
                TypeDef def = _assembly.File.Map.GetDefinition(currentEntry.Table, currentEntry.FileOffset) as TypeDef;
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

        private class Entry
        {
            public Entry(Core.COFF.MetadataTables table, int fileOffset)
            {
                this.Table = table;
                this.FileOffset = fileOffset;
            }
            public Core.COFF.MetadataTables Table;
            public int FileOffset;
        }
    }
}