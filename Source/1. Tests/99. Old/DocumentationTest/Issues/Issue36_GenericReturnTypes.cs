
namespace DocumentationTest.Issues
{
    using System.Collections.Generic;

    class Issue36_GenericReturnTypes<T>
    {
        public List<T> ReturnGenericTypedList() => new List<T>() { };

        public List<string> ReturnWellKnownTypeList() => new List<string>() { };

        public Issue36_GenericReturnTypes<string> ReturnInLibraryGenericType() => null;
    }
}
