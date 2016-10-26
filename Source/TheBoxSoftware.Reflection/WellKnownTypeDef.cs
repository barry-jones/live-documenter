
namespace TheBoxSoftware.Reflection
{
    using System.Collections.Generic;

    public class WellKnownTypeDef : TypeRef
    {
        public WellKnownTypeDef(AssemblyDef assembly, string namespaceName, string name)
        {
            Name = name;
            Namespace = namespaceName;
            Assembly = assembly;
            ExtensionMethods = new List<MethodDef>();
            IsExternalReference = true;
        }
    }
}
