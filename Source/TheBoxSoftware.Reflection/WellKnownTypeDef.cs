
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

        public static WellKnownTypeDef Boolean = new WellKnownTypeDef(null, "System", "Boolean");
        public static WellKnownTypeDef I = new WellKnownTypeDef(null, "System", "IntPtr");
        public static WellKnownTypeDef I1 = new WellKnownTypeDef(null, "System", "SByte");
        public static WellKnownTypeDef I2 = new WellKnownTypeDef(null, "System", "Int16");
        public static WellKnownTypeDef I4 = new WellKnownTypeDef(null, "System", "Int32");
        public static WellKnownTypeDef I8 = new WellKnownTypeDef(null, "System", "Int64");
        public static WellKnownTypeDef U = new WellKnownTypeDef(null, "System", "UIntPtr");
        public static WellKnownTypeDef U1 = new WellKnownTypeDef(null, "System", "Byte");
        public static WellKnownTypeDef U2 = new WellKnownTypeDef(null, "System", "UInt16");
        public static WellKnownTypeDef U4 = new WellKnownTypeDef(null, "System", "UInt32");
        public static WellKnownTypeDef U8 = new WellKnownTypeDef(null, "System", "UInt64");
        public static WellKnownTypeDef Char = new WellKnownTypeDef(null, "System", "Char");
        public static WellKnownTypeDef R4 = new WellKnownTypeDef(null, "System", "Single");
        public static WellKnownTypeDef R8 = new WellKnownTypeDef(null, "System", "Double");
        public static WellKnownTypeDef TypedByRef = new WellKnownTypeDef(null, "System", "TypedReference");
        public static WellKnownTypeDef String = new WellKnownTypeDef(null, "System", "String");
        public static WellKnownTypeDef Object = new WellKnownTypeDef(null, "System", "Object");
        public static WellKnownTypeDef Void = new WellKnownTypeDef(null, "System", "Void");
    }
}
