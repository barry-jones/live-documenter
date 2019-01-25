
namespace DocumentationTest
{
    class RefReturnTypeTest
    {
        private string aname = "aname";

        public ref string GetName()
        {
            return ref aname;
        }
    }
}
