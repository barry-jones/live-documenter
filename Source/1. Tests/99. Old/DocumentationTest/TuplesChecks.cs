
namespace DocumentationTest
{
    public class TuplesChecks
    {
        public (string, string) UnamedTuplesReturned()
        {
            return (string.Empty, string.Empty);
        }

        public (int first, int second) NamedTuplesReturned()
        {
            return (1, 2);
        }
    }
}
