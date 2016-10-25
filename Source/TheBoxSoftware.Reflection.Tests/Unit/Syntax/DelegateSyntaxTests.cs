
namespace TheBoxSoftware.Reflection.Tests.Unit.Syntax
{
    using NUnit.Framework;
    using Reflection.Syntax;

    [TestFixture]
    public class DelegateSyntaxTests
    {
        // cant test delegates at the moment as they instantiate MethodSyntax which 
        // in turn request the Signature to be loaded - which we cant mock at the moment.
    }
}
