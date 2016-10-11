using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection
{
    public class WellKnownTypeDef : TypeRef
    {
        public WellKnownTypeDef(AssemblyDef assembly, string namespaceName, string name)
        {
            this.Name = name;
            this.Namespace = namespaceName;
            this.Assembly = assembly;
            this.ExtensionMethods = new List<MethodDef>();
            this.IsExternalReference = true;
        }
    }
}
