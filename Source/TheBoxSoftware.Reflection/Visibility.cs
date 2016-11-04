
namespace TheBoxSoftware.Reflection
{
    /// <summary>
    /// Visibility flags that apply to all members in the reflection namespace.
    /// </summary>
    public enum Visibility : byte
    {
        NotApplicable       = 0,
        Private             = 1,
        Protected           = 2,
        InternalProtected   = 3,
        Internal            = 4,
        Public              = 5
    }
}