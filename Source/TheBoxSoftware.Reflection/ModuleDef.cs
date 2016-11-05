
namespace TheBoxSoftware.Reflection
{
    using System;

    /// <summary>
    /// Provides a definition for a module in an assembly.
    /// </summary>
    public class ModuleDef : ReflectedMember
    {
        private Guid _moduleVersionId;

        public ModuleDef() { }

        public ModuleDef(string name, AssemblyDef definingAssembly, Guid moduleVersionId)
        {
            Name = name;
            Assembly = definingAssembly;
            UniqueId = definingAssembly.CreateUniqueId();
            _moduleVersionId = moduleVersionId;
        }

        /// <summary>
        /// The GUID that represents the Modules version identifier.
        /// </summary>
        public Guid ModuleVersionId
        {
            get { return _moduleVersionId; }
            set { _moduleVersionId = value; }
        }
    }
}