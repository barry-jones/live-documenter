
namespace SyntaxTests
{
    public interface ITest
    {
        void DoIt();
    }

    // visibility class tests
    public class ClassPublic
    {
        protected class ClassProtected { }

        private class ClassPrivate { }
    }

    internal class ClassInternal { }

    
    // base class tests
    public class BaseClass { }

    public class DerivedClass : BaseClass { }

    public class DerivedClassWithInterface : BaseClass, ITest
    {
        public void DoIt() { }
    }


    // generic class tests
    public class GenericClass<T> { }

    public class GenericClass<T, U> { }

    public class GenericClassWhereNew<T> where T : new() { }

    public class GenericClassWhereStruct<T> where T : struct { }

    public class GenericClassWhereClass<T> where T : class { }

    public class GenericClassWhereClassAndInteface<T> where T : class, ITest { }

    public class GenericClassWhereClassAll<T> where T : class, ITest, new() { }

    
    // modifier tests
    public abstract class AbstractClass { }

    public sealed class SealedClass { }

    public static class StaticClass { }

    public class ContainerA
    {
        public class NestedClass { }
    }

    public class ContainerB : ContainerA
    {
        public new class NestedClass { }
    }
}
