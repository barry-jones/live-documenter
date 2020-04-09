
namespace DocumentationTest
{
    /// <summary>
    /// Class that tests that marker interfaces are displayed in the documentation.
    /// </summary>
    public class MarkerInterfaces : IMarkerInterface
    {
    }

    /// <summary>
    /// A marker interface does not provide any methods and is only for
    /// checks in code.
    /// </summary>
    public interface IMarkerInterface { }
}
