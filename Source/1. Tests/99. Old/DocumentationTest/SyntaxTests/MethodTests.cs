
namespace SyntaxTests
{
    public interface ITest
    {
        void DoIt();
    }

    public class ForMethod
    {
        // visibility tests
        public void PublicMethod() { }
        internal void InternalMethod() { }
        protected void ProtectedMethod() { }
        protected internal void ProtectedInternalMethod() { }
        private void PrivateMethod() { }


        // returns types
        public void ReturnVoid() { }
        public int ReturnWellKnownType() { return 0; }
        public int[][] ReturnJaggedArray() { return new int[0][]; }
        public ForMethod ReturnClass() { return new ForMethod(); }
        public byte[] ReturnArray() { return new byte[0]; }
        public DocumentationTest.GenericClass<string> ReturnGeneric() { return new DocumentationTest.GenericClass<string>(); }

        
        // static method
        public static void StaticMethod() { }


        // parameter modifiers
        public void ParameterNormal(int test) { }
        public void ParameterRef(ref int test) { }
        public void ParameterOut(out int test) { test = 0; }
        public void ParameterDefault(int test = 3) { }

        
        // generic methods
        public void Generic<T>() { }
        public void GenericWhereClass<T>() where T : class { }
        public void GenericWhereStruct<T>() where T : struct { }
        public void GenericWhereInterface<T>() where T : ITest { }
        public void GenericWhereNew<T>() where T : new() { }
        public void GenericWhereAll<T>() where T : class, ITest, new() { }
    }
}